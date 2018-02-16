(function () {
    'use strict';

    angular
        .module('app.funcionario')
        .controller('ListarFuncionario', ListarFuncionario);

    ListarFuncionario.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.funcionario'];

    function ListarFuncionario($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsFuncionario) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .cadastro .funcionários');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addFuncionario = addFuncionario;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListaFunc');
            blocker.start();

            dsFuncionario
                .listarfuncionarios()
                .then(function (result) {
                    vm.funcionarios = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addFuncionario(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/funcionario/crud.funcionario.html',
                controller: 'FuncionarioCrud as vm',
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
                var blocker = blockUI.instances.get('blockModalListaProc');
                blocker.start();
                dsFuncionario
                   .listarfuncionariosPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.funcionarios = result.data;
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