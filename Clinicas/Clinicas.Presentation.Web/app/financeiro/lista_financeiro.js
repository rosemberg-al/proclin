(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('FinanceiroController', FinanceiroController);

    FinanceiroController.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.financeiro', '$stateParams'];
    function FinanceiroController($scope, $http, $q, $modal, $modalInstance, DTOptionsBuilder, blockUI, common, notification, exception, dsFuncionario, $stateParams) {
        var vm = this;
        vm.titulo = "Contas a pagar";
        vm.busca = {
            Descricao: "",
            TipoBusca: "Codigo",
        };

        
        vm.init = init;
        vm.listar = listar;
        vm.pesquisar = pesquisar;
        vm.excluir = excluir;
        vm.crud = crud;

        init();

        function init() {
            vm.redirecionamento = $stateParams.tipo;
            if ($stateParams.tipo == "P")
                vm.titulo = "Contas a pagar";
            else
                vm.titulo = "Contas a receber";

            listar();
        }

        function pesquisar() {
            if (vm.busca.Descricao != "") {


                dsFinanceiro
                    .pesquisarparcela(vm.busca.TipoBusca, vm.busca.Descricao, $stateParams.tipo)
                    .then(function (result) {
                        vm.financeiros = result.data;
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                    });

            }
            else {
                notification.showErrorTop("Para realizar a busca preencha o campo descrição.");
            }
        }

        function listar() {
           
            if ($state.params.tipo == 1) {

                dsFinanceiro
                   .getContasApagarReceber('P')
                   .then(function (result) {
                       vm.financeiros = result.data;
                   })
                   .catch(function (ex) {
                       exception.throwEx(ex);
                   })['finally'](function () {
                   });
            }
            else{
                dsFinanceiro
                  .getContasApagarReceber('R')
                  .then(function (result) {
                      vm.financeiros = result.data;
                  })
                  .catch(function (ex) {
                      exception.throwEx(ex);
                  })['finally'](function () {
                  });
            }
        }

        function crud(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud_financeiro.html',
                controller: "CrudFinanceiro as vm",
                //windowClass: "animated flipInY",
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

        function excluir(idFinanceiro, idParcela) {

            vm.askOptions = { Title: 'Excluir Parcela', Text: 'Tem certeza que deseja excluir o registro selecionado?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    dsFinanceiro
                        .excluirParcela(idFinanceiro, idParcela)
                        .then(function (result) {
                            notification.showSuccessBar("Exclusão realizada com sucesso!");
                            init();
                        })
                        .catch(function (ex) {
                            exception.throwEx(ex);
                        })['finally'](function () {
                        });
                } 
            });
           
        }

    }
})();