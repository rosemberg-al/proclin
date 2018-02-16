(function () {
    'use strict';

    angular
        .module('app.financeiro')
        .controller('ContaCrud', ContaCrud);

    ContaCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'financeiroservice', 'id'];

    function ContaCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, financeiroservice, id) {

        var vm = this;
        vm.State = "Incluir Conta";
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
                vm.State = "Editar Conta";
                var blocker = blockUI.instances.get('blockModalContas');
                blocker.start();
                financeiroservice
                    .getContaById(id)
                    .then(function (result) {
                        vm.conta = result.data;

                        if (vm.conta.Situacao == 'Ativo')
                            vm.SituacaoA = "Ativo";
                        else
                            vm.SituacaoI = "Inativo";
                    })
                    .catch(function (ex) {
                        notification.showError(ex.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            } else {
                vm.SituacaoA = "Ativo";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.contas.$valid) {
                

                if (vm.SituacaoA == 'Ativo')
                    vm.conta.Situacao = "Ativo";
                else
                    vm.conta.Situacao = "Inativo";

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalContas');
                blocker.start();

                financeiroservice
                    .saveConta(vm.conta)
                    .then(function (result) {
                        vm.conta = result.data;
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
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }

    }
})();