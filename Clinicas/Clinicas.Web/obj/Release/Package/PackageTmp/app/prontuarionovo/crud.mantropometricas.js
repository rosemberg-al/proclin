(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('CrudMAntrometrica', CrudMAntrometrica);

    CrudMAntrometrica.$inject = ['$scope', '$http', '$q',  '$modal','$modalInstance',  'DTOptionsBuilder', 'blockUI', 'common', 'notification','cadastroservice', 'pacienteservice','prontuarioservice','idpaciente','idmedida'];

    function CrudMAntrometrica($scope, $http, $q,  $modal, $modalInstance,  DTOptionsBuilder, blockUI, common, notification,cadastroservice, pacienteservice,prontuarioservice,idpaciente,idmedida) {

        var vm = this;
        vm.init = init;
        vm.save = save;
        vm.calcularImc = calcularImc;
        vm.cancel = cancel;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

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

            blocker.start();
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

            if(idmedida>0){

                blocker.start();
                prontuarioservice
                .obterMedidas(idmedida)
                .then(function (result) {
                    vm.medidaantropmetrica = result.data;   
                    vm.profissionalSelecionado = vm.medidaantropmetrica.IdProfissionalSaude;
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

        function calcularImc() {

            if(vm.medidaantropmetrica.Peso != undefined && vm.medidaantropmetrica.Altura != undefined)
            {
                if (vm.medidaantropmetrica.Peso != "" && vm.medidaantropmetrica.Altura != "") {
                    var kilos = parseFloat(vm.medidaantropmetrica.Peso);
                    var metros = parseFloat(vm.medidaantropmetrica.Altura);

                    var altura = parseFloat((metros * 100) / 100);
                    var imc = kilos / (altura * altura);

                    vm.medidaantropmetrica.Imc = imc.toFixed(2);
                }
            }else{
                 vm.medidaantropmetrica.Imc =0;
            }
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModal');
                blocker.start();

                //seta o código d paciente
                vm.medidaantropmetrica.IdPaciente = idpaciente;
                vm.medidaantropmetrica.IdProfissionalSaude = vm.profissionalSelecionado;

                prontuarioservice
                    .salvarMedidas(vm.medidaantropmetrica)
                    .then(function (result) {
                        notification.showSuccess("Medidas antropométricas cadastrada com sucesso");
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