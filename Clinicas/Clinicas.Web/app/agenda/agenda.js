(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('Agenda', Agenda);

    Agenda.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'agendaservice', 'cadastroservice', '$stateParams'];

    function Agenda($scope, $http, $q, $modal, blockUI, common, notification, DTInstances, DTOptionsBuilder, agendaservice, cadastroservice, $stateParams) {


        var vm = this;

        common.setBreadcrumb('Atendimento .Agenda .Calendário');

        //Funções
        vm.init = init;
        vm.visualizar = visualizar;
        vm.novoagendamento = novoagendamento;
        vm.selecionar = selecionar;

        vm.busca = {};
        vm.busca.TipoBusca = 'Codigo';
        vm.idprofissionalselecionado = 0;
        vm.aux = undefined;
        vm.inicio = true;

        init();

        vm.dtOptions = DTOptionsBuilder
            .newOptions()
            .withOption('order', [[0, 'desc']]);

        function addeventos(events) {
            $('.example-calendar-block').fullCalendar('removeEvents');
            $('.example-calendar-block').fullCalendar('addEventSource', events);
            $('.example-calendar-block').fullCalendar('rerenderEvents');
        }


        function calendario(dados) {
            //$('.example-calendar-block').fullCalendar('removeEventSources'); 
            if (vm.aux != undefined)
                $('.example-calendar-block').fullCalendar('removeEvents', vm.aux);

            $('.example-calendar-block').fullCalendar({
                //aspectRatio: 2,
                axisFormat: 'H:mm',
                timeFormat: {
                    agenda: 'H:mm{ - H:mm}'
                },
                editable: false,

                height: 1000,
                lang: 'pt-BR',
                header: {
                    left: 'prev, next',
                    center: 'title',
                    right: 'month, agendaWeek, agendaDay'
                },
                buttonIcons: {
                    prev: 'none fa fa-arrow-left',
                    next: 'none fa fa-arrow-right',
                    prevYear: 'none fa fa-arrow-left',
                    nextYear: 'none fa fa-arrow-right'
                },
                editable: true,
                eventLimit: true, // allow "more" link when too many events
                viewRender: function (event, view, element) {
                    //console.log(event.title);
                    //console.log(view);
                    //console.log(element);
                    //var teste = retornaMes(event.title);
                    //console.log(teste);
                    if (event.name == 'month') {
                        //var date = new Date($('.example-calendar-block').fullCalendar('getDate'));
                        //var month_int = parseInt(date.getMonth()) + 2;
                        
                        if (vm.idprofissionalselecionado > 0)
                            carregaAgenda(retornaMes(event.title), vm.idprofissionalselecionado, vm.inicio);
                    }

                    if (!cleanUI.hasTouch) {
                        $('.fc-scroller').jScrollPane({
                            autoReinitialise: true,
                            autoReinitialiseDelay: 100
                        });
                    }
                },
                //defaultDate: '2017-07-07',
                events: dados,
                eventClick: function (calEvent, jsEvent, view) {
                    if (!$(this).hasClass('event-clicked')) {
                        $('.fc-event').removeClass('event-clicked');
                        $(this).addClass('event-clicked');
                    }

                    if (calEvent.className == 'fc-event-warning') {
                        agendamentomedico(calEvent.id, moment(calEvent.start._i).format("DD/MM/YYYY"));
                    } else {
                        vm.visualizar(calEvent.id);
                    }
                }
            });

            vm.aux = dados;
        }

        function retornaMes(mes) {

            if (_.includes(mes, 'Janeiro'))
                return 1;
            else if (_.includes(mes, 'Fevereiro'))
                return 2;
            else if (_.includes(mes, 'Março'))
                return 3;
            else if (_.includes(mes, 'Abril'))
                return 4;
            else if (_.includes(mes, 'Maio'))
                return 5;
            else if (_.includes(mes, 'Junho'))
                return 6;
            else if (_.includes(mes, 'Julho'))
                return 7;
            else if (_.includes(mes, 'Agosto'))
                return 8;
            else if (_.includes(mes, 'Setembro'))
                return 9;
            else if (_.includes(mes, 'Outubro'))
                return 10;
            else if (_.includes(mes, 'Novembro'))
                return 11;
            else if (_.includes(mes, 'Dezembro'))
                return 12;
            else 
                return 1;


        }

        function selecionar(item) {
            var d = new Date();
            var n = d.getMonth();
            console.log('selecionar');
            carregaAgenda(parseInt(n) + 1, item.IdFuncionario, vm.inicio);
            vm.idprofissionalselecionado = item.IdFuncionario;
        }

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
                init();
            });
        }

        function carregaAgenda(mes, idprofissional, init) {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            agendaservice
                .listarAgendaProfissionalSaudePorMes(mes, idprofissional)
                .then(function (result) {
                    if (init == true)
                        calendario(result.data);
                    else
                        addeventos(result.data);
                })
                .catch(function (ex) {

                })['finally'](function () {
                    blocker.stop();
                });
        }

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('block');
            blocker.start();
            cadastroservice
                .listarProfissionaisSaude()
                .then(function (result) {
                    vm.p = result.data;

                    if (result.data.length > 0) {
                        var d = new Date();
                        var n = d.getMonth();
                        console.log('init');
                        carregaAgenda(parseInt(n) + 1, result.data[0].IdFuncionario, vm.inicio);
                        vm.idprofissionalselecionado = result.data[0].IdFuncionario;
                    }
                })
                .catch(function (ex) {

                })['finally'](function () {
                    blocker.stop();
                    vm.inicio = false;
                });
        }

        function agendamentomedico(id, data) {
            var dados = {
                IdPaciente: 0,
                IdAgenda: id,
                IdProfissional: vm.idprofissionalselecionado,
                Data: data
            };
            if (vm.idprofissionalselecionado > 0) {
                var modalInstance = $modal.open({
                    templateUrl: 'app/agenda/novoagendamentomedico.html',
                    controller: 'NovoAgendamentoMedico as vm',
                    size: 'xl',
                    backdrop: 'static',
                    resolve: {
                        dados: function () {
                            return dados;
                        }
                    }
                });
                modalInstance.result.then(function (id) {
                    var d = new Date();
                    var n = d.getMonth();
                    carregaAgenda(parseInt(n) + 1, id, vm.inicio);
                });
            }
            else {
                var modalInstance = $modal.open({
                    templateUrl: 'app/agenda/novoagendamento.html',
                    controller: 'NovoAgendamento as vm',
                    size: 'xl',
                    backdrop: 'static',
                });
                modalInstance.result.then(function (id) {
                    var d = new Date();
                    var n = d.getMonth();
                    carregaAgenda(parseInt(n) + 1, id, vm.inicio);
                });
            }
        }

        function novoagendamento() {
            var modalInstance = $modal.open({
                templateUrl: 'app/agenda/novoagendamento.html',
                controller: 'NovoAgendamento as vm',
                size: 'xl',
                backdrop: 'static',
            });
            modalInstance.result.then(function (id) {
                var d = new Date();
                var n = d.getMonth();
                carregaAgenda(parseInt(n) + 1, id, vm.inicio);
            });
        }

    }
})();