(function () {
    'use strict';

    angular
        .module('app.hospital')
        .controller('ListaMedicos', ListaMedicos);

    ListaMedicos.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'funcionarioservice', '$stateParams'];

    function ListaMedicos($scope, $http, $q, blockUI, common, notification, $modal, funcionarioservice, $stateParams) {

        common.setBreadcrumb('Cadastro .Médico');
        var vm = this;

        //Funções
        vm.init = init;
        vm.addMedico = addMedico;
        vm.buscar = buscar;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalListaMedicos');
            blocker.start();

            funcionarioservice
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

        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaMedicos');
                blocker.start();
                funcionarioservice
                   .listarMedicosPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.medicos = result.data;
                   })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
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