(function () {
    'use strict';

    angular
        .module('app.consultorio')
        .controller('CrudConsultorio', CrudConsultorio);

    CrudConsultorio.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'id'];

    function CrudConsultorio($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice, id) {

        var vm = this;
        vm.State = "Incluir Consultório";
        vm.FormMessage = "";

        $scope.forms = {};
        vm.formValid = true;
        vm.FormMessage = "";

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {

            cadastroservice
                .listarUnidadesAtendimento()
                .then(function (result) {
                    vm.unidades = result.data;
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
            if(id>0){

                vm.State = "Editar Consultório";
                cadastroservice.obterConsultorioPorId(id)
                        .then(function (result) {
                            vm.consultorio = result.data;
                            vm.unidadeAtendimentoSelecionada = vm.consultorio.IdUnidadeAtendimento;
                        }).catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() 
        {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.consultorio.$valid) {

                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalConsultorio');
                blocker.start();
                vm.consultorio.IdUnidadeAtendimento = vm.unidadeAtendimentoSelecionada;

                cadastroservice
                    .salvarConsultorio(vm.consultorio)
                    .then(function (result) {
                        vm.consultorio = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }
    }
})();