(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('PlanoAgenda', PlanoAgenda);

    PlanoAgenda.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'agendaservice', 'cadastroservice', '$stateParams'];

    function PlanoAgenda($scope, $http, $q, $modal, blockUI, common, notification, DTInstances, DTOptionsBuilder, agendaservice,cadastroservice, $stateParams) {


        var vm = this;

        common.setBreadcrumb('Atendimento .Plano de Agenda');

        //Funções
        vm.init = init;
        vm.crud = crud;
       

        vm.busca = {};
        vm.busca.TipoBusca = 'Codigo';

        init();

        vm.dtOptions = DTOptionsBuilder
                     .newOptions()
                     .withOption('order', [[0, 'desc']]);


        //Implementations
        function init() {

            var blocker = blockUI.instances.get('block');
            blocker.start();
            cadastroservice
                .listarProfissionaisSaude()
                .then(function (result) {
                    vm.dados = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex)
                })['finally'](function () {
                    blocker.stop();
                });


            var blocker = blockUI.instances.get('block');
            blocker.start();
            agendaservice
                .listarAgendaAguardando(0)
                .then(function (result) {
                    vm.agenda = result.data;
                })
                .catch(function (ex) {

                })['finally'](function () {
                    blocker.stop();
                });
        }

        

        function crud() {
            var modalInstance = $modal.open({
                templateUrl: 'app/agenda/liberaragenda.html',
                controller: 'LiberarAgenda as vm',
                size: 'lg',
                backdrop: 'static',
            });
            modalInstance.result.then(function () {
                init();
            });
        }
    }
})();