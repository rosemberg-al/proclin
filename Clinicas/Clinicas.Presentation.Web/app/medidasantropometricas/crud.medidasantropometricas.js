(function () {
    'use strict';

    angular
        .module('app.medidasantropometricas')
        .controller('MedidasCrud', MedidasCrud);

    MedidasCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.prontuario', 'paciente', 'id'];

    function MedidasCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsProntuario, paciente, id) {

        var vm = this;
        vm.State = "Incluir Medidas Antropométricas";
        vm.FormMessage = "";
        vm.medidaantropmetrica = {
            Paciente: undefined
        };


        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.calcularImc = calcularImc;

        //Feature Start
        init();

        //Implementations
        function init() {

            vm.FormMessage = "";
            vm.medidaantropmetrica.Paciente = paciente.NmPaciente;
            if (id > 0) {
                vm.State = "Editar Medidas Antropométricas";
                var blocker = blockUI.instances.get('blockModalMedidas');
                blocker.start();

                dsProntuario
                    .getMedidasById(id)
                    .then(function (result) {
                        vm.medidaantropmetrica = result.data;
                        vm.medidaantropmetrica.Paciente = paciente.NmPaciente;
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
            }
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.medidas);

            if (vm.formValid) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalMedidas');
                blocker.start();

                //seta o código d paciente
                vm.medidaantropmetrica.IdPaciente = paciente.IdPaciente;

                dsProntuario
                    .saveMedidas(vm.medidaantropmetrica)
                    .then(function (result) {
                        vm.medidaantropmetrica = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Medidas Antropométricas cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Medidas Antropométricas alterada com sucesso!");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }

    }
})();