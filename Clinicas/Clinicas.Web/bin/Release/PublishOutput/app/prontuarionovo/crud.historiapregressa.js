(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('CrudHistoriaPregressa', CrudHistoriaPregressa);

    CrudHistoriaPregressa.$inject = ['$scope', '$http', '$q',  '$modal','$modalInstance',  'DTOptionsBuilder', 'blockUI', 'common', 'notification','cadastroservice', 'pacienteservice','prontuarioservice','idpaciente'];

    function CrudHistoriaPregressa($scope, $http, $q,  $modal, $modalInstance,  DTOptionsBuilder, blockUI, common, notification,cadastroservice, pacienteservice,prontuarioservice,idpaciente) {

        var vm = this;
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        init();
        
        function init() {

             var blocker = blockUI.instances.get('blockModalHP');
            blocker.start();

             pacienteservice
                .getPacienteById(idpaciente)
                .then(function (result) {
                    vm.paciente = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 

            cadastroservice
                .listarProfissionaisSaude()
                .then(function (result) {
                    vm.profissionais = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalHP');
                blocker.start();

                //seta o código d paciente
                vm.historia.IdPaciente = idpaciente;
                vm.historia.IdFuncionario = vm.profissionalSelecionado;

                prontuarioservice
                    .salvarHistoriaPregressa(vm.historia)
                    .then(function (result) {
                        vm.historia = result.data;
                        notification.showSuccess("História Pregressa cadastrada com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento.";
            }
        }
    }
})();