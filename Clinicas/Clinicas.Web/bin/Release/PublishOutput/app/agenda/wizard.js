(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('Wizard', Wizard);

    Wizard.$inject = ['$scope', '$state', '$http', '$q', '$timeout', '$modal', 'blockUI', 'DTInstances', 'DTOptionsBuilder', 'common', 'notification', 'cadastroservice', 'agendaservice'];

    function Wizard($scope, $state, $http, $q, $timeout, $modal, blockUI, DTInstances, DTOptionsBuilder, common, notification, cadastroservice, agendaservice) {

        var vm = this;
        vm.steps = ['medicos', 'horarios', 'confirmacao'];
        vm.currentStep = 'medicos';
        vm.currentIndex = 0;
        vm.finalizado = false;
        vm.nextStepText = 'Próximo';
        vm.vamoscomecar = true;
        vm.agendamedica = [];
        vm.passofinal = false;
        vm.domingo = { checado: false, possuiintervalos: false };
        vm.segunda = { checado: false, possuiintervalos: false };
        vm.terca = { checado: false, possuiintervalos: false };
        vm.quarta = { checado: false, possuiintervalos: false };
        vm.quinta = { checado: false, possuiintervalos: false };
        vm.sexta = { checado: false, possuiintervalos: false };
        vm.sabado = { checado: false, possuiintervalos: false };
        vm.unidadeSelecionada = undefined;

        vm.addfuncionario= addfuncionario;

        $scope.forms = {};
        vm.formValid = true;
        vm.diasselecionados = [];
        vm.diasselecionadosaux = [];
        vm.desabilita = false;
        vm.DiaSemana = {
            Domingo: false,
            Segunda: false,
            Terca: false,
            Quarta: false,
            Quinta: false,
            Sexta: false,
            Sabado: false
        };
        vm.agenda = {
            Dia: undefined,
            IntervaloMinutos: undefined,
            HoraInicio: undefined,
            HoraTermino: undefined,
            IntervaloInicio: undefined,
            IntervaloTermino: undefined,
        }
        common.setBreadcrumb('Agenda .Plano de Agenda');

        //Funções
        vm.init = init;
        vm.nextStep = nextStep;
        vm.prevStep = prevStep;
       // vm.adicionar = adicionar;
        vm.gerar = gerar;


        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModalWizard');
            blocker.start();

            var pProfissionais = cadastroservice.listarProfissionaisSaude();
            pProfissionais.then(function (result) {
                vm.medicos = result.data;
            });

            var pUnidades = cadastroservice.getDadosClinica();
            pUnidades.then(function (result) {
                vm.unidades = result.data.Unidades;
            });


            $q.all([pUnidades, pProfissionais]).then(function () {

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });

           
            //cadastroservice
            //    .listarProfissionaisSaude()
            //    .then(function (result) {
            //        vm.medicos = result.data;
            //    })
            //    .catch(function (ex) {
            //        notification.showError(ex)
            //    })['finally'](function () {
            //        blocker.stop();
            //    });
        }

        function gerar() {
            vm.agenda = {
                DataInicio: vm.DataInicio,
                DataFim: vm.DataTermino,
                AgendaMedica: vm.agendamedica,
                IdProfissional: vm.profissionalSelecionado,
                IdUnidadeAtendimento: vm.unidadeSelecionada == undefined ? 0 : vm.unidadeSelecionada
            };

            var blocker = blockUI.instances.get('blockModalWizard');
            blocker.start();
            agendaservice
                .gerarAgenda(vm.agenda)
                .then(function (result) {
                    vm.askOptions = { Title: 'Agenda gerada com sucesso!', Text: 'Desejar cadastrar um novo plano de agenda ?', Yes: 'Sim', No: 'Não' };
                    notification.ask(vm.askOptions, function (confirm) {
                        if (confirm) {
                            reset();
                        }
                        else {
                            $state.go('agenda');
                        }
                    });
                })
               .catch(function (ex) {
                   notification.showError(ex)
               })['finally'](function () {
                   blocker.stop();
               });
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

        function reset() {
            vm.currentStep = 'medicos';
            vm.currentIndex = 0;
            vm.finalizado = false;
            vm.nextStepText = 'Próximo';
            vm.vamoscomecar = true;
            vm.agendamedica = [];
            vm.passofinal = false;
            vm.possuiintervalos = false;
            vm.DataInicio = undefined;
            vm.DataTermino = undefined;
            vm.profissionalSelecionado = undefined;
            vm.agenda = {
                Dia: undefined,
                IntervaloMinutos: undefined,
                HoraInicio: undefined,
                HoraTermino: undefined,
                IntervaloInicio: undefined,
                IntervaloTermino: undefined,
            }
            vm.diasselecionados = [];
            vm.diasselecionadosaux = [];
            vm.desabilita = false;
            vm.domingo = { checado: false, possuiintervalos: false };
            vm.segunda = { checado: false, possuiintervalos: false };
            vm.terca = { checado: false, possuiintervalos: false };
            vm.quarta = { checado: false, possuiintervalos: false };
            vm.quinta = { checado: false, possuiintervalos: false };
            vm.sexta = { checado: false, possuiintervalos: false };
            vm.sabado = { checado: false, possuiintervalos: false };
            applyStep();
        }


        $scope.$watchCollection('vm.domingo.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.domingo.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosDomingo.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosDomingo.$invalid = false;

                if (vm.domingo.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioDomingo.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioDomingo.$invalid = false;

                if (vm.domingo.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoDomingo.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoDomingo.$invalid = false;

                if (vm.domingo.possuiintervalos) {
                    if (vm.domingo.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioDomingo.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioDomingo.$invalid = false;

                    if (vm.domingo.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoDomingo.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoDomingo.$invalid = false;
                }

                if (!vm.domingo.checado) {
                    vm.domingo.IntervaloMinutos = undefined;
                    vm.domingo.HoraInicio = undefined;
                    vm.domingo.HoraTermino = undefined;
                    vm.domingo.IntervaloInicio = undefined;
                    vm.domingo.IntervaloTermino = undefined;
                }
            }
        });


        $scope.$watchCollection('vm.segunda.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.segunda.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosSegunda.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosSegunda.$invalid = false;

                if (vm.segunda.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioSegunda.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioSegunda.$invalid = false;

                if (vm.segunda.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoSegunda.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoSegunda.$invalid = false;

                if (vm.segunda.possuiintervalos) {
                    if (vm.segunda.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioSegunda.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioSegunda.$invalid = false;

                    if (vm.segunda.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoSegunda.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoSegunda.$invalid = false;
                }

                if (!vm.segunda.checado) {
                    vm.segunda.IntervaloMinutos = undefined;
                    vm.segunda.HoraInicio = undefined;
                    vm.segunda.HoraTermino = undefined;
                    vm.segunda.IntervaloInicio = undefined;
                    vm.segunda.IntervaloTermino = undefined;
                }
            }
        });


        $scope.$watchCollection('vm.terca.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.terca.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosTerca.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosTerca.$invalid = false;

                if (vm.terca.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioTerca.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioTerca.$invalid = false;

                if (vm.terca.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoTerca.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoTerca.$invalid = false;

                if (vm.terca.possuiintervalos) {
                    if (vm.terca.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioTerca.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioTerca.$invalid = false;

                    if (vm.terca.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoTerca.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoTerca.$invalid = false;
                }
                if (!vm.terca.checado) {
                    vm.terca.IntervaloMinutos = undefined;
                    vm.terca.HoraInicio = undefined;
                    vm.terca.HoraTermino = undefined;
                    vm.terca.IntervaloInicio = undefined;
                    vm.terca.IntervaloTermino = undefined;
                }
            }
        });

        $scope.$watchCollection('vm.quarta.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.quarta.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosQuarta.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosQuarta.$invalid = false;

                if (vm.quarta.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioQuarta.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioQuarta.$invalid = false;

                if (vm.quarta.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoQuarta.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoQuarta.$invalid = false;

                if (vm.quarta.possuiintervalos) {
                    if (vm.quarta.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioQuarta.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioQuarta.$invalid = false;

                    if (vm.quarta.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoQuarta.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoQuarta.$invalid = false;
                }

                if (!vm.quarta.checado) {
                    vm.quarta.IntervaloMinutos = undefined;
                    vm.quarta.HoraInicio = undefined;
                    vm.quarta.HoraTermino = undefined;
                    vm.quarta.IntervaloInicio = undefined;
                    vm.quarta.IntervaloTermino = undefined;
                }
            }
        });


        $scope.$watchCollection('vm.quinta.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.quinta.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosQuinta.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosQuinta.$invalid = false;

                if (vm.quinta.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioQuinta.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioQuinta.$invalid = false;

                if (vm.quinta.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoQuinta.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoQuinta.$invalid = false;

                if (vm.quinta.possuiintervalos) {
                    if (vm.quinta.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioQuinta.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioQuinta.$invalid = false;

                    if (vm.quinta.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoQuinta.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoQuinta.$invalid = false;
                }

                if (!vm.quinta.checado) {
                    vm.quinta.IntervaloMinutos = undefined;
                    vm.quinta.HoraInicio = undefined;
                    vm.quinta.HoraTermino = undefined;
                    vm.quinta.IntervaloInicio = undefined;
                    vm.quinta.IntervaloTermino = undefined;
                }
            }
        });


        $scope.$watchCollection('vm.sexta.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.sexta.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosSexta.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosSexta.$invalid = false;

                if (vm.sexta.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioSexta.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioSexta.$invalid = false;

                if (vm.sexta.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoSexta.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoSexta.$invalid = false;

                if (vm.sexta.possuiintervalos) {
                    if (vm.sexta.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioSexta.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioSexta.$invalid = false;

                    if (vm.sexta.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoSexta.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoSexta.$invalid = false;
                }

                if (!vm.sexta.checado) {
                    vm.sexta.IntervaloMinutos = undefined;
                    vm.sexta.HoraInicio = undefined;
                    vm.sexta.HoraTermino = undefined;
                    vm.sexta.IntervaloInicio = undefined;
                    vm.sexta.IntervaloTermino = undefined;
                }
            }
        });

        $scope.$watchCollection('vm.sabado.checado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (vm.sabado.IntervaloMinutos == undefined)
                    $scope.forms.diasagenda.intervaloMinutosSabado.$invalid = true;
                else
                    $scope.forms.diasagenda.intervaloMinutosSabado.$invalid = false;

                if (vm.sabado.HoraInicio == undefined)
                    $scope.forms.diasagenda.DataHoraInicioSabado.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraInicioSabado.$invalid = false;

                if (vm.sabado.HoraTermino == undefined)
                    $scope.forms.diasagenda.DataHoraTerminoSabado.$invalid = true;
                else
                    $scope.forms.diasagenda.DataHoraTerminoSabado.$invalid = false;

                if (vm.sabado.possuiintervalos){
                    if (vm.sabado.IntervaloInicio == undefined)
                        $scope.forms.diasagenda.dataIntervaloInicioSabado.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloInicioSabado.$invalid = false;

                    if (vm.sabado.IntervaloTermino == undefined)
                        $scope.forms.diasagenda.dataIntervaloTerminoSabado.$invalid = true;
                    else
                        $scope.forms.diasagenda.dataIntervaloTerminoSabado.$invalid = false;
                }

                if (!vm.sabado.checado) {
                    vm.sabado.IntervaloMinutos = undefined;
                    vm.sabado.HoraInicio = undefined;
                    vm.sabado.HoraTermino = undefined;
                    vm.sabado.IntervaloInicio = undefined;
                    vm.sabado.IntervaloTermino = undefined;
                }
            }
        });

        function montarvalores() {
            if (vm.domingo.checado) {
                vm.agenda = {
                    Dia: 'Domingo',
                    IntervaloMinutos: vm.domingo.IntervaloMinutos,
                    HoraInicio: vm.domingo.HoraInicio,
                    HoraTermino: vm.domingo.HoraTermino,
                    IntervaloInicio: vm.domingo.IntervaloInicio,
                    IntervaloTermino: vm.domingo.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
            if (vm.segunda.checado) {
                vm.agenda = {
                    Dia: 'Segunda',
                    IntervaloMinutos: vm.segunda.IntervaloMinutos,
                    HoraInicio: vm.segunda.HoraInicio,
                    HoraTermino: vm.segunda.HoraTermino,
                    IntervaloInicio: vm.segunda.IntervaloInicio,
                    IntervaloTermino: vm.segunda.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
            if (vm.terca.checado) {
                vm.agenda = {
                    Dia: 'Terca',
                    IntervaloMinutos: vm.terca.IntervaloMinutos,
                    HoraInicio: vm.terca.HoraInicio,
                    HoraTermino: vm.terca.HoraTermino,
                    IntervaloInicio: vm.terca.IntervaloInicio,
                    IntervaloTermino: vm.terca.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
            if (vm.quarta.checado) {
                vm.agenda = {
                    Dia: 'Quarta',
                    IntervaloMinutos: vm.quarta.IntervaloMinutos,
                    HoraInicio: vm.quarta.HoraInicio,
                    HoraTermino: vm.quarta.HoraTermino,
                    IntervaloInicio: vm.quarta.IntervaloInicio,
                    IntervaloTermino: vm.quarta.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
            if (vm.quinta.checado) {
                vm.agenda = {
                    Dia: 'Quinta',
                    IntervaloMinutos: vm.quinta.IntervaloMinutos,
                    HoraInicio: vm.quinta.HoraInicio,
                    HoraTermino: vm.quinta.HoraTermino,
                    IntervaloInicio: vm.quinta.IntervaloInicio,
                    IntervaloTermino: vm.quinta.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
            if (vm.sexta.checado) {
                vm.agenda = {
                    Dia: 'Sexta',
                    IntervaloMinutos: vm.sexta.IntervaloMinutos,
                    HoraInicio: vm.sexta.HoraInicio,
                    HoraTermino: vm.sexta.HoraTermino,
                    IntervaloInicio: vm.sexta.IntervaloInicio,
                    IntervaloTermino: vm.sexta.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
            if (vm.sabado.checado) {
                vm.agenda = {
                    Dia: 'Sabado',
                    IntervaloMinutos: vm.sabado.IntervaloMinutos,
                    HoraInicio: vm.sabado.HoraInicio,
                    HoraTermino: vm.sabado.HoraTermino,
                    IntervaloInicio: vm.sabado.IntervaloInicio,
                    IntervaloTermino: vm.sabado.IntervaloTermino,
                };
                vm.agendamedica.push(vm.agenda);
            }
        }

        //Steps e navegacao
        function nextStep() {
            if (vm.currentIndex === _.indexOf(vm.steps, 'medicos')) {
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
                    vm.AlertMessage = "Para prosseguir você tem que selecionar o profissional!";
                }
            }
            else if (vm.currentIndex === _.indexOf(vm.steps, 'horarios')) {
                if (moment(vm.DataInicio).isAfter(vm.DataTermino)) {
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    vm.AlertClassI = 'fa fa-exclamation-triangle';
                    vm.AlertClassDiv = 'alert alert-danger';
                    vm.AlertMessage = "A data inicial não pode ser menor que a data final!";
                    $scope.forms.datas.DataInicio.$invalid = true;
                    $scope.forms.datas.DataTermino.$invalid = true;
                    return;
                }
                else {
                    vm.AlertMessage = "";
                    $scope.forms.datas.DataInicio.$invalid = false;
                    $scope.forms.datas.DataTermino.$invalid = false;
                }

                $scope.showErrorsCheckValidity = true;
                var formagenda = $scope.forms.diasagenda.$valid;
                var formdatas = $scope.forms.datas.$valid;

                if (formagenda && formdatas) {
                    montarvalores();
                    if (vm.agendamedica.length > 0){
                        vm.AlertMessage = "";
                        vm.currentIndex += 1;
                        vm.currentStep = vm.steps[vm.currentIndex];
                        applyStep();
                    }
                    else {
                        $('html, body').animate({ scrollTop: 0 }, 'slow');
                        vm.AlertClassI = 'fa fa-exclamation-triangle';
                        vm.AlertClassDiv = 'alert alert-danger';
                        vm.AlertMessage = "Você deve adicionar os dias da semana com os horários para prosseguir!";
                    }
                }
                else {
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    vm.AlertClassI = 'fa fa-exclamation-triangle';
                    vm.AlertClassDiv = 'alert alert-danger';
                    vm.AlertMessage = "Para prosseguir você tem que preencher os horários!";
                }
            }
            else {
                vm.AlertMessage = "";
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

        $scope.$watch('vm.profissionalSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                //seta o profissional
                var prof = _.find(vm.medicos, { IdFuncionario: parseInt(vm.profissionalSelecionado) });
                if (prof != null)
                    vm.Profissional = prof.Nome;
            }
        });

        function applyStep() {
            if (vm.currentIndex == 0) {

                var aba1 = angular.element(document.querySelector('#aba1'));
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));

                var dados1 = angular.element(document.querySelector('#medicos'));
                var dados2 = angular.element(document.querySelector('#horarios'));
                var dados3 = angular.element(document.querySelector('#confirmacao'));

                dados2.removeClass("active");
                dados3.removeClass("active");
                dados1.addClass("active");

                aba3.removeClass("step-block step-primary");
                aba3.addClass("step-block step-default step-disabled");
                aba2.removeClass("step-block step-primary");
                aba2.addClass("step-block step-default step-disabled");
                aba1.removeClass("step-block step-default step-disabled");
                aba1.addClass("step-block step-primary");
                vm.passofinal = false;
            }
            else if (vm.currentIndex == 1) {

                //$timeout(function () {
                //    vm.vamoscomecar = false;
                //}, 4000);

                var aba1 = angular.element(document.querySelector('#aba1'));
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));

                var dados1 = angular.element(document.querySelector('#medicos'));
                var dados2 = angular.element(document.querySelector('#horarios'));
                var dados3 = angular.element(document.querySelector('#confirmacao'));

                dados3.removeClass("active");
                dados1.removeClass("active");
                dados2.addClass("active");


                aba1.removeClass("step-block step-primary");
                aba1.addClass("step-block step-default step-disabled");
                aba2.removeClass("step-block step-default step-disabled");
                aba2.addClass("step-block step-primary");
                aba3.removeClass("step-block step-primary");
                aba3.addClass("step-block step-default step-disabled");
                vm.passofinal = false;

            }
            else if (vm.currentIndex == 2) {
                
                vm.passofinal = true;
                var aba2 = angular.element(document.querySelector('#aba2'));
                var aba3 = angular.element(document.querySelector('#aba3'));

                vm.dataIni = moment(vm.DataInicio).format("DD/MM/YYYY");
                vm.dataFim = moment(vm.DataTermino).format("DD/MM/YYYY");

                var dados2 = angular.element(document.querySelector('#horarios'));
                var dados3 = angular.element(document.querySelector('#confirmacao'));

                dados2.removeClass("active");
                dados3.addClass("active");


                aba2.removeClass("step-block step-primary");
                aba2.addClass("step-block step-default step-disabled");
                aba3.removeClass("step-block step-default step-disabled");
                aba3.addClass("step-block step-primary");

            }
        }

    }


})();