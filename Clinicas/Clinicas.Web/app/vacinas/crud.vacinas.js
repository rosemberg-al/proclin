(function () {
    'use strict';

    angular
        .module('app.vacinas')
        .controller('VacinaCrud', VacinaCrud);

    VacinaCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'prontuarioservice', 'id'];

    function VacinaCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, prontuarioservice, id) {

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
                prontuarioservice
                    .getVacinaById(id)
                    .then(function (result) {
                        vm.vacina = result.data;

                        if (result.data.Situacao == 'Ativo')
                            vm.SituacaoA = "A";
                        else
                            vm.SituacaoI = "I";
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.SituacaoA = "A";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }


        function save() {
           
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.vacinascrud.$valid) {
                vm.FormMessage = "";

                if (vm.SituacaoA == "A")
                    vm.vacina.Situacao = "A";
                else
                    vm.vacina.Situacao = "I";


                var blocker = blockUI.instances.get('blockModalCrudVacina');
                blocker.start();

                prontuarioservice
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