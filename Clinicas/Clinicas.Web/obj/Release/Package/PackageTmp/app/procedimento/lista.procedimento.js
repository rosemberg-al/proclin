(function () {
    'use strict';

    angular
        .module('app.procedimento')
        .controller('ListaProcedimentos', ListaProcedimentos);

    ListaProcedimentos.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function ListaProcedimentos($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;
        vm.TipoBusca = 'Descricao';

        common.setBreadcrumb('Pesquisa .Procedimento');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addProcedimento = addProcedimento;
        vm.visualizar = visualizar;


        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaProc');
            blocker.start();

            cadastroservice
                .getAllProcedimentos()
                .then(function (result) {
                    vm.procedimentos = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function visualizar(procedimento) {
            var modalInstance = $modal.open({
                templateUrl: 'app/procedimento/procedimentoespecialidade.html',
                controller: 'ProcedimentoEspecialidade as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    procedimento: function () {
                        return procedimento;
                    }
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }

        function addProcedimento(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/procedimento/crud.procedimento.html',
                controller: 'ProcedimentoCrud as vm',
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
                init();
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaProc');
                blocker.start();
                cadastroservice
                   .getProcedimentosPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.procedimentos = result.data;
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