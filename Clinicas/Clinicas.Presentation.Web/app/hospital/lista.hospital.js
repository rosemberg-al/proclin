(function () {
    'use strict';

    angular
        .module('app.hospital')
        .controller('ListaHospital', ListaHospital);

    ListaHospital.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'exception', 'ds.prontuario', '$stateParams'];

    function ListaHospital($scope, $http, $q, blockUI, common, notification, $modal, exception, dsProntuario, $stateParams) {

        common.setBreadcrumb('Hospitais');
        var vm = this;

        common.setBreadcrumb('pagina-inicial .cadastro .hospitais');

        //Funções
        vm.init = init;
        vm.addHospital = addHospital;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalListaHospitais');
            blocker.start();

            dsProntuario
                .getHospitais()
                .then(function (result) {
                    vm.hospitais = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addHospital(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/hospital/crud.hospital.html',
                controller: 'HospitalCrud as vm',
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