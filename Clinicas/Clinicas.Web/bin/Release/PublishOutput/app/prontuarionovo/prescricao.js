(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('PrescricaoController', PrescricaoController);

    PrescricaoController.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','medicamentoservice','$stateParams'];

    function PrescricaoController($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice, medicamentoservice,$stateParams) {

        var vm = this;
        vm.init = init;
        vm.addprescricao = addprescricao;
        vm.excluirPrescricao = excluirPrescricao;
        vm.printPrescricao = printPrescricao;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        vm.prescricoes = [];
        vm.IdPaciente = $stateParams.id;
       
        init();
        
        function init() {
            vm.dtOptions = DTOptionsBuilder
                .newOptions()
                .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockPresc');
            blocker.start();

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
        function excluirPrescricao(id) {
        }

        function printPrescricao(id) {

        }

        function addprescricao(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/prescricao.crud.html',
                controller: 'PrescricaoCrudController as vm',
                size: 'xl',
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