(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('AgendarProcedimento', AgendarProcedimento);

    AgendarProcedimento.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'DTInstances', 'ds.agenda', 'ds.funcionario', 'ds.paciente', 'agenda', 'idProfissional'];

    function AgendarProcedimento($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, DTInstances, dsAgenda, dsFuncionario, dsPaciente, agenda, idProfissional) {

        var vm = this;
        vm.FormMessage = "";
        vm.profissionalSelecionado = undefined;
        vm.tipoAgendamentoSelecionado = undefined;
        vm.tipoAtendimentoSelecionado = undefined;
        vm.especialidadeSelecionada = undefined;
        vm.procedimentoSelecionado = undefined;
        vm.convenioSelecionado = undefined;
        vm.pacienteSelecionado = undefined;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.agendar = agendar;
        vm.buscarPaciente = buscarPaciente;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.texto = " para dia " + agenda.Data + " às " + agenda.Hora ;

            var blocker = blockUI.instances.get('blockAgendamentoPaciente');
            blocker.start();

            vm.TipoAgendamento = [{ Key: "P", Value: "Particular" }, { Key: "C", Value: "Convênio" }];
            vm.FormMessage = "";

            var pEspecialidades = dsAgenda.especialidadesPorMedico(idProfissional);
            pEspecialidades.then(function (result) {
                vm.especialidades = result.data;
            });

            var pProcedimentos = dsAgenda.procedimentoPorMedico(idProfissional);
            pProcedimentos.then(function (result) {
                vm.procedimentos = result.data;
            });

            var pTiposAtendimento = dsAgenda.tiposAtendimento();
            pTiposAtendimento.then(function (result) {
                vm.tpatendimentos = result.data;
            });


            var pProfissionais = dsFuncionario.listarProfissionaisAtivos();
            pProfissionais.then(function (result) {
                vm.profissionais = result.data;
                var prof = _.find(vm.profissionais, { IdFuncionario: idProfissional });
                vm.profissionalSelecionado = prof.IdFuncionario;
            });

            var pConvenios = dsAgenda.convenios();
            pConvenios.then(function (result) {
                vm.convenios = result.data;
            });

            $q.all([pEspecialidades, pConvenios, pProfissionais, pProcedimentos, pTiposAtendimento]).then(function () {
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                exception.throwEx(ex);
            });

        }

        $scope.$watch('vm.procedimentoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (newValue != oldValue) {
                    if (vm.procedimentoSelecionado != undefined) {
                        dsAgenda
                            .conveniosPorProcedimento(vm.procedimentoSelecionado)
                            .then(function (result) {
                                vm.convenios = result.data;
                            })
                            .catch(function (ex) {
                                exception.throwEx(ex);
                            });
                    }
                }
            }
        });


        $scope.$watch('vm.tipoAgendamentoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (newValue != oldValue) {
                    if (vm.tipoAgendamentoSelecionado != undefined) {
                        if (vm.tipoAgendamentoSelecionado == "P") {
                            if (vm.procedimentoSelecionado != undefined) {
                                dsAgenda
                                    .valorAtendimento("P", vm.procedimentoSelecionado, 0)
                                    .then(function (result) {
                                        vm.valor = result.data.Valor;
                                        vm.valorProfissional = result.data.ValorProfissional;
                                    })
                                    .catch(function (ex) {
                                        exception.throwEx(ex);
                                    });
                            }
                        }
                    }
                }
            }
        });

        $scope.$watch('vm.convenioSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (newValue != oldValue) {
                    if (vm.convenioSelecionado != undefined) {
                        if (vm.procedimentoSelecionado != undefined) {
                            dsAgenda
                                .valorAtendimento("C", vm.procedimentoSelecionado, vm.convenioSelecionado)
                                .then(function (result) {
                                    vm.valor = result.data.Valor;
                                    vm.valorProfissional = result.data.ValorProfissional;
                                })
                                .catch(function (ex) {
                                    exception.throwEx(ex);
                                });
                        }
                    }
                }
            }
        });



        function buscarPaciente() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/busca.paciente.html',
                controller: 'BuscaPaciente as vm',
                size: 'lg',
                backdrop: 'static'
            });
            modalInstance.result.then(function (item) {
                vm.pacienteSelecionado = item;
                vm.pacienteSelecionadoNome = item.NmPaciente.toUpperCase();
            });
        }

        function agendar() {

            vm.formValid = common.validateForm($scope.forms.agendamentopaciente);

            if (vm.formValid) {

                var blocker = blockUI.instances.get('blockAgendamentoPaciente');
                blocker.start();

                vm.FormMessage = "";

                var model = {
                    Data: agenda.Data,
                    Observacao: vm.Observacoes,
                    Solicitante: vm.Solicitante,
                    Hora: agenda.Hora,
                    Tipo: "",
                    IdPaciente: 0,
                    IdTipoAtendimento: 0,
                    IdProfissional: 0,
                    IdAgenda: agenda.IdAgenda,
                    IdProcedimento: 0,
                    IdConvenio: 0,
                    Valor: vm.valor,
                    ValorProfissional: vm.valorProfissional
                };


                if (vm.profissionalSelecionado != undefined)
                    model.IdProfissional = vm.profissionalSelecionado;

                if (vm.pacienteSelecionado != undefined)
                    model.IdPaciente = vm.pacienteSelecionado.IdPaciente;

                if (vm.tipoAgendamentoSelecionado != undefined)
                    model.Tipo = vm.tipoAgendamentoSelecionado;

                if (vm.tipoAtendimentoSelecionado != undefined)
                    model.IdTipoAtendimento = vm.tipoAtendimentoSelecionado;

                if (vm.procedimentoSelecionado != undefined)
                    model.IdProcedimento = vm.procedimentoSelecionado;

                if (vm.convenioSelecionado != undefined)
                    model.IdConvenio = vm.convenioSelecionado;

                dsAgenda
                    .agendar(model)
                    .then(function (result) {
                        notification.showSuccessBar("Agendamento realizado com sucesso!");
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

    }
})();