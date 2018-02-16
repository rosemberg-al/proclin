(function () {
    'use strict';

    angular
        .module('app.bloqueio')
        .controller('AusenciaController', AusenciaController);

    AusenciaController.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'agendaservice'];

    function AusenciaController($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, agendaservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .agenda .bloqueio de agenda');

        //Funções
        vm.init = init;
        vm.abrir = abrir;
        vm.excluir = excluir;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                .newOptions()
                .withOption('order', [[1, 'desc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockListAusencia');
            blocker.start();

            agendaservice
                .listarBloqueioAgenda()
                .then(function (result) {
                    vm.bloqueios = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function excluir(id) {
            vm.askOptions = { Title: 'Cancelar', Text: 'Tem certeza que deseja excluir o bloqueio de agenda selecionado?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    agendaservice
                        .excluirBloqueioAgenda(id)
                        .then(function (result) {
                            notification.showSuccessBar("Bloqueio de agenda excluído com sucesso!");
                            init();
                        })
                        .catch(function (ex) {
                        })['finally'](function () {
                        });
                }
            });
        }

        function abrir(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/bloqueioagenda/crud.bloqueioagenda.html',
                controller: 'CrudAusenciaController as vm',
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