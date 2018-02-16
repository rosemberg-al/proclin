(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('Bancos', Bancos);

    Bancos.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'financeiroservice', '$stateParams'];
    function Bancos($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, financeiroservice, $stateParams) {
        var vm = this;

        common.setBreadcrumb('Financeiro .Banco');

        vm.init = init;
        vm.addBanco = addBanco;
        vm.excluirBanco = excluirBanco;

        init();

        function init() {

            vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[0, 'asc']]);

            var blocker = blockUI.instances.get('blockListaBancos');
            blocker.start();

            financeiroservice
                .listarbancos()
                .then(function (result) {
                    vm.bancos = result.data;
                })
                .catch(function (ex) {
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function excluirBanco(id) {

            vm.askOptions = { Title: 'Excluir', Text: 'Tem certeza que deseja excluir o registro selecionado?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    financeiroservice
                       .excluirbanco(id)
                       .then(function (result) {
                           notification.showSuccessBar("Banco excluido com sucesso!");
                           init();
                       })
                       .catch(function (ex) {
                       })['finally'](function () {
                       });
                }
            });
        }

        function addBanco(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud.banco.html',
                controller: "BancoCrud as vm",
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

    }
})();