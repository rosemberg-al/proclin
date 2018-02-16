(function () {
    'use strict';

    angular
        .module('app.hospital')
        .controller('ListaMedicos', ListaMedicos);

    ListaMedicos.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'exception', 'ds.funcionario', '$stateParams'];

    function ListaMedicos($scope, $http, $q, blockUI, common, notification, $modal, exception, dsFunionario, $stateParams) {

        common.setBreadcrumb('Médicos');
        var vm = this;

        //Funções
        vm.init = init;
        vm.addMedico = addMedico;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalListaMedicos');
            blocker.start();

            dsFunionario
                .listarMedicos()
                .then(function (result) {
                    vm.medicos = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addMedico(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/hospital/crud.medico.html',
                controller: 'MedicoCrud as vm',
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