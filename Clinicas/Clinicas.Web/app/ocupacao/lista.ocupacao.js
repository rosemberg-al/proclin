(function () {
    'use strict';

    angular
        .module('app.ocupacao')
        .controller('ListaOcupacoes', ListaOcupacoes);

    ListaOcupacoes.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function ListaOcupacoes($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;
        common.setBreadcrumb('Pesquisa .Ocupação');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addOcupacao = addOcupacao;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaOcupacao');
            blocker.start();

            cadastroservice
                .getAllOcupcaoes()
                .then(function (result) {
                    vm.ocupacoes = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addOcupacao(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/ocupacao/crud.ocupacao.html',
                controller: 'OcupacaoCrud as vm',
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
                var blocker = blockUI.instances.get('blockModalListaOcupacao');
                blocker.start();
                cadastroservice
                   .getOcupacoesPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.ocupacoes = result.data;
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