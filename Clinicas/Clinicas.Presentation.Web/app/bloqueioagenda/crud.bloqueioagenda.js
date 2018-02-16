(function () {
    'use strict';

    angular
        .module('app.bloqueio')
        .controller('CrudAusenciaController', CrudAusenciaController);

    CrudAusenciaController.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.funcionario'];

    function CrudAusenciaController($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsFuncionario) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.add = add;
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

        function add(id) {

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
    }
})();