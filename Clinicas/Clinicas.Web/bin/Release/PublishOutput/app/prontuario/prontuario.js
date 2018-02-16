(function () {
    'use strict';

    angular
        .module('app.prontuario')
        .controller('Prontuario', Prontuario)

    Prontuario.$inject = ['$scope', '$http', '$q', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', '$modal', 'exception', 'ds.paciente', '$stateParams'];

    function Prontuario($scope, $http, $q, DTInstances, DTOptionsBuilder, blockUI, common, notification, $modal, exception, dsPaciente, $stateParams) {

        common.setBreadcrumb('atendimento .prontuário eletrônico');
        var vm = this;

        //Funções
        vm.init = init;
        vm.pesquisar = pesquisar;

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
            dsPaciente
                .listar()
                .then(function (result) {
                    vm.dados = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function pesquisar() {
            if (vm.busca.Descricao == "") {
                init();
            } else {

                var blocker = blockUI.instances.get('block');
                blocker.start();
                dsPaciente
                    .pesquisar(vm.busca)
                    .then(function (result) {
                        vm.dados = result.data;
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