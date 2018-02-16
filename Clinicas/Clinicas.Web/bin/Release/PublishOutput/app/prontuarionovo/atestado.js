(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('Atestado', Atestado);

    Atestado.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','prontuarioservice','$stateParams'];

    function Atestado($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice,prontuarioservice,$stateParams) {

        var vm = this;
        vm.init = init;
        vm.alterarfoto = alterarfoto;
        vm.IdPaciente = $stateParams.id;
        vm.crudAtestado = crudAtestado;
        vm.printAtestado = printAtestado;
        vm.excluirAtestado = excluirAtestado;

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
                .listarAtestados($stateParams.id)
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

        function printAtestado(id) {
            prontuarioservice.printAtestado(id);
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

        function crudAtestado(idpaciente) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/crud.atestado.html',
                controller: 'CrudAtestado as vm',
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

        function excluirAtestado(id) {
             vm.askOptions = { Title: 'Excluir', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockProntuario');
                    blocker.start();
                    prontuarioservice.excluirAtestado(id).then(function (result) {
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