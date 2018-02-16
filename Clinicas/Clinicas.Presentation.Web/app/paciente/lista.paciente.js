(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('ListaPacientes', ListaPacientes);

    ListaPacientes.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.paciente'];

    function ListaPacientes($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsPaciente) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .cadastro .pacientes');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addPaciente = addPaciente;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaPacientes');
            blocker.start();

            dsPaciente
                .listar()
                .then(function (result) {
                    vm.pacientes = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addPaciente(id) {
            var paciente = { IdPaciente: id };

            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/crud.paciente.html',
                controller: 'PacienteCrud as vm',
                size: 'xl',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
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
                var blocker = blockUI.instances.get('blockModalListaPacientes');
                blocker.start();
                dsPaciente
                   .getPacientesPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.pacientes = result.data;
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