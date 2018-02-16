(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('CrudTransferencias', CrudTransferencias);

    CrudTransferencias.$inject = ['$scope', '$state', '$http', '$q', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'financeiroservice', 'id'];
    function CrudTransferencias($scope, $state, $http, $q, $modal, $modalInstance, blockUI, common, notification, financeiroservice, id) {
        var vm = this;
        vm.titulo = "Incluir transferência entre contas";
        vm.FormMessage = "";
        vm.transferencia = {};
        vm.contas = [];
        $scope.forms = {};
        vm.formValid = true;

        vm.init = init;
        vm.cancel = cancel;
        vm.save = save;

        init();

        function init() {

            var pContas = financeiroservice.listarContas();
            pContas.then(function (result) {
                vm.contaOrigem = result.data;
                vm.contaDestino = result.data;
            });

            var blocker = blockUI.instances.get('blockModalTransf');
            blocker.start();


            $q.all([pContas]).then(function () {
                if (id > 0) {
                    vm.titulo = "Alterar transferência entre contas";

                    financeiroservice
                        .getTransferenciaById(id)
                        .then(function (response) {
                            vm.transferencia = response.data;

                            var origem = _.find(vm.contaOrigem, { IdConta: response.IdContaOrigem });
                            var destino = _.find(vm.contaDestino, { IdConta: response.IdContaDestino });

                            if (origem != null)
                                vm.contaOrigemSelecionada = origem.IdConta;

                            if (destino != null)
                                vm.contaDestinoSelecionada = destino.IdConta;

                        })
                        .catch(function (ex) {
                            exception.throwEx(ex);
                        })['finally'](function () {
                        });

                } else {
                    vm.titulo = "Incluir transferência entre contas";
                }

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
            });


        }

        function cancel() {
            $modalInstance.dismiss();
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.transferencias.$valid) {

                if (vm.contaDestinoSelecionada != undefined)
                    vm.transferencia.IdContaDestino = vm.contaDestinoSelecionada;

                if (vm.contaOrigemSelecionada != undefined)
                    vm.transferencia.IdContaOrigem = vm.contaOrigemSelecionada;

                var blocker = blockUI.instances.get('blockModalPlanoContas');
                blocker.start();

                financeiroservice
                    .saveTransferencia(vm.transferencia)
                    .then(function (result) {
                        vm.financeiro = result.data;
                        if (id == 0) {
                            notification.showSuccessBar("Transferência entre contas cadastrada com sucesso!");
                        }
                        else {
                            notification.showSuccessBar("Transferência entre contas alterada com sucesso!");
                        }
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });

            } else {
                vm.FormMessage = 'Existem campos obrigatórios sem o devido preenchimento';
                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }

        }

    }
})();