(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('FinanceiroController', FinanceiroController);

    FinanceiroController.$inject = ['$scope', '$state', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'financeiroservice', '$stateParams'];
    function FinanceiroController($scope, $state, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, financeiroservice, $stateParams) {
        var vm = this;
        common.setBreadcrumb('Financeiro .Contas a pagar');

        vm.titulo = "";
        vm.busca = {
            Descricao: "",
            TipoBusca: "Nome",
        };

        vm.init = init;
        vm.listar = listar;
        vm.pesquisar = pesquisar;
        vm.excluir = excluir;
        vm.crud = crud;
        vm.visualizar = visualizar;
        vm.editarparcela = editarparcela;

        vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[0, 'desc']]);

        init();

        function init() {
            vm.redirecionamento = $stateParams.tipo;
            if ($stateParams.tipo == "P"){
                common.setBreadcrumb('Financeiro .Contas a pagar');
                vm.titulo = "Contas a Pagar";
            }else{
                common.setBreadcrumb('Financeiro .Contas a receber');
                vm.titulo = "Contas a Receber";
            }
            listar();
        }

        function pesquisar() {
            if (vm.busca.Descricao != "") {
                financeiroservice
                    .pesquisar(vm.busca.TipoBusca, vm.busca.Descricao, $stateParams.tipo)
                    .then(function (result) {
                        vm.financeiros = result.data;
                    })
                    .catch(function (ex) {
                    })['finally'](function () {
                    });
            }
            else {
                notification.showErrorTop("Para realizar a busca preencha o campo descrição.");
            }
        }

        function listar() {
            if ($state.params.tipo == "P") {
                financeiroservice
                   .getContasApagarReceber('P')
                   .then(function (result) {
                       vm.financeiros = result.data;
                   })
                   .catch(function (ex) {
                   })['finally'](function () {
                   });
            }
            else {
                financeiroservice
                  .getContasApagarReceber('R')
                  .then(function (result) {
                      vm.financeiros = result.data;
                  })
                  .catch(function (ex) {
                  })['finally'](function () {
                  });
            }
        }

        function crud(tipo) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud_financeiro.html',
                controller: "CrudFinanceiro as vm",
                //windowClass: "animated flipInY",
                backdrop: 'static',
                size: 'lg',
                resolve: {
                    tipo: function () {
                        return tipo;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function editarparcela(idparcela,idfinanceiro) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/edit_parcela.html',
                controller: "EditarParcela as vm",
                //windowClass: "animated flipInY",
                backdrop: 'static',
                size: 'lg',
                resolve: {
                    tipo:function(){
                        return $state.params.tipo;
                    },
                    idparcela: function () {
                        return idparcela;
                    },
                    idfinanceiro: function () {
                        return idfinanceiro;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }


        function excluir(idFinanceiro, idParcela) {
            vm.askOptions = { Title: 'Excluir Parcela', Text: 'Tem certeza que deseja excluir o registro selecionado?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    financeiroservice
                        .excluirParcela(idFinanceiro, idParcela)
                        .then(function (result) {
                            notification.showSuccessBar("Exclusão realizada com sucesso!");
                            init();
                        })
                        .catch(function (ex) {
                        })['finally'](function () {
                        });
                }
            });
        }

         function visualizar(idfinanceiro,idparcela) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/visualizar_financeiro.html',
                controller: 'VisualizarFinanceiro as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    idfinanceiro: function () {
                        return idfinanceiro;
                    },
                    idparcela: function () {
                        return idparcela;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }
    }
})();