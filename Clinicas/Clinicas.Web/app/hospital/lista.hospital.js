(function () {
    'use strict';

    angular
        .module('app.hospital')
        .controller('ListaHospital', ListaHospital);

    ListaHospital.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'prontuarioservice', '$stateParams'];

    function ListaHospital($scope, $http, $q, blockUI, common, notification, $modal, prontuarioservice, $stateParams) {

        var vm = this;

        common.setBreadcrumb('Cadastro .Hospitais');

        //Funções
        vm.init = init;
        vm.addHospital = addHospital;
        vm.buscar = buscar;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalListaHospitais');
            blocker.start();

            prontuarioservice
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

        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaHospitais');
                blocker.start();
                prontuarioservice
                   .listarHospitaisPorNome(vm.nome)
                   .then(function (result) {
                       vm.hospitais = result.data;
                       console.log(vm.hospitais);
                   })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
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