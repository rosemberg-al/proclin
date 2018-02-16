(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('Conta', Conta);

    Conta.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'financeiroservice', '$stateParams'];
    function Conta($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, financeiroservice, $stateParams) {

        var vm = this;
        vm.pesq = {};


        common.setBreadcrumb('Financeiro .Conta');

        vm.init = init;
        vm.addConta = addConta;
        vm.buscar = buscar;
        vm.excluir = excluir;

        init();

        function init() {

            vm.pesq = {};

            vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListaConta');
            blocker.start();

            financeiroservice
                .listarContas()
                .then(function (result) {
                    vm.contas = result.data;
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addConta(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud.conta.html',
                controller: "ContaCrud as vm",
                backdrop: 'static',
                size: 'lg',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function excluir(id) {
            vm.askOptions = { Title: 'Excluir Conta', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    financeiroservice.excluirConta(id).then(function (result) {
                        notification.showSuccessBar("Exclusão realizada com sucesso");
                        init();
                    })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                    blocker.stop();
                }
            });
        }

        function buscar() {
            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaFor');
                blocker.start();

                if (vm.pesq.Nome == undefined) {
                    vm.pesq.Nome = "";
                }
                financeiroservice
                    .pesquisarContas(vm.pesq.Nome, vm.pesq.Codigo)
                    .then(function (result) {
                        vm.contas = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

    }
})();