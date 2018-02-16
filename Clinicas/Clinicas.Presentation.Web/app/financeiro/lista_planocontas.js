(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('PlanoContas', PlanoContas);

    PlanoContas.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.financeiro', '$stateParams'];
    function PlanoContas($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsFinanceiro, $stateParams) {
        var vm = this;
        vm.titulo = "Plano de Contas";

        vm.init = init;
        vm.addPlanoConta = addPlanoConta;

        init();

        function init() {

            vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListaPlanoContas');
            blocker.start();

            dsFinanceiro
                .listarplanodecontas()
                .then(function (result) {
                    vm.planoscontas = result.data;
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addPlanoConta(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud.planocontas.html',
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

    }
})();