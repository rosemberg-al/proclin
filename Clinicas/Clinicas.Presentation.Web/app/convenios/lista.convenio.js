(function () {
    'use strict';

    angular
        .module('app.convenio')
        .controller('ListaConvenios', ListaConvenios);

    ListaConvenios.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros'];

    function ListaConvenios($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsCadastros) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .cadastro .convênios');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addConvenio = addConvenio;
        vm.tabelaPreco = tabelaPreco;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaConvenio');
            blocker.start();

            dsCadastros
                .getAllConvenios()
                .then(function (result) {
                    vm.convenios = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function tabelaPreco(item) {
            
            var modalInstance = $modal.open({
                templateUrl: 'app/convenios/tabela.preco.convenio.html',
                controller: 'TabelaConvenio as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    convenio: function () {
                        return item;
                    },
                }
            });

            modalInstance.result.then(function () {
            });
        }

        function addConvenio(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/convenios/crud.convenio.html',
                controller: 'ConvenioCrud as vm',
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

        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaConvenio');
                blocker.start();
                dsCadastros
                   .getConveniosPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.convenios = result.data;
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