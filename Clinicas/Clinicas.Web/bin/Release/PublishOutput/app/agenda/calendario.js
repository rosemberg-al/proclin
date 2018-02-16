(function () {
    'use strict';

    angular
        .module('app.calendario')
        .controller('Calendario', Calendario)

    Calendario.$inject = ['$scope', '$http', '$q', 'notification', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', '$modal', 'exception', 'ds.agenda', '$stateParams'];

    function Calendario($scope, $http, $q, notification, DTInstances, DTOptionsBuilder, blockUI, common, $modal, exception, dsAgenda, $stateParams) {

        common.setBreadcrumb('atendimento .agendar');
        var vm = this;

        //Funções
        vm.init = init;

        //Feature Start
        init();

        //Implementations
        function init() {

           
        }

    }
})();