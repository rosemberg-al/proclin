 (function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('PlanoContas', PlanoContas);

    PlanoContas.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification','financeiroservice', '$stateParams'];
    function PlanoContas($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, financeiroservice, $stateParams) {
        var vm = this;
        common.setBreadcrumb('Financeiro .Plano de Conta');

        vm.init = init;
        vm.addPlanoConta = addPlanoConta;
        vm.excluirPlanoConta = excluirPlanoConta;
        vm.buscar = buscar;
        vm.pesq = {};

        init();

        function init() {

            vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[1, 'asc']]);

            var blocker = blockUI.instances.get('blockListaPlanoContas');
            blocker.start();

            financeiroservice
                .listarPlanodeContas()
                .then(function (result) {
                    vm.planoscontas = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addPlanoConta(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud_planocontas.html',
                controller: "CrudPlanoContas as vm",    
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

                if (vm.pesq.Tipo == undefined) {
                    vm.pesq.Tipo = "";
                }
                financeiroservice
                    .pesquisarPlanoContas(vm.pesq.Nome, vm.pesq.Codigo, vm.pesq.Tipo)
                    .then(function (result) {
                        vm.planoscontas = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }


        function excluirPlanoConta(id) {
            vm.askOptions = { Title: 'Excluir Plano de Conta', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    financeiroservice.excluirPlanoConta(id).then(function (result) {
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

    }
})();