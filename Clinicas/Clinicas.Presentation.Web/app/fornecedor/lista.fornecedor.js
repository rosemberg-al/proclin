(function () {
    'use strict';

    angular
        .module('app.fornecedor')
        .controller('ListarFornecedor', ListarFornecedor);

    ListarFornecedor.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros'];

    function ListarFornecedor($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsCadastros) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('pagina-inicial .cadastro .fornecedores');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addFornecedor = addFornecedor;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'asc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListaFor');
            blocker.start();

            dsCadastros
                .listarFornecedores()
                .then(function (result) {
                    vm.fornecedores = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addFornecedor(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/fornecedor/crud.fornecedor.html',
                controller: 'FornecedorCrud as vm',
                size: 'xl',
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

        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaFor');
                blocker.start();
                dsCadastros
                   .listarFornecedoresPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.fornecedores = result.data;
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