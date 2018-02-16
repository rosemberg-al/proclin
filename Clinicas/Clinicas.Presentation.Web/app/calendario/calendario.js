(function () {
    'use strict';

    angular
        .module('app.dashboard')
        .controller('Calendario', Calendario);

    Calendario.$inject = ['$scope', '$http', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', '$modal', 'exception', 'ds.agenda'];

    function Calendario($scope, $http, DTInstances, DTOptionsBuilder, blockUI, common, $modal, exception, dsAgenda) {

        common.setBreadcrumb('atendimento .calendário');
        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
       
        //Feature Start
        init();


        vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('block');
            blocker.start();
            dsAgenda
                .listarAgendaPorProfissional(10,'22/04/2017')
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
})();