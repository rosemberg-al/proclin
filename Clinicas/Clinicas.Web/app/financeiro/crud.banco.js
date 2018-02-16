(function () {
    'use strict';

    angular
        .module('app.financeiro')
        .controller('BancoCrud', BancoCrud);

    BancoCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'financeiroservice', 'id'];

    function BancoCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, financeiroservice, id) {

        var vm = this;
        vm.State = "Incluir Banco";
        vm.FormMessage = "";

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
                vm.State = "Editar Banco";
                var blocker = blockUI.instances.get('blockModalBancos');
                blocker.start();
                financeiroservice
                    .getBancoById(id)
                    .then(function (result) {
                        vm.banco = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.bancos.$valid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalBancos');
                blocker.start();

                financeiroservice
                    .salvarBanco(vm.banco)
                    .then(function (result) {
                        vm.banco = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.Message);
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