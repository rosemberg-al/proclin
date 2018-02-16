(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('CrudOdontograma', CrudOdontograma);

    CrudOdontograma.$inject = ['$scope', '$http', '$q',  '$modal','$modalInstance',  'DTOptionsBuilder', 'blockUI', 'common', 'notification','cadastroservice', 'pacienteservice','prontuarioservice','idodontograma','idpaciente'];

    function CrudOdontograma($scope, $http, $q,  $modal, $modalInstance,  DTOptionsBuilder, blockUI, common, notification,cadastroservice, pacienteservice,prontuarioservice,idodontograma,idpaciente) {

        var vm = this;
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        vm.situacoes = [{ Key: "A Realizar", Value: "A Realizar" }, 
            { Key: "Em tratamento", Value: "Em tratamento" },
            { Key: "Realizado", Value: "Realizado" }
        ];

        vm.faces = [{ Key: "Lingual/Palatal", Value: "Lingual/Palatal" }, 
            { Key: "Mesial", Value: "Mesial" },
            { Key: "Distal", Value: "Distal" },
            { Key: "Vestibular", Value: "Vestibular" },
            { Key: "Oclusal", Value: "Oclusal" }
        ];

        init();
        
        function init() {

             var blocker = blockUI.instances.get('blockModal');
            blocker.start();
            
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

            cadastroservice
                .listarProfissionaisSaude()
                .then(function (result) {
                    vm.profissionais = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 


            if(idodontograma>0){

                prontuarioservice
                    .obterOdontogramaPorId(idodontograma)
                    .then(function (result) {
                        vm.odontograma = result.data;    
                        vm.situacaoSelecionado = result.data.Situacao;
                        vm.profissionalSelecionado = result.data.IdFuncionario;
                        vm.faceSelecionado = result.data.Face;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    }); 
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.odontograma.$valid) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModal');
                blocker.start();

                //seta o código d paciente
                vm.odontograma.IdPaciente = idpaciente;
                vm.odontograma.IdFuncionario = vm.profissionalSelecionado;
                vm.odontograma.Situacao = vm.situacaoSelecionado;
                vm.odontograma.Face = vm.faceSelecionado;

                prontuarioservice
                    .salvarOdontograma(vm.odontograma)
                    .then(function (result) {
                        vm.odontograma = result.data;
                        notification.showSuccess("Cadastro realizado com sucesso");
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento.";
            }
        }





    }
})();