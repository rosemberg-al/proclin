(function () {
    'use strict';

    angular
        .module('app.prontuario')
        .controller('RequisicaoExame', RequisicaoExame);

    RequisicaoExame.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'funcionarioservice', 'prontuarioservice', 'paciente', 'id'];

    function RequisicaoExame($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, funcionarioservice, prontuarioservice, paciente, id) {

        var vm = this;
        vm.State = "Incluir Requisição de exame";
        vm.FormMessage = "";
        vm.requisicao = {
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
            vm.requisicao.Paciente = paciente.NmPaciente;

            var blocker = blockUI.instances.get('blockModalRequisicao');
            blocker.start();

            vm.tipos = [{ Key: "Urgente", Value: 'Urgente' },
                        { Key: "Rotina", Value: 'Rotina' },
                        { Key: "Controle", Value: 'Controle' }];

            var pMedicos = funcionarioservice.listarMedicos();
            pMedicos.then(function (result) {
                vm.medicos = result.data;
            });

            $q.all([pMedicos]).then(function () {
                if (id > 0) {
                    vm.State = "Editar Requisição de exame";
                    prontuarioservice
                        .getRequisicaoById(id)
                        .then(function (result) {
                            vm.requisicao = result.data;

                            //seta a o tipo
                            var tipo = _.find(vm.tipos, { Key: vm.requisicao.Tipo });
                            vm.tipoSelecionado = tipo.Key;

                            //seta o medico
                            var medico = _.find(vm.medicos, { IdMedico: vm.requisicao.IdMedico });
                            vm.medicoSelecionado = medico.IdMedico;

                            vm.requisicao.Paciente = paciente.NmPaciente;

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

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.requisicao.$valid) {
                vm.FormMessage = "";

                vm.requisicao.IdPaciente = paciente.IdPaciente;

                if (vm.medicoSelecionado != undefined)
                    vm.requisicao.IdMedico = vm.medicoSelecionado;

                if (vm.tipoSelecionado != undefined)
                    vm.requisicao.Tipo = vm.tipoSelecionado;

                var blocker = blockUI.instances.get('blockModalRequisicao');
                blocker.start();

                prontuarioservice
                    .saveRequisicaoExame(vm.requisicao)
                    .then(function (result) {
                        if (id == 0)
                            notification.showSuccessBar("Requisição de exame cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Requisição de exame alterada com sucesso!");

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