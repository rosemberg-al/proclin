(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('NovoAgendamentoMedico', NovoAgendamentoMedico)

    NovoAgendamentoMedico.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'agendaservice', 'cadastroservice', 'pacienteservice', 'dados'];

    function NovoAgendamentoMedico($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, DTInstances, DTOptionsBuilder, agendaservice, cadastroservice, pacienteservice, dados) {

        var vm = this;
        vm.steps = ['paciente', 'atendimento', 'confirmacao'];
        vm.currentStep = 'paciente';
        vm.currentIndex = 0;
        vm.finalizado = false;
        vm.nextStepText = 'Próximo';
        vm.passofinal = false;
        vm.avulsa = false;
        vm.valor = 0;
        vm.valorProfissional = 0;

        //TODo
        vm.profissionalSelecionado = undefined;
        vm.pacienteSelecionado = undefined;

        var diaSelecionado;
        vm.datasDisponiveis = [];
        vm.data = '';
        vm.hora = '';
        vm.exibeBotaoAgendar = false;

        //funções paciente
        vm.buscarpaciente = buscarpaciente;
        vm.addpaciente = addpaciente;
        vm.selecionarPaciente = selecionarPaciente;
        vm.agendaDisponivelProfissionalSaude = agendaDisponivelProfissionalSaude;
        vm.listarprocedimentoporEspecialidade = listarprocedimentoporEspecialidade;
        vm.cancelar = cancelar;


        $scope.forms = {};
        vm.formValid = true;
        vm.pesq = {};

        vm.formValid = true;


        //Funções
        vm.init = init;
        vm.nextStep = nextStep;
        vm.prevStep = prevStep;
        vm.save = save;

        //Feature Start
        init();

        function cancelar() {
            $modalInstance.dismiss('cancel');
        }


        //********************************************************************************************************************


        $scope.$watch('vm.pacienteSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                //seta o paciente
                var paciente = _.find(vm.pacientes, { IdPaciente: parseInt(vm.pacienteSelecionado) });
                if (paciente != null)
                    vm.Paciente = paciente.Nome;
            }
        });


        $scope.$watch('vm.especialidadeSelecionada', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                //seta a especialidade
                var espec = _.find(vm.especialidades, { IdEspecialidade: parseInt(vm.especialidadeSelecionada) });
                if (espec != null)
                    vm.Especialidade = espec.NmEspecialidade;
            }
        });

        $scope.$watch('vm.procedimentoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.proctabela = [];
                vm.tipoAgendamentoSelecionado = undefined;
                vm.convenioSelecionado = undefined;
                vm.tabelaprecoSelecionada = undefined;
                vm.valor = 0;
                vm.valorProfissional = 0;

                //seta o procedimento
                var procs = _.find(vm.procedimentos, { IdProcedimento: parseInt(vm.procedimentoSelecionado) });
                if (procs != null)
                    vm.Procedimento = procs.NmProcedimento;

            }
        });

        $scope.$watch('vm.tabelaprecoSelecionada', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                //seta o procedimento
                var pProcs = cadastroservice.getProcedimentoTabela(vm.procedimentoSelecionado, vm.tabelaprecoSelecionada);
                pProcs.then(function (result) {
                    vm.proctabela = result.data;
                    if (vm.proctabela.IdProcedimento > 0) {
                        vm.AlertMessage = "";
                        vm.valor = vm.proctabela.Valor;
                        vm.valorProfissional = vm.proctabela.ValorProfissional;
                    }
                    else {
                        vm.valor = 0;
                        vm.valorProfissional = 0;
                        vm.AlertMessage = "O procedimento selecionado não está associado a tabela.";
                    }
                });

            }
        });


        $scope.$watch('vm.tipoAgendamentoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.tabelas = [];
                //seta o tipo de agendamento
                var tipo = _.find(vm.TipoAgendamento, { Key: vm.tipoAgendamentoSelecionado });
                if (tipo != null)
                    vm.Tipo = tipo.Value;

                var idConv = 0;

                //recupera a tabela de preco
                if (vm.convenioSelecionado != undefined)
                    idConv = vm.convenioSelecionado;

                var pProcs = cadastroservice.getAllTabelasPorConvenio(vm.tipoAgendamentoSelecionado, idConv);
                pProcs.then(function (result) {
                    vm.tabelas = result.data;
                });
            }
        });


        $scope.$watch('vm.convenioSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.tabelas = [];
                var idConv = 0;

                //recupera a tabela de preco
                if (vm.convenioSelecionado != undefined)
                    idConv = vm.convenioSelecionado;

                var pProcs = cadastroservice.getAllTabelasPorConvenio(vm.tipoAgendamentoSelecionado, idConv);
                pProcs.then(function (result) {
                    vm.tabelas = result.data;
                });

                //seta o convenio
                var convenio = _.find(vm.convenios, { IdConvenio: parseInt(vm.convenioSelecionado) });
                if (convenio != null)
                    vm.Convenio = convenio.Nome;
            }
        });

        $scope.$watch('vm.DataAvulsa', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.data = vm.DataAvulsa;
            }
        });

        $scope.$watch('vm.HoraAvulsa', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.hora = vm.HoraAvulsa;
            }
        });



        function dataBrasileira() {
            return vm.data ? moment(vm.data, "YYYY-MM-DD").format("DD/MM/YYYY") : '';
        }

       
        //*******************************************************************************************************************

        //Implementations
        function init() {
            vm.TipoAgendamento = [{ Key: "P", Value: "Particular" }, { Key: "C", Value: "Convênio" }];

            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();
            agendaservice
                .obteragendaporid(dados.IdAgenda)
                .then(function (result) {
                    vm.agenda = result.data;

                    vm.Profissional = result.data.NmProfissionalSaude;
                    vm.profissionalSelecionado = dados.IdProfissional;
                    vm.data = moment(result.data.Data).format("DD/MM/YYYY");
                    vm.hora = result.data.Hora;

                    var pEspecialidades = cadastroservice.listarEspecialidadesPorProfissionalSaude(dados.IdProfissional);
                    pEspecialidades.then(function (result) {
                        vm.especialidades = result.data;
                    });

                    var pConvenios = cadastroservice.listarConvenios();
                    pConvenios.then(function (result) {
                        vm.convenios = result.data;
                    });

                    $q.all([pEspecialidades, pConvenios]).then(function () {
                    })['finally'](function () {
                    }).catch(function (ex) {
                        notification.showError(ex.data.Message);
                    });

                })
                .catch(function (ex) {
                    notification.showError(ex)
                })['finally'](function () {
                    blocker.stop();
                });

            //listarDatas();
        }

        function save() {
            var model = {
                Data: vm.data,
                Hora: vm.hora,
                IdPaciente: vm.pacienteSelecionado,
                IdProfissional: vm.profissionalSelecionado,
                IdAgenda: dados.IdAgenda,
                Observacao: vm.Observacoes,
                IdProcedimento: 0,
                IdConvenio: 0,
                IdEspecialidade: 0,
                IdTipoAtendimento: 1,
                Valor: vm.valor,
                ValorProfissional: vm.valorProfissional,
                Avulsa: false
            };

            if (vm.especialidadeSelecionada != undefined)
                model.IdEspecialidade = vm.especialidadeSelecionada;

            if (vm.convenioSelecionado != undefined)
                model.IdConvenio = vm.convenioSelecionado;

            if (vm.procedimentoSelecionado != undefined)
                model.IdProcedimento = vm.procedimentoSelecionado;


            if (vm.tipoAgendamentoSelecionado != undefined)
                model.IdTipoAtendimento = vm.tipoAgendamentoSelecionado;



            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();

            agendaservice
                .agendar(model)
                .then(function (result) {
                    notification.showSuccessBar("Agendamento criado com sucesso!");
                    blocker.stop();
                    $modalInstance.close(vm.profissionalSelecionado);
                })
                .catch(function (ex) {
                    blocker.stop();
                    notification.showError(ex.data.Message);
                });
        }

        function selecionarPaciente(id) {
            vm.pacienteSelecionado = id;
        }

        function buscarpaciente() {

            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalNovoAgendamento');
                blocker.start();
                if (vm.pesq.Nome == undefined) {
                    vm.pesq.Nome = "";
                }
                pacienteservice
                    .pesquisarPacientes(vm.pesq.Nome, vm.pesq.Codigo)
                    .then(function (result) {
                        vm.pacientes = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }

        }

        function addpaciente(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/crud.paciente.html',
                controller: 'pacienteCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();

                buscarpaciente();
            });
        }

        //Steps e navegacao
        function nextStep() {
            if (vm.currentIndex === _.indexOf(vm.steps, 'paciente')) {
                if (vm.pacienteSelecionado != undefined) {
                    vm.AlertMessage = "";
                    vm.currentIndex += 1;
                    vm.currentStep = vm.steps[vm.currentIndex];
                    applyStep();
                }
                else {
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    vm.AlertClassI = 'fa fa-exclamation-triangle';
                    vm.AlertClassDiv = 'alert alert-danger';
                    vm.AlertMessage = "Para prosseguir você deve selecionar o paciente";
                }
            }
            else if (vm.currentIndex === _.indexOf(vm.steps, 'atendimento')) {
                $scope.showErrorsCheckValidity = true;
                if ($scope.forms.atendimento.$valid) {
                    vm.AlertMessage = "";
                    vm.currentIndex += 1;
                    vm.currentStep = vm.steps[vm.currentIndex];
                    applyStep();
                }
                else {
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    vm.AlertClassI = 'fa fa-exclamation-triangle';
                    vm.AlertClassDiv = 'alert alert-danger';
                    vm.AlertMessage = "Para prosseguir você tem que preencher os campos em vermelho!";
                }
            }
            else {
                vm.currentIndex += 1;
                vm.currentStep = vm.steps[vm.currentIndex];
                applyStep();
            }
        }

        function prevStep() {
            vm.currentIndex -= 1;
            vm.currentStep = vm.steps[vm.currentIndex];
            applyStep();
        }

        function applyStep() {
            if (vm.currentIndex == 0) {
                var aba1 = angular.element(document.querySelector('#aba1'));
                var aba2 = angular.element(document.querySelector('#aba2'));

                var dados1 = angular.element(document.querySelector('#paciente'));
                var dados2 = angular.element(document.querySelector('#atendimento'));
                var dados3 = angular.element(document.querySelector('#confirmacao'));

                dados1.addClass("active");
                dados2.removeClass("active");
                dados3.removeClass("active");

                aba2.removeClass("done");
                aba2.addClass("disabled");
            }
            else if (vm.currentIndex == 1) {
                var aba1 = angular.element(document.querySelector('#aba1'));
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));


                var dados1 = angular.element(document.querySelector('#paciente'));
                var dados2 = angular.element(document.querySelector('#atendimento'));
                var dados3 = angular.element(document.querySelector('#confirmacao'));

                dados2.addClass("active");
                dados1.removeClass("active");
                dados3.removeClass("active");



                aba2.removeClass("disabled");
                aba2.addClass("done");

                aba3.removeClass("done");
                aba3.addClass("disabled");
                vm.passofinal = false;//se voltar para a aba 2 esconde o botão que finaliza e mostra o botão que vai para o próximo passo
            }
            else if (vm.currentIndex == 2) {
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));
                var aba4 = angular.element(document.querySelector('#aba4'));


                var dados1 = angular.element(document.querySelector('#paciente'));
                var dados2 = angular.element(document.querySelector('#atendimento'));
                var dados3 = angular.element(document.querySelector('#confirmacao'));

                dados3.addClass("active");
                dados1.removeClass("active");
                dados2.removeClass("active");

                aba3.removeClass("disabled");
                aba3.addClass("done");

                vm.passofinal = true;//esconde o botão próximo passo e mostra o botão de finalizar
            }
        }

        function agendaDisponivelProfissionalSaude(id) {

            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();

            vm.horarios = [];
            agendaservice
                .listarAgendaAguardando(id)
                .then(function (result) {
                    vm.horarios = result.data;
                })
                .catch(function (ex) {

                })['finally'](function () {
                    blocker.stop();
                });

            var pEspecialidades = cadastroservice.listarEspecialidadesPorProfissionalSaude(id);
            pEspecialidades.then(function (result) {
                vm.especialidades = result.data;
            });

            var pConvenios = cadastroservice.listarConvenios();
            pConvenios.then(function (result) {
                vm.convenios = result.data;
            });

            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();

            $q.all([pEspecialidades, pConvenios]).then(function () {
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });

            vm.procedimentos = null;
        }

        function listarprocedimentoporEspecialidade() {

            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();
            cadastroservice
                .listarProcedimentosPorEspecialidade(vm.especialidadeSelecionada)
                .then(function (result) {
                    vm.procedimentos = result.data;

                    if (vm.procedimentos.length == 0) {
                        vm.AlertMessage = " Nenhum procedimento associado a está especialidade ";
                    } else {
                        vm.AlertMessage = "";
                    }
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

    }
})();