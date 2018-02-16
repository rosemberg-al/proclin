(function () {
    'use strict';

    angular
        .module('app.vacinas')
        .controller('VacinaCrud', VacinaCrud);

    VacinaCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.prontuario', 'id'];

    function VacinaCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsProntuario, id) {

        var vm = this;
        vm.State = "Incluir Vacina";
        vm.FormMessage = "";
        vm.vacina = {};

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
                vm.State = "Editar Vacina";
                var blocker = blockUI.instances.get('blockModalCrudVacina');
                blocker.start();
                dsProntuario
                    .getVacinaById(id)
                    .then(function (result) {
                        vm.vacina = result.data;

                        if (result.data.Situacao == 'Ativo')
                            vm.vacina.Situacao = "A";
                        else
                            vm.vacina.Situacao = "I";
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.vacina.Situacao = "A";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.vacinascrud);

            if (vm.formValid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalCrudVacina');
                blocker.start();

                dsProntuario
                    .saveVacina(vm.vacina)
                    .then(function (result) {
                        vm.vacina = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Vacina cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Vacina alterada com sucesso!");

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