(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('CrudAnamnese', CrudAnamnese);

    CrudAnamnese.$inject = ['$scope', '$http', '$q',  '$modal','$modalInstance',  'DTOptionsBuilder', 'blockUI', 'common', 'notification','cadastroservice', 'pacienteservice','prontuarioservice','idpaciente'];

    function CrudAnamnese($scope, $http, $q,  $modal, $modalInstance,  DTOptionsBuilder, blockUI, common, notification,cadastroservice, pacienteservice,prontuarioservice,idpaciente) {

        var vm = this;
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        init();
        
        function init() {

             var blocker = blockUI.instances.get('blockModalAnamnese');
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
            if ($scope.forms.anamnese.$valid) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalAnamnese');
                blocker.start();

                //seta o código d paciente
                vm.anamnese.IdPaciente = idpaciente;
                vm.anamnese.IdProfissionalSaude = vm.profissionalSelecionado;

                prontuarioservice
                    .salvarAnamnese(vm.anamnese)
                    .then(function (result) {
                        vm.anamnese = result.data;
                        notification.showSuccess("Anamnese cadastrada com sucesso");

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