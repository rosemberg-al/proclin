﻿(function () {
    'use strict';

    angular
        .module('app.prontuario')
        .controller('ModeloProntuario', ModeloProntuario);

    ModeloProntuario.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'exception', 'ds.prontuario', '$stateParams'];

    function ModeloProntuario($scope, $http, $q, blockUI, common, notification, $modal, exception, dsProntuario, $stateParams) {

        common.setBreadcrumb('pagina-inicial .cadastro .Modelos de Prontuários');
        var vm = this;

        //Funções
        vm.init = init;
        vm.addModelo = addModelo;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalListaModelo');
                blocker.start();

                dsProntuario
                    .getModelos()
                    .then(function (result) {
                        vm.modelos = result.data;
                        console.log(result.data);
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
        }

        function addModelo(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuario/crud.modelo.prontuario.html',
                controller: 'ModeloCrud as vm',
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