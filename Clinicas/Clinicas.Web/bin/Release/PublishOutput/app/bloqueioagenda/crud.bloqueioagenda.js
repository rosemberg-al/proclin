(function () {
    'use strict';

    angular
        .module('app.bloqueio')
        .controller('CrudAusenciaController', CrudAusenciaController);

    CrudAusenciaController.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'agendaservice', 'id'];

    function CrudAusenciaController($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, agendaservice, id) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;
        vm.State = "Incluir Bloqueio de Agenda";
        vm.bloqueio = {};
        vm.edit = false;
        vm.aceito = true;
        vm.retorno = [];

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            
            var pFuncionarios = cadastroservice.listarProfissionaisSaude();
            pFuncionarios.then(function (result) {
                vm.funcionarios = result.data;
            });

            vm.motivos = [
                { Key: "Compromisso Profissional", Value: "Compromisso Profissional" },
                { Key: "Congresso", Value: "Congresso" },
                { Key: "Outro", Value: "Outro" },
                { Key: "Ferias", Value: "Férias" },
            ];

            $q.all([pFuncionarios]).then(function () {
                if (id > 0) {
                    vm.edit = true;
                    vm.State = "Editar Bloqueio de Agenda";
                    agendaservice
                        .obterBloqueioAgenda(id)
                        .then(function (result) {
                            vm.bloqueio = result.data;
                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                        });
                }

            })['finally'](function () {
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save(id) {
            $scope.showErrorsCheckValidity = true;

            var form = $scope.forms.addbloqueio.$valid;
            if (form) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalBloq');
                blocker.start();
                agendaservice
                    .salvarBloqueioAgenda(vm.bloqueio)
                    .then(function (result) {
                        if (result.data.Aceito) {//foi aceito o bloqueio
                            vm.aceito = true;
                            if (id > 0)
                                notification.showSuccessBar("Alteração realizada com sucesso");
                            else
                                notification.showSuccessBar("Cadastro realizado com sucesso");

                            $modalInstance.close();
                        }
                        else {//já existe pacientes para este bloqueio
                            vm.aceito = false;
                            vm.retorno = result.data.PacientesMarcados;
                        }
                    })['finally'](function () {
                        blocker.stop();
                    }).catch(function (ex) {
                        notification.showError(ex.data.Message);
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }
    }
})();