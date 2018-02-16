(function () {
    'use strict';

    angular
        .module('app.atendimento')
        .controller('ReceituarioCrud', ReceituarioCrud);

    ReceituarioCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'funcionarioservice', 'prontuarioservice', 'paciente', 'id'];

    function ReceituarioCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, funcionarioservice, prontuarioservice, paciente, id) {

        var vm = this;
        vm.State = "Incluir Receituário";
        vm.edit = false;
        vm.FormMessage = "";
        $scope.forms = {};
        vm.exibeModelo = true;
        vm.receituario = {
            Paciente: undefined
        };

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            vm.receituario.Paciente = paciente.NmPaciente;
           
            var blocker = blockUI.instances.get('blockModalReceituario');
            blocker.start();

            var pProfissionais = funcionarioservice.listarProfissionaisAtivos();
            pProfissionais.then(function (result) {
                vm.profissionais = result.data;
            });

            var pModelos = prontuarioservice.getModelosReceituarios();
            pModelos.then(function (result) {
                vm.modelosreceituarios = result.data;
            });

            $q.all([pProfissionais, pModelos]).then(function () {
                if (id > 0) {
                    vm.exibeModelo = false;
                    vm.State = "Editar Receituário";
                    vm.receituario.Paciente = paciente.NmPaciente;
                   

                    prontuarioservice
                        .getReceituarioById(id)
                        .then(function (result) {
                            vm.receituario = result.data;
                            vm.receituario.Paciente = paciente.NmPaciente;
                            if (result.data.Situacao == 'Ativo')
                                vm.receituario.Situacao = "A";
                            else
                                vm.receituario.Situacao = "I";

                            //seta o profissional de saúde
                            var profissional = _.find(vm.profissionais, { IdFuncionario: vm.receituario.IdFuncionario });
                            vm.profissionalSelecionado = profissional.IdFuncionario;

                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                        });
                }
                else {
                    vm.receituario.Situacao = "A";
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

        //recupera o modelo de atestado selecionado
        $scope.$watch('vm.modeloSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (vm.modeloSelecionado != undefined) {
                    $scope.disabled = false;
                    var modelo = _.find(vm.modelosreceituarios, { IdModeloProntuario: vm.modeloSelecionado });
                    vm.receituario.Descricao = modelo.Descricao;
                }
            }
        });

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.receituario.$valid) {
                vm.FormMessage = "";
                console.log(vm.receituario);
                var blocker = blockUI.instances.get('blockModalReceituario');
                blocker.start();

                //seta o código d paciente
                vm.receituario.IdPaciente = paciente.IdPaciente;

                if (vm.profissionalSelecionado != undefined)
                    vm.receituario.IdFuncionario = vm.profissionalSelecionado;

                prontuarioservice
                    .saveReceituario(vm.receituario)
                    .then(function (result) {
                        vm.receituario = result.data;

                        if (id == 0)
                            notification.showSuccessBar("Receituário cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Receituário alterado com sucesso!");

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