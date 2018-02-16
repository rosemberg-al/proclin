    (function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('NovoAgendamento', NovoAgendamento)

    NovoAgendamento.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'agendaservice', 'cadastroservice', 'pacienteservice', '$stateParams'];

    function NovoAgendamento($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, DTInstances, DTOptionsBuilder, agendaservice, cadastroservice, pacienteservice, $stateParams) {

        var vm = this;
        vm.steps = ['medico', 'paciente', 'atendimento', 'horario', 'confirmacao'];
        vm.currentStep = 'medico';
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
        vm.unidadeSelecionada = undefined;

        var diaSelecionado;
        vm.datasDisponiveis = [];
        vm.data = '';
        vm.hora = '';
        vm.exibeBotaoAgendar = false;

        //funções paciente
        vm.buscarpaciente = buscarpaciente;
        vm.addpaciente = addpaciente;
        vm.selecionarPaciente = selecionarPaciente;
        vm.dataHoje = dataHoje;
        vm.agendaDisponivelProfissionalSaude = agendaDisponivelProfissionalSaude;
        vm.listarprocedimentoporEspecialidade = listarprocedimentoporEspecialidade;
        vm.cancelar = cancelar;
        vm.addfuncionario = addfuncionario;


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


        function addfuncionario(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/funcionario/crud.funcionario.html',
                controller: 'funcionarioCrud as vm',
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
            });
        }


        //********************************************************************************************************************


        //quando selecionar a hora, exibir o botão de agendar
        $scope.$watch('vm.hora', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (newValue != oldValue) {
                    vm.exibeBotaoAgendar = true;
                }
            }
        });

        $scope.$watch('vm.pacienteSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                //seta o paciente
                var paciente = _.find(vm.pacientes, { IdPaciente: parseInt(vm.pacienteSelecionado) });
                if (paciente != null)
                    vm.Paciente = paciente.Nome;
            }
        });


        $scope.$watch('vm.profissionalSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.AlertMessage = "";

                //seta o profissional
                var prof = _.find(vm.dados, { IdFuncionario: parseInt(vm.profissionalSelecionado) });
                
                if (prof != null)
                    vm.Profissional = prof.Nome;

                var pEspecialidades = cadastroservice.listarEspecialidadesPorProfissionalSaude(vm.profissionalSelecionado);
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


                $('#divAgenda').html("");
                agendaservice
                    .datasdisponiveis(vm.profissionalSelecionado)
                    .then(function (result) {
                        if (result.data != null) {
                            var resultado = result.data;

                            $('#myCalendar,#myCalendar *').unbind().removeData();
                            $("#myCalendar").html("");
                            $('#zabuto_calendar_aon,#zabuto_calendar_aon *').unbind().removeData();
                            $("#zabuto_calendar_aon").html("");
                            $("#horaResult").html("");

                            var idDiv = "myCalendar_" + Math.floor((Math.random() * 10000) + 1);
                            var ano = new Date().getFullYear();
                            var mes = new Date().getMonth() + 1;

                            $('<div>', { id: idDiv }).appendTo('#myCalendar');

                            if (resultado.Datas && resultado.Datas.length > 0) {
                                var data = resultado.Datas[0].date.split('/');
                                ano = data[0];
                                mes = data[1];
                            }

                            $("#" + idDiv).zabuto_calendar({
                                year: ano,
                                month: mes,
                                data: resultado.Datas,
                                language: "pt",
                                today: true,
                                cell_border: true,
                                show_previous: true,
                                show_next: resultado.Meses.length,
                                weekstartson: 0,
                                nav_icon: {
                                    prev: '<i class="fa fa-chevron-circle-left"></i>',
                                    next: '<i class="fa fa-chevron-circle-right"></i>'
                                },
                                action: function () {
                                    var obj = $('#' + this.id).data();
                                    if (obj && obj.hasEvent) {
                                        if (diaSelecionado) {
                                            diaSelecionado.css('background-color', '');
                                            diaSelecionado.css('color', '');
                                        }

                                        diaSelecionado = $(this).children().first();
                                        diaSelecionado.css('background-color', '#6d5cae');
                                        diaSelecionado.css('color', '#fff');

                                        vm.data = obj.date;
                                        vm.hora = '';
                                        listarHorariosDoPrestador();
                                    }
                                }
                            });
                        }
                    })
                    .catch(function (ex) {
                    });
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

                var pProcs = cadastroservice.getAllTabelasPorConvenioAtivas(vm.tipoAgendamentoSelecionado, idConv);
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

                var pProcs = cadastroservice.getAllTabelasPorConvenioAtivas(vm.tipoAgendamentoSelecionado, idConv);
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



        function listarDatas() {

            var resultado = agendaservice.constants;

            $('#myCalendar,#myCalendar *').unbind().removeData();
            $("#myCalendar").html("");
            $('#zabuto_calendar_aon,#zabuto_calendar_aon *').unbind().removeData();
            $("#zabuto_calendar_aon").html("");
            $("#horaResult").html("");

            var idDiv = "myCalendar_" + Math.floor((Math.random() * 10000) + 1);
            var ano = new Date().getFullYear();
            var mes = new Date().getMonth() + 1;

            $('<div>', { id: idDiv }).appendTo('#myCalendar');

            if (resultado.Datas && resultado.Datas.length > 0) {
                var data = resultado.Datas[0].date.split('/');
                ano = data[0];
                mes = data[1];
            }

            $("#" + idDiv).zabuto_calendar({
                year: ano,
                month: mes,
                data: resultado.Datas,
                language: "pt",
                today: true,
                cell_border: true,
                show_previous: false,
                show_next: resultado.Meses.length - 1,
                weekstartson: 0,
                nav_icon: {
                    prev: '<i class="fa fa-chevron-circle-left"></i>',
                    next: '<i class="fa fa-chevron-circle-right"></i>'
                },
                action: function () {
                    var obj = $('#' + this.id).data();
                    if (obj && obj.hasEvent) {
                        if (diaSelecionado) {
                            diaSelecionado.css('background-color', '');
                            diaSelecionado.css('color', '');
                        }

                        diaSelecionado = $(this).children().first();
                        diaSelecionado.css('background-color', '#6d5cae');
                        diaSelecionado.css('color', '#fff');

                        vm.data = obj.date;
                        vm.hora = '';
                        listarHorariosDoPrestador();
                    }
                }
            });
        }

        function dataBrasileira() {
            return vm.data ? moment(vm.data, "YYYY-MM-DD").format("DD/MM/YYYY") : '';
        }

        function listarHorariosDoPrestador() {


            agendaservice
			.agendaMedica(vm.profissionalSelecionado, vm.data)
			.then(function (result) {
			    if (result.data != null) {
			        var resultado = result.data;
			        vm.horariosRetornados = result.data;

			        $('#divAgenda').html("");
			        $('#divAgenda').append('<h5>Horários disponíves em ' + dataBrasileira() + '</h5>');
			        $('#divAgenda').append('<hr class="hora-divider">');


			        $.each(resultado, function (i, item) {
			            var button = '<button class="btn btnHora" value="' + item.Hora + '" style="width:75px;">' + item.Hora + '</button>';
			            $('#divAgenda').append(button);
			        });

			        $('.btnHora').click(function (e) {
			            e.preventDefault();
			            $('.btnHora').removeClass('btn-primary');
			            $(this).addClass('btn-primary');
			            vm.hora = $(this).val();
			            $scope.$digest()
			            return false;
			        });
			    }
			})
			.catch(function (ex) {
			});

            function visualizar(id) {
                var modalInstance = $modal.open({
                    templateUrl: 'app/agenda/visualizaragenda.html',
                    controller: 'VisualizarAgenda as vm',
                    size: 'lg',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return id;
                        },
                    }
                });
                modalInstance.result.then(function () {
                    listarHorariosDoPrestador();
                });
            }

            function marcar(id) {
                //recupero a agenda selecionada
                var agenda = _.find(vm.horariosRetornados, { IdAgenda: parseInt(id) });
                var modalInstance = $modal.open({
                    templateUrl: 'app/agenda/agendarprocedimento.html',
                    controller: 'AgendarProcedimento as vm',
                    size: 'xl',
                    backdrop: 'static',
                    resolve: {
                        agenda: function () {
                            return agenda;
                        },
                        idProfissional: function () {
                            return vm.profissionalSelecionado
                        }
                    }
                });
                modalInstance.result.then(function () {
                    listarHorariosDoPrestador();
                });
            }
        }


        //*******************************************************************************************************************

        //Implementations
        function init() {
            vm.TipoAgendamento = [{ Key: "P", Value: "Particular" }, { Key: "C", Value: "Convenio" }];

            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();

            var pProfissionais = cadastroservice.listarProfissionaisSaude();
            pProfissionais.then(function (result) {
                vm.dados = result.data;
            });

            var pUnidades = cadastroservice.getDadosClinica();
            pUnidades.then(function (result) {
                vm.unidades = result.data.Unidades;
            });


            $q.all([pUnidades, pProfissionais]).then(function () {

                listarDatas();
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });

            
        }

        function save() {
            if (vm.avulsa) {
                vm.data = vm.DataAvulsa;
                vm.hora = vm.HoraAvulsa;

                var model = {
                    Data: vm.data,
                    Hora: vm.hora,
                    IdPaciente: vm.pacienteSelecionado,
                    IdProfissional: vm.profissionalSelecionado,
                    IdAgenda: 0,
                    Observacao: vm.Observacoes,
                    IdProcedimento: 0,
                    IdConvenio: 0,
                    IdEspecialidade: 0,
                    Valor: vm.valor,
                    ValorProfissional: vm.valorProfissional,
                    IdTipoAtendimento: 1,
                    IdUnidadeAtendimento: 0,
                    Tipo: "",
                    Avulsa: true
                };

                if (vm.especialidadeSelecionada != undefined)
                    model.IdEspecialidade = vm.especialidadeSelecionada;

                if (vm.convenioSelecionado != undefined)
                    model.IdConvenio = vm.convenioSelecionado;

                if (vm.procedimentoSelecionado != undefined)
                    model.IdProcedimento = vm.procedimentoSelecionado;

                if (vm.tipoAgendamentoSelecionado != undefined)
                    model.Tipo = vm.tipoAgendamentoSelecionado;

                if (vm.unidadeSelecionada != undefined)
                    model.IdUnidadeAtendimento = vm.unidadeSelecionada;

                console.log(vm.tipoAgendamentoSelecionado);

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
                    vm.AlertMessage = ex.data.Message;
                });

            }
            else {
                //recupero o id da agenda selecionada
                var agenda = _.find(vm.horariosRetornados, { Hora: vm.hora });
                var model = {
                    Data: vm.data,
                    Hora: vm.hora,
                    IdPaciente: vm.pacienteSelecionado,
                    IdProfissional: vm.profissionalSelecionado,
                    IdAgenda: agenda.IdAgenda,
                    Observacao: vm.Observacoes,
                    IdProcedimento: 0,
                    IdConvenio: 0,
                    IdEspecialidade: 0,
                    IdTipoAtendimento: 1,
                    Valor: vm.valor,
                    Tipo:"",
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

                if (vm.tipoAgendamentoSelecionado != undefined)
                    model.Tipo = vm.tipoAgendamentoSelecionado;

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
        }

        function dataHoje() {
            var data = new Date();
            var dia = data.getDate();
            var mes = data.getMonth() + 1;
            if (mes < 10) {
                mes = "0" + mes;
            }
            var ano = data.getFullYear();
            var horas = new Date().getHours();
            if (horas < 10) {
                horas = "0" + horas;
            }
            var minutos = new Date().getMinutes();
            if (minutos < 10) {
                minutos = "0" + minutos;
            }
            var result = dia + "/" + mes + "/" + ano + " - " + horas + "h" + minutos;
            return result;
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
            if (vm.currentIndex === _.indexOf(vm.steps, 'medico')) {
                if (vm.profissionalSelecionado != undefined) {
                    vm.AlertMessage = "";
                    vm.currentIndex += 1;
                    vm.currentStep = vm.steps[vm.currentIndex];
                    applyStep();
                }
                else {
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    vm.AlertClassI = 'fa fa-exclamation-triangle';
                    vm.AlertClassDiv = 'alert alert-danger';
                    vm.AlertMessage = "Para prosseguir você tem que selecionar o profissional";
                }
            }
            else if (vm.currentIndex === _.indexOf(vm.steps, 'paciente')) {
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
            else if (vm.currentIndex === _.indexOf(vm.steps, 'horario')) {
                if (vm.avulsa) {
                    if (vm.DataAvulsa != undefined && vm.HoraAvulsa != undefined) {
                        vm.AlertMessage = "";
                        vm.currentIndex += 1;
                        vm.currentStep = vm.steps[vm.currentIndex];
                        applyStep();
                    }
                    else {
                        $('html, body').animate({ scrollTop: 0 }, 'slow');
                        vm.AlertClassI = 'fa fa-exclamation-triangle';
                        vm.AlertClassDiv = 'alert alert-danger';
                        vm.AlertMessage = "Para prosseguir você tem que selecionar o horário";
                    }
                }
                else {
                    if (vm.data != "" && vm.hora != "") {
                        vm.AlertMessage = "";
                        vm.currentIndex += 1;
                        vm.currentStep = vm.steps[vm.currentIndex];
                        applyStep();
                    }
                    else {
                        $('html, body').animate({ scrollTop: 0 }, 'slow');
                        vm.AlertClassI = 'fa fa-exclamation-triangle';
                        vm.AlertClassDiv = 'alert alert-danger';
                        vm.AlertMessage = "Para prosseguir você tem que selecionar o horário";
                    }
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
                    vm.AlertMessage = "Para prosseguir você tem que preencher os campos em vermelho";
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

                var dados1 = angular.element(document.querySelector('#medico'));
                var dados2 = angular.element(document.querySelector('#paciente'));
                var dados3 = angular.element(document.querySelector('#atendimento'));
                var dados4 = angular.element(document.querySelector('#horario'));
                var dados5 = angular.element(document.querySelector('#confirmacao'));

                dados1.addClass("active");
                dados2.removeClass("active");
                dados3.removeClass("active");
                dados4.removeClass("active");
                dados5.removeClass("active");

                aba2.removeClass("done");
                aba2.addClass("disabled");
            }
            else if (vm.currentIndex == 1) {
                var aba1 = angular.element(document.querySelector('#aba1'));
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));


                var dados1 = angular.element(document.querySelector('#medico'));
                var dados2 = angular.element(document.querySelector('#paciente'));
                var dados3 = angular.element(document.querySelector('#atendimento'));
                var dados4 = angular.element(document.querySelector('#horario'));
                var dados5 = angular.element(document.querySelector('#confirmacao'));

                dados2.addClass("active");
                dados1.removeClass("active");
                dados3.removeClass("active");
                dados4.removeClass("active");


             
                aba2.removeClass("disabled");
                aba2.addClass("done");

                aba3.removeClass("done");
                aba3.addClass("disabled");
            }
            else if (vm.currentIndex == 2) {
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));
                var aba4 = angular.element(document.querySelector('#aba4'));


                var dados1 = angular.element(document.querySelector('#medico'));
                var dados2 = angular.element(document.querySelector('#paciente'));
                var dados3 = angular.element(document.querySelector('#atendimento'));
                var dados4 = angular.element(document.querySelector('#horario'));
                var dados5 = angular.element(document.querySelector('#confirmacao'));

                dados3.addClass("active");
                dados1.removeClass("active");
                dados2.removeClass("active");
                dados4.removeClass("active");
                dados5.removeClass("active");

                aba3.removeClass("disabled");
                aba3.addClass("done");

                aba4.removeClass("done");
                aba4.addClass("disabled");
                
            }
            else if (vm.currentIndex == 3) {
                var aba3 = angular.element(document.querySelector('#aba3'));
                var aba4 = angular.element(document.querySelector('#aba4'));
                var aba5 = angular.element(document.querySelector('#aba5'));

                var dados1 = angular.element(document.querySelector('#medico'));
                var dados2 = angular.element(document.querySelector('#paciente'));
                var dados3 = angular.element(document.querySelector('#atendimento'));
                var dados4 = angular.element(document.querySelector('#horario'));
                var dados5 = angular.element(document.querySelector('#confirmacao'));


                dados4.addClass("active");
                dados1.removeClass("active");
                dados2.removeClass("active");
                dados3.removeClass("active");
                dados5.removeClass("active");


                aba4.removeClass("disabled");
                aba4.addClass("done");

                aba5.removeClass("last current");
                aba5.addClass("disabled");

                vm.passofinal = false;//se voltar para a aba 3 esconde o botão que finaliza e mostra o botão que vai para o próximo passo
            }
            else if (vm.currentIndex == 4) {
                var aba3 = angular.element(document.querySelector('#aba3'));
                var aba4 = angular.element(document.querySelector('#aba4'));
                var aba5 = angular.element(document.querySelector('#aba5'));


                var dados1 = angular.element(document.querySelector('#medico'));
                var dados2 = angular.element(document.querySelector('#paciente'));
                var dados3 = angular.element(document.querySelector('#atendimento'));
                var dados4 = angular.element(document.querySelector('#horario'));
                var dados5 = angular.element(document.querySelector('#confirmacao'));

                dados5.addClass("active");
                dados1.removeClass("active");
                dados2.removeClass("active");
                dados3.removeClass("active");
                dados4.removeClass("active");

                aba5.removeClass("disabled");
                aba5.addClass("last current");

                vm.passofinal = true;//esconde o botão próximo passo e mostra o botão de finalizar
            }
        }

        function agendaDisponivelProfissionalSaude(id) {

            var blocker = blockUI.instances.get('blockModalNovoAgendamento');
            blocker.start();

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
                    
                    if(vm.procedimentos.length == 0){
                        vm.AlertMessage = " Nenhum procedimento associado a está especialidade ";
                    }else{
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