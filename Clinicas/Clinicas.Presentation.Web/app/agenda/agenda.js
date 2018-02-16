(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('Agenda', Agenda)

    Agenda.$inject = ['$scope', '$http', '$q','notification', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', '$modal', 'exception', 'ds.agenda', '$stateParams'];

    function Agenda($scope, $http, $q,notification, DTInstances, DTOptionsBuilder, blockUI, common, $modal, exception, dsAgenda, $stateParams) {

        common.setBreadcrumb('atendimento .agenda');
        var vm = this;

        //Funções
        vm.init = init;
        vm.visualizar = visualizar;
        vm.pesquisar = pesquisar;
        vm.liberaragenda = liberaragenda;

        vm.busca = {};
        vm.busca.Tipo = 'NmPaciente';
        //Feature Start
        init();



        vm.dtOptions = DTOptionsBuilder
                       .newOptions()
                       .withOption('order', [[0, 'desc']]);

        //Implementations
        function init() {

            // if ($stateParams.id > 0) {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            dsAgenda
                .listar("T", 0)
                .then(function (result) {
                    vm.agenda = result.data;
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blocker.stop();
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


        function liberaragenda() {
            var modalInstance = $modal.open({
                templateUrl: 'app/agenda/liberaragenda.html',
                controller: 'LiberarAgenda as vm',
                size: 'xl',
                backdrop: 'static'
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function pesquisar() {

            if (vm.busca.Descricao == "") {
                init();
            } else {

                var blocker = blockUI.instances.get('block');
                blocker.start();
                dsAgenda
                    .pesquisar(vm.busca)
                    .then(function (result) {
                        vm.agenda = result.data;
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }
    }
})();