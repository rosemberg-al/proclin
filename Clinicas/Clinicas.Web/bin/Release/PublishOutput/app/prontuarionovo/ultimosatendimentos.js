(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('UltimosAtendimentos', UltimosAtendimentos);

    UltimosAtendimentos.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice', 'prontuarioservice', '$stateParams'];

    function UltimosAtendimentos($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice, prontuarioservice, $stateParams) {

        var vm = this;
        vm.init = init;
        vm.visualizar = visualizar;
        vm.alterarfoto = alterarfoto;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        vm.IdPaciente = $stateParams.id;

        init();

        vm.dtOptions = DTOptionsBuilder
            .newOptions()
            .withOption('order', [[0, 'desc']]);


        function init() {

            var blocker = blockUI.instances.get('blockProntuario');
            blocker.start();

            prontuarioservice
                .getUltimosAtendimentos($stateParams.id)
                .then(function (result) {
                    vm.atendimentos = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });


            pacienteservice
                .getPacienteById($stateParams.id)
                .then(function (result) {
                    vm.paciente = result.data;
                    console.log(result.data);
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function alterarfoto() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/alterar_foto.html',
                controller: 'AlterarFoto as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return vm.paciente.IdPaciente;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function visualizar(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/agenda/visualizaragenda.html',
                controller: 'VisualizarAgenda as vm',
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