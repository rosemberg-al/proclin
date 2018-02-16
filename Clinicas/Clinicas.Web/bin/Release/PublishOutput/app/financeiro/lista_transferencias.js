(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('Transferencias', Transferencias);

    Transferencias.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'financeiroservice', '$stateParams'];
    function Transferencias($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, financeiroservice, $stateParams) {
        var vm = this;

        common.setBreadcrumb('Financeiro .Transferências entre contas');

        vm.init = init;
        vm.abrir = abrir;
        vm.excluir = excluir;

        init();

        function init() {

            vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListaTransf');
            blocker.start();

            financeiroservice
                .getTransferencias()
                .then(function (result) {
                    vm.transferencias = result.data;
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function excluir(id) {

            vm.askOptions = { Title: 'Excluir', Text: 'Tem certeza que deseja excluir o registro selecionado?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    financeiroservice
                       .excluirTransferencia(id)
                       .then(function (result) {
                           notification.showSuccessBar("Transferência excluida com sucesso!");
                           init();
                       })
                       .catch(function (ex) {
                           exception.throwEx(ex);
                       })['finally'](function () {
                       });
                }
            });
        }

        function abrir(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud_transferencias.html',
                controller: "CrudTransferencias as vm",
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