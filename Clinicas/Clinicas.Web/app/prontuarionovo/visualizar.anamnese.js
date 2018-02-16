(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('VisualizarAnamnese', VisualizarAnamnese);

    VisualizarAnamnese.$inject = ['$scope', '$http', '$q', '$modal','$modalInstance', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','prontuarioservice','id','idpaciente'];

    function VisualizarAnamnese($scope, $http, $q, $modal,$modalInstance, DTOptionsBuilder, blockUI, common, notification, pacienteservice,prontuarioservice,id,idpaciente) {

        var vm = this;
        vm.init = init;
        vm.cancel = cancel;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
       
        init();

        
        function init() {

            var blocker = blockUI.instances.get('blockModalAnamnese');
            blocker.start();

             prontuarioservice
                .obterAnamnesePorId(id)
                .then(function (result) {
                    vm.anamnese = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });

            pacienteservice
                .getPacienteById(idpaciente)
                .then(function (result) {
                    vm.paciente = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

    }
})();