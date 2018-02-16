(function () {
    'use strict';

    angular
        .module('app.medicamento')
        .controller('ListarMedicamento', ListarMedicamento);

    ListarMedicamento.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'medicamentoservice'];

    function ListarMedicamento($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, medicamentoservice) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Pesquisa .Medicamentos');

        //Funções
        vm.init = init;
        vm.detalhar = detalhar;
        
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListaFor');
            blocker.start();

            medicamentoservice
                .listarMedicamentos()
                .then(function (result) {
                    vm.medicamentos = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function detalhar(item) {
            var modalInstance = $modal.open({
                templateUrl: 'app/medicamento/detalhar_medicamento.html',
                controller: 'DetalharMedicamento as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    item: function () {
                        return item;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }
    }
})();