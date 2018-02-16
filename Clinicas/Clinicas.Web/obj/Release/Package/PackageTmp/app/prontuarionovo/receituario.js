(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('Receituario', Receituario);

    Receituario.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','prontuarioservice','$stateParams'];

    function Receituario($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice,prontuarioservice,$stateParams) {

        var vm = this;
        vm.init = init;
        vm.IdPaciente = $stateParams.id;
        vm.crudReceituario = crudReceituario;
        vm.printReceituario = printReceituario;
        vm.excluirReceituario = excluirReceituario;
        vm.alterarfoto = alterarfoto;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        
       
        init();
        
        function init() {

            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockProntuario');
            blocker.start();

             prontuarioservice
                .listarReceituario($stateParams.id)
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

        function printReceituario(id) {
            prontuarioservice.printReceituario(id);
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

        function crudReceituario(idpaciente) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/crud.receituario.html',
                controller: 'CrudReceituario as vm',
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

         function excluirReceituario(id) {
             vm.askOptions = { Title: 'Excluir', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockProntuario');
                    blocker.start();
                    prontuarioservice.excluirReceituario(id).then(function (result) {
                        notification.showSuccessBar("Exclusão realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }
       

    }
})();