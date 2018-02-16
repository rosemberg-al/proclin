(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('AgendaPaciente', AgendaPaciente)

    AgendaPaciente.$inject = ['$scope', '$http', '$q', 'notification', 'blockUI', 'common', '$modal', 'exception', 'ds.agenda', 'ds.paciente', 'ds.funcionario', '$stateParams'];

    function AgendaPaciente($scope, $http, $q, notification, blockUI, common, $modal, exception, dsAgenda, dsPaciente, dsFuncionario, $stateParams) {

        common.setBreadcrumb('atendimento .marcação');
        var vm = this;
        var diaSelecionado;
        vm.datasDisponiveis = [];
        vm.data = '';
        vm.hora = '';
        vm.exibeBotaoAgendar = false;

        //Funções
        vm.init = init;
        vm.agendar = agendar;

        //Feature Start
        init();


        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockModal');
            blocker.start();

            var pProfissionais = dsFuncionario.listarProfissionaisAtivos();
            pProfissionais.then(function (result) {
                vm.profissionais = result.data;
            });

            //var pPacientes = dsPaciente.listar();
            //pPacientes.then(function (result) {
            //    vm.pacientes = result.data;
            //});


            $q.all([pProfissionais]).then(function () {

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });

            //listarDatas();
        }

        function reset() {
            vm.profissionalSelecionado = undefined;
            vm.pacienteSelecionado = undefined;
            vm.exibeBotaoAgendar = false;
        }

        function agendar() {

            //recupero o id da agenda selecionada
            var agenda = _.find(vm.horariosRetornados, { Hora: vm.hora });

            var model = {
                Data: vm.data,
                Hora: vm.hora,
                IdPaciente: vm.pacienteSelecionado,
                IdProfissional: vm.profissionalSelecionado,
                IdAgenda: agenda.IdAgenda
            }

            var blocker = blockUI.instances.get('blockModal');
            blocker.start();

            dsAgenda
			.agendar(model)
			.then(function (result) {
			    notification.showSuccessBar("Agendamento criado com sucesso!");
			    blocker.stop();
			    reset();
			})
			.catch(function (ex) {
			    notification.showError(ex.data.Message);
			});
        }

        //quando selecionar a hora, exibir o botão de agendar
        $scope.$watch('vm.hora', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (newValue != oldValue) {
                    vm.exibeBotaoAgendar = true;
                }
            }
        });

        $scope.$watch('vm.profissionalSelecionado', function (newValue, oldValue) {

            if (angular.isDefined(newValue)) {
                $('#divAgenda').html("");
                dsAgenda
                    .datasdisponiveis(vm.profissionalSelecionado)
                    .then(function (result) {
                        if (result.data != null) {
                            var resultado = result.data;

                            console.log(resultado);

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
                                //var data = resultado.Datas[0].date.split('-');
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
                        exception.throwEx(ex);
                    });
            }
        });


        function listarDatas() {

            var resultado = dsAgenda.constants;

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
                //var data = resultado.Datas[0].date.split('-');
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
            // });
        }

        function dataBrasileira() {
            return vm.data ? moment(vm.data, "YYYY-MM-DD").format("DD/MM/YYYY") : '';
        }

        function listarHorariosDoPrestador() {

            dsAgenda
			.agendaMedica(vm.profissionalSelecionado, vm.data)
			.then(function (result) {
			    if (result.data != null) {
			        var resultado = result.data;
			        vm.horariosRetornados = result.data;

			        $('#divAgenda').html("");
			        $('#divAgenda').append('<h5>Horários disponíves em ' + dataBrasileira() + '</h5>');
			        $('#divAgenda').append('<hr class="hora-divider">');

			        var tabela = '<table class="table table-hover table-condensed table-striped" id="tableAgenda">' +
                                    '<thead>' +
                                     '   <tr class="bg-complete-lighter">' +
                                    '        <th style="width: 15%">Hora </th>' +
                                     '       <th style="width: 25%">Situacao </th>' +
                                     '       <th style="width: 45%">Paciente</th>' +
                                     '       <th style="width: 15%">Ações</th>' +
                                     '   </tr>' +
                                    '</thead>' +
                                    '<tbody></tbody>' +
                                '</table>';

			        $('#divAgenda').append(tabela);


			        $.each(resultado, function (i, item) {
			            var botao = "";
			            var situacao = "";
			            if (item.Situacao == "MARCADO"){
			                botao = '<button type="button" class="btn btn-xs btn-primary btnMarcado" value="' + item.IdAgenda + '" tooltip="Visualizar"> <span class="glyphicon glyphicon-edit"></span> </button>';
			                situacao = '<span class="label label-rounded label-warning">MARCADO</span>';
			            }
			            else if (item.Situacao == "AGUARDANDO"){
			                botao = '<button type="button" class="btn btn-xs btn-success btnAguardando" value="' + item.IdAgenda + '" tooltip="Visualizar"> <span class="glyphicon glyphicon-edit"></span> </button>';
			                situacao =  '<span class="label label-rounded label-success">AGUARDANDO</span>';
			            }
			            else if(item.Situacao == "CANCELADO"){
			                botao = '<button type="button" class="btn btn-xs btn-primary btnMarcado" value="' + item.IdAgenda + '" tooltip="Visualizar"> <span class="glyphicon glyphicon-search"></span> </button>';
			                situacao =  '<span class="label label-rounded label-danger">CANCELADO</span>';
			            }
			            else if (item.Situacao == "REALIZADO") {
			                botao = '<button type="button" class="btn btn-xs btn-primary btnMarcado" value="' + item.IdAgenda + '" tooltip="Visualizar"> <span class="glyphicon glyphicon-search"></span> </button>';
			                situacao = '<span class="label label-rounded label-info">REALIZADO</span>';
			            }

			            var linha = '<tr><td>' + item.Hora + '</td><td>' + situacao + '</td><td>' + item.Paciente + '</td><td>' + botao + '</td></tr>';
			           // var button = '<button class="btn btnHora" value="' + item.Hora + '" style="width:75px;">' + item.Hora + '</button>';
			            // $('#horaResult').append(button);
			            //$('#tableAgenda').remove();
			            
			            $('#tableAgenda').append(linha);
			        });

			        $('.btnHora').click(function (e) {
			            e.preventDefault();
			            $('.btnHora').removeClass('btn-primary');
			            $(this).addClass('btn-primary');
			            vm.hora = $(this).val();
			            $scope.$digest()
			            return false;
			        });
			        $('.btnMarcado').click(function (e) {
			            e.preventDefault();
			            visualizar($(this).val());
			            $scope.$digest()
			            return false;
			        });
			        $('.btnAguardando').click(function (e) {
			            e.preventDefault();
			            marcar($(this).val());
			            $scope.$digest()
			            return false;
			        });
			    }
			})
			.catch(function (ex) {
			    exception.throwEx(ex);
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

    }
})();