(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('Anamnese', Anamnese);

    Anamnese.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','prontuarioservice','$stateParams'];

    function Anamnese($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice,prontuarioservice,$stateParams) {

        var vm = this;
        vm.init = init;
        vm.crudAnamnese = crudAnamnese;
        vm.alterarfoto = alterarfoto;
        vm.visualizarAnamnese = visualizarAnamnese;
        vm.IdPaciente = $stateParams.id;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
       
        init();

        vm.dtOptions = DTOptionsBuilder
                     .newOptions()
            .withOption('order', [[0, 'desc']]);  
        
        function init() {

            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);


            var blocker = blockUI.instances.get('blockProntuario');
            blocker.start();

             prontuarioservice
                .listarAnamneses($stateParams.id)
                .then(function (result) {
                    vm.dados = result.data;                    
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

        function visualizarAnamnese(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/visualizar.anamnese.html',
                controller: 'VisualizarAnamnese as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                    idpaciente: function () {
                        return vm.IdPaciente;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function crudAnamnese(idpaciente) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/crud.anamnese.html',
                controller: 'CrudAnamnese as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    idpaciente: function () {
                        return idpaciente;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

    }
})();