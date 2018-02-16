(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('TvEspera', TvEspera)

    TvEspera.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'common','$timeout', 'notification', 'DTInstances', 'DTOptionsBuilder', 'agendaservice','cadastroservice','$interval', 'dateFilter', '$stateParams'];

    function TvEspera($scope, $http, $q, $modal, blockUI, common,$timeout, notification, DTInstances, DTOptionsBuilder, agendaservice,cadastroservice,$interval, dateFilter, $stateParams) {

        common.setBreadcrumb('Atendimento .Sala de Espera');
        var vm = this;

        //Funções
        vm.init = init;
        vm.ultimaschamadas = {};
        vm.ultimo = {};
        vm.play = play;
        vm.nome = "";

       init();

        var timer;
        //Implementations
        function init() {

            var blocker = blockUI.instances.get('block');
            blocker.start();
             
            agendaservice
                .listarPacientesConvocados()
                .then(function (result) {
                     vm.ultimaschamadas = result.data;
                     vm.ultimo = vm.ultimaschamadas[0];
                     
                    if(vm.ultimaschamadas.length>0){
                        if(vm.nome!=vm.ultimo.NmPaciente){
                            vm.play(); 
                        }
                    }

                    if(vm.ultimo!=null){
                        vm.nome = vm.ultimo.NmPaciente;
                    }
                    
                })
                .catch(function (ex) {
                    notification.showError(ex);
                })['finally'](function () {
                    blocker.stop();
                });
               timer = $timeout(function() { init(); }, 10000); // 10 segundos

                  
        }

        function play(){
           $('.player_audio').trigger("play");
        }

    }
})();