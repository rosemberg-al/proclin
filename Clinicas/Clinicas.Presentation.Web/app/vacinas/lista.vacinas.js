﻿(function () {
    'use strict';

    angular
        .module('app.vacinas')
        .controller('ListarVacinas', ListarVacinas);

    ListarVacinas.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'exception', 'ds.prontuario'];

    function ListarVacinas($scope, $http, $q, blockUI, common, notification, $modal, exception, dsProntuario) {

        common.setBreadcrumb('pagina-inicial .cadastro .Vacinas');
        var vm = this;

        //Funções
        vm.init = init;
        vm.addVacina = addVacina;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalListaVacinas');
            blocker.start();

            dsProntuario
                .getVacinas()
                .then(function (result) {
                    vm.vacinas = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addVacina(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/vacinas/crud.vacinas.html',
                controller: 'VacinaCrud as vm',
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
    }
})();