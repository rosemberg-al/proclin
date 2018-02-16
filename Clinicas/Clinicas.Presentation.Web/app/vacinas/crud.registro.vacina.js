(function () {
    'use strict';

    angular
        .module('app.vacinas')
        .controller('RegistroVacina', RegistroVacina);

    RegistroVacina.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'DTInstances', 'ds.vacinas', 'paciente', 'id'];

    function RegistroVacina($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, DTInstances, dsVacinas, paciente, id) {

        var vm = this;
        vm.State = "Incluir Registro de Vacina";
        vm.FormMessage = "";
        vm.registro = {
            Paciente: undefined
        };
        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.registro.Paciente = paciente.NmPaciente;

            var blocker = blockUI.instances.get('blockmodalRegistro');
            blocker.start();

            vm.doses = [{ Id: "1", Descricao: 'Primeira Dose' },
                        { Id: "2", Descricao: 'Segunda Dose' },
                        { Id: "3", Descricao: 'Terceira Dose' },
                        { Id: "4", Descricao: 'Quarta Dose' }
                        ];

            var pVacinas = dsVacinas.getVacinasAtivas();
            pVacinas.then(function (result) {
                vm.vacinas = result.data;
            });

            $q.all([pVacinas]).then(function () {
                if (id > 0) {
                    vm.State = "Editar Registro de Vacina";
                    dsVacinas
                        .getRegistroVacinaById(id)
                        .then(function (result) {
                            vm.registro = result.data;

                            //seta a dose
                            var dose = _.find(vm.doses, { Id: vm.registro.Dose });
                            vm.doseSelecionada = dose.Id;

                            //seta a vacina
                            var vacina = _.find(vm.vacinas, { IdVacina: vm.registro.IdVacina });
                            vm.vacinaSelecionada = vacina.IdVacina;

                            vm.registro.Paciente = paciente.NmPaciente;

                            vm.registro.Hora = result.data.HoraVacina;

                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                        });
                }
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });

            
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.vacinas);

            if (vm.formValid) {

                vm.registro.IdPaciente = paciente.IdPaciente;

                if (vm.vacinaSelecionada != undefined)
                    vm.registro.IdVacina = vm.vacinaSelecionada;

                if (vm.doseSelecionada != undefined)
                    vm.registro.Dose = vm.doseSelecionada;

                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockmodalRegistro');
                blocker.start();

                dsVacinas
                    .saveRegistroVacina(vm.registro)
                    .then(function (result) {
                        if (id == 0)
                            notification.showSuccessBar("Registro de vacina cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Registro de vacina alterada com sucesso!");

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