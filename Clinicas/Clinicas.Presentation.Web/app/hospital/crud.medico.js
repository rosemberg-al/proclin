(function () {
    'use strict';

    angular
        .module('app.hospital')
        .controller('MedicoCrud', MedicoCrud);

    MedicoCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.funcionario', 'id'];

    function MedicoCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsFuncionario, id) {

        var vm = this;
        vm.State = "Incluir Médico";
        vm.FormMessage = "";
        vm.medico = {};

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
            vm.FormMessage = "";

            if (id > 0) {
                vm.State = "Editar Médico";
                var blocker = blockUI.instances.get('blockModalCrudMedico');
                blocker.start();
                dsFuncionario
                    .getMedicoById(id)
                    .then(function (result) {
                        vm.medico = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.medicos);

            if (vm.formValid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalCrudMedico');
                blocker.start();

                dsFuncionario
                    .saveMedico(vm.medico)
                    .then(function (result) {
                        vm.medico = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Médico cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Médico alterado com sucesso!");

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