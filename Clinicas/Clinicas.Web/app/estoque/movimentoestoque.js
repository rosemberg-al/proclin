(function () {
    'use strict';

    angular
        .module('app.estoque')
        .controller('MovimentoEstoque', MovimentoEstoque);

    MovimentoEstoque.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'DTInstances', 'DTOptionsBuilder', 'common', 'notification', 'cadastroservice','estoqueservice'];

    function MovimentoEstoque($scope, $http, $q, $modal, blockUI, DTInstances, DTOptionsBuilder, common, notification, cadastroservice,estoqueservice) {

        var vm = this;
        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('Estoque .Movimento de Estoque');

        //Funções
        vm.init = init;
        vm.addMovimentoEstoque = addMovimentoEstoque;
        vm.excluirMovimentoEstoque = excluirMovimentoEstoque;


        init();

         vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

        function init() {
          
            var blocker = blockUI.instances.get('block');
            blocker.start();

              estoqueservice
                .listarMovimentoEstoque()
                .then(function (result) {
                    vm.movimentos = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function addMovimentoEstoque(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/estoque/crud.movimentoestoque.html',
                controller: 'CrudMovimentoEstoque as vm',
                size: 'lg',
                backdrop: 'static',
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

        function excluirMovimentoEstoque(id) {
            vm.askOptions = { Title: 'Excluir ', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block');
                    blocker.start();
                    estoqueservice.excluirMovimentoEstoque(id).then(function (result) {
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