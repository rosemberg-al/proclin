(function () {
    'use strict';

    angular
        .module('app.tabelapreco')
        .controller('TabelaController', TabelaController);

    TabelaController.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function TabelaController($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Sistema .Tabela de Preço');

        vm.tipoBusca = 'Nome';

        //Funções
        vm.init = init;
        vm.addTabela = addTabela;
        vm.addProcedimentos = addProcedimentos;
        vm.excluir = excluir;


        //Feature Start
        init();

        //Implementations
        function init() {
            vm.pesq = {};
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListarTabelas');
            blocker.start();

            cadastroservice
                .listarTabelas()
                .then(function (result) {
                    vm.tabelas = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }


        function excluir(id) {
            vm.askOptions = { Title: 'Excluir Tabela de Preço', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListarTabelas');
                    blocker.start();
                    cadastroservice.excluirTabela(id).then(function (result) {
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

        function addProcedimentos(tabela) {
            var modalInstance = $modal.open({
                templateUrl: 'app/tabelapreco/tabelapreco.procedimento.html',
                controller: 'TabProcController as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    tabela: function () {
                        return tabela;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function addTabela(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/tabelapreco/crud.tabelapreco.html',
                controller: 'TabCrudController as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }
    }
})();