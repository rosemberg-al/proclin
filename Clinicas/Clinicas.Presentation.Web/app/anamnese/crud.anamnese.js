(function () {
    'use strict';

    angular
        .module('app.anamnese')
        .controller('AnamneseCrud', AnamneseCrud);

    AnamneseCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'DTInstances', 'ds.anamnese', 'paciente', 'id'];

    function AnamneseCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, DTInstances, dsAnamnese, paciente, id) {

        common.setBreadcrumb('Anamnese');
        var vm = this;
        vm.State = "Incluir Anamnese";
        vm.FormMessage = "";
        vm.anamnese = {
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
            console.log(paciente);
            vm.FormMessage = "";
            vm.anamnese.Paciente = paciente.NmPaciente;
            if (id > 0) {
                vm.State = "Editar Anamnese";
                var blocker = blockUI.instances.get('blockModalAnamnese');
                blocker.start();

                dsAnamnese
                    .getById(id)
                    .then(function (result) {
                        vm.anamnese = result.data;
                        vm.anamnese.Paciente = paciente.NmPaciente;

                        if (result.data.Situacao == 'Ativo')
                            vm.anamnese.Situacao = "A";
                        else
                            vm.anamnese.Situacao = "I";
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.anamnese.Situacao = "A";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.anamnese);

            if (vm.formValid) {
                vm.FormMessage = "";
                console.log(vm.anamnese);
                var blocker = blockUI.instances.get('blockModalAnamnese');
                blocker.start();

                //seta o código d paciente
                vm.anamnese.IdPaciente = paciente.IdPaciente;

                dsAnamnese
                    .save(vm.anamnese)
                    .then(function (result) {
                        vm.anamnese = result.data;
                        if(id == 0)
                            notification.showSuccessBar("Anamnese cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Anamnese alterada com sucesso!");

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