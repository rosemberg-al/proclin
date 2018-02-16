(function () {
    'use strict';

    angular
        .module('app.procedimento')
        .controller('ListaProcedimentos', ListaProcedimentos);

    ListaProcedimentos.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros'];

    function ListaProcedimentos($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsCadastros) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .cadastro .procedimentos');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addProcedimento = addProcedimento;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaProc');
            blocker.start();

            dsCadastros
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
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaProc');
                blocker.start();
                dsCadastros
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