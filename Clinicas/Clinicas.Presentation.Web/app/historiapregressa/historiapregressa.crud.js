(function () {
    'use strict';

    angular
        .module('app.historia')
        .controller('CrudHistoria', CrudHistoria);

    CrudHistoria.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.prontuario', 'paciente', 'id'];

    function CrudHistoria($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, dsProntuario, paciente, id) {

        var vm = this;
        vm.State = "Incluir História Pregressa";
        vm.FormMessage = "";
        vm.historia = {
            Paciente: undefined
        };
        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.historia.Paciente = paciente.NmPaciente;

            if (id > 0) {
                vm.State = "Editar História Pregressa";
                var blocker = blockUI.instances.get('blockModalHistoria');
                blocker.start();

                dsProntuario
                    .getHistoriaById(id)
                    .then(function (result) {
                        vm.historia = result.data;
                        vm.historia.Paciente = paciente.NmPaciente;

                        if (result.data.Situacao == 'Ativo')
                            vm.historia.Situacao = "A";
                        else
                            vm.historia.Situacao = "I";

                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.historia.Situacao = "A";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.historia);

            if (vm.formValid) {

                vm.historia.IdPaciente = paciente.IdPaciente;
                vm.FormMessage = "";
               

                var blocker = blockUI.instances.get('blockModalHistoria');
                blocker.start();

                dsProntuario
                    .saveHistoria(vm.historia)
                    .then(function (result) {
                        if (id == 0)
                            notification.showSuccessBar("História Pregressa cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("História Pregressa alterada com sucesso!");

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