(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('Odontograma', Odontograma);

    Odontograma.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','prontuarioservice','$stateParams'];

    function Odontograma($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice,prontuarioservice,$stateParams) {

        var vm = this;
        vm.init = init;
        vm.alterarfoto = alterarfoto;
        vm.crudOdontograma= crudOdontograma;
        vm.excluirOdontograma = excluirOdontograma;
        vm.print = print;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        vm.IdPaciente = $stateParams.id;
       
        init();
        
        function init() {

            if(vm.IdPaciente>0){

             prontuarioservice
                .listarOdontogramaPorIdPaciente(vm.IdPaciente)
                .then(function (result) {
                    vm.dados = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
            }

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

        function print(id) {
            prontuarioservice.printOrcamento(id);
        }

        function crudOdontograma(idodontograma,idpaciente) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/crud.odontograma.html',
                controller: 'CrudOdontograma as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    idodontograma: function () {
                        return idodontograma;
                    },
                    idpaciente: function () {
                        return idpaciente;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

       function excluirOdontograma(id) {
                vm.askOptions = { Title: 'Excluir ', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
                notification.ask(vm.askOptions, function (confirm) {
                    if (confirm) {

                        var blocker = blockUI.instances.get('blockModal');
                        blocker.start();
                        prontuarioservice.excluirOdontograma(id).then(function (result) {
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


    }
})();