(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('SalaEspera', SalaEspera)

    SalaEspera.$inject = ['$scope', '$http', '$q', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', '$modal', 'exception', 'ds.agenda', '$stateParams'];

    function SalaEspera($scope, $http, $q, DTInstances, DTOptionsBuilder, blockUI, common, $modal, exception, dsAgenda, $stateParams) {

        common.setBreadcrumb('atendimento .sala de espera');
        var vm = this;

        //Funções
        vm.init = init;
        vm.visualizar = visualizar;
        vm.confirmaratendimento = confirmaratendimento;
        vm.cancelar = cancelar;

        //Feature Start
        init();

        vm.dtOptions = DTOptionsBuilder
                       .newOptions()
                       .withOption('order', [[0, 'asc']]);

        //Implementations
        function init() {

            // if ($stateParams.id > 0) {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            dsAgenda
                .salaespera()
                .then(function (result) {
                    vm.dados = result.data;
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

        function confirmaratendimento() {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            agendaservice
                .realizado(vm.agenda.IdAgenda)
                .then(function (result) {
                    notification.showSuccessBar("Atendimento realizado com sucesso");
                    $modalInstance.close();
                })
                .catch(function (ex) {
                    notification.showError(ex.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function cancelar() {

            vm.askOptions = { Title: 'Confirmar', Text: 'Tem certeza que deseja cancelar a agenda selecionada ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block');
                    blocker.start();

                    agendaservice
                        .cancelado(vm.agenda.IdAgenda)
                        .then(function (result) {
                            notification.showSuccessBar("Cancelamento realizado com sucesso");
                            $modalInstance.close();
                        })
                        .catch(function (ex) {
                            notification.showError(ex);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
            });

        }
    }
})();