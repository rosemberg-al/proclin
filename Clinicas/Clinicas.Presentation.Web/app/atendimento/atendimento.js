(function () {
    'use strict';

    angular
        .module('app.atendimento')
        .controller('Atendimento', Atendimento);

    Atendimento.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', '$modal', 'exception', 'notification', 'DTOptionsBuilder', 'ds.dashboard', 'ds.receituario', 'ds.atestado', 'ds.prontuario', 'ds.paciente', 'ds.anamnese', 'ds.vacinas', 'ds.agenda', '$stateParams'];

    function Atendimento($scope, $http, $q, blockUI, common, $modal, exception, notification, DTOptionsBuilder, DsDashboard, dsReceituario, dsAtestado, dsProntuario, dsPaciente, dsAnamnese, dsVacinas, dsAgenda, $stateParams) {

        common.setBreadcrumb('Atendimento');
        var vm = this;

        //Funções
        vm.init = init;
        vm.anamneseModal = anamneseModal;
        vm.receituarioModal = receituarioModal;
        vm.abrirAnamnese = abrirAnamnese;
        vm.cadastro = cadastro;
        vm.vacinasModal = vacinasModal;
        vm.atestadosModal = atestadosModal;
        vm.historiaModal = historiaModal;
        vm.medidasModal = medidasModal;
        vm.ExamesModal = ExamesModal;
        vm.visualizarAtendimento = visualizarAtendimento;
        vm.printAtestado = printAtestado;
        vm.printReceituario = printReceituario;
        vm.excluirmedidas = excluirmedidas;
        vm.printRequisicao = printRequisicao;

        //Feature Start
        init();


        //Implementations
        function init() {

            vm.dtOptions = DTOptionsBuilder.newOptions().withOption('order', [0, 'desc']);

            if ($stateParams.id > 0) {
                var blocker = blockUI.instances.get('blockPaciente');
                blocker.start();

                dsPaciente
                    .getById($stateParams.id)
                    .then(function (result) {
                        vm.pacienteAtendimento = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            dadospaciente();
        }


        function printAtestado(id) {
            dsAtestado.printAtestado(id);
        }

        function printReceituario(id) {
            dsReceituario.printReceituario(id);
        }

        function printRequisicao(id) {
            dsProntuario.printRequisicao(id);
        }

        function visualizarAtendimento(id) {
            console.log(id);
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
            });
        }

        function cadastro(paciente) {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/crud.paciente.html',
                controller: 'PacienteCrud as vm',
                size: 'xl',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }


        function dadospaciente() {

            var pAnamneses = dsAnamnese.getAnamneseByPaciente($stateParams.id);
            pAnamneses.then(function (result) {
                vm.anamneses = result.data;
            });

            var pVacinas = dsVacinas.getVacinasByPaciente($stateParams.id);
            pVacinas.then(function (result) {
                vm.vacinaspaciente = result.data;
            });

            var pAtestados = dsAtestado.getAtestadosByPaciente($stateParams.id);
            pAtestados.then(function (result) {
                vm.atestadosPaciente = result.data;
            });

            var pHistorias = dsProntuario.getHistoriaByIdPaciente($stateParams.id);
            pHistorias.then(function (result) {
                vm.historias = result.data;
            });

            var pMedidas = dsProntuario.getMedidasByPaciente($stateParams.id);
            pMedidas.then(function (result) {
                vm.medidas = result.data;
            });

            var pReceituarios = dsReceituario.getReceituarioByIdPaciente($stateParams.id);
            pReceituarios.then(function (result) {
                vm.receituarios = result.data;
            });

            var pRequisicao = dsProntuario.getRequisicaoByPaciente($stateParams.id);
            pRequisicao.then(function (result) {
                vm.requisicoes = result.data;
            });

            var pUltimos = dsAgenda.getUltimosAgendamentosPaciente($stateParams.id);
            pUltimos.then(function (result) {
                vm.ultimosatendimentos = result.data;
            });

            $q.all([pAnamneses, pVacinas, pAtestados, pHistorias, pMedidas, pReceituarios, pRequisicao, pUltimos]).then(function () {

            })['finally'](function () {
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
        }

        function abrirAnamnese(id) { }

        function anamneseModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/anamnese/crud.anamnese.html',
                controller: 'AnamneseCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pAnamneses = dsAnamnese.getAnamneseByPaciente($stateParams.id);
                pAnamneses.then(function (result) {
                    vm.anamneses = result.data;
                });

                $q.all([pAnamneses]).then(function () {

                })['finally'](function () {
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }

        function ExamesModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/requisicaoexame/crud.requisicaoexame.html',
                controller: 'RequisicaoExame as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pRequisicao = dsProntuario.getRequisicaoByPaciente($stateParams.id);
                pRequisicao.then(function (result) {
                    vm.requisicoes = result.data;
                });
                $q.all([pRequisicao]).then(function () {

                })['finally'](function () {
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }

        function medidasModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/medidasantropometricas/crud.medidasantropometricas.html',
                controller: 'MedidasCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pMedidas = dsProntuario.getMedidasByPaciente($stateParams.id);
                pMedidas.then(function (result) {
                    vm.medidas = result.data;
                });
                $q.all([pMedidas]).then(function () {

                })['finally'](function () {
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }


        function excluirmedidas(id) {

            vm.askOptions = { Title: 'Excluir Medidas Antropométricas', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    var blocker = blockUI.instances.get('blockPaciente');
                    blocker.start();

                    dsProntuario
                      .excluirMedidas(id)
                      .then(function (result) {
                          vm.tabela = result.data;
                          notification.showSuccessBar("Exclusão realizada com sucesso");
                          init();
                      })
                      .catch(function (ex) {
                          notification.showError(ex.data.Message);
                      })['finally'](function () {
                          blocker.stop();
                      });
                }
            });

        }




        function historiaModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/historiapregressa/historiapregressa.crud.html',
                controller: 'CrudHistoria as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pHistorias = dsProntuario.getHistoriaByIdPaciente($stateParams.id);
                pHistorias.then(function (result) {
                    vm.historias = result.data;
                });
                $q.all([pHistorias]).then(function () {

                })['finally'](function () {
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }

        function atestadosModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/atestado/crud.atestado.html',
                controller: 'AtestadoCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pAtestados = dsAtestado.getAtestadosByPaciente($stateParams.id);
                pAtestados.then(function (result) {
                    vm.atestadosPaciente = result.data;
                });
                $q.all([pAtestados]).then(function () {

                })['finally'](function () {
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }

        function vacinasModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/vacinas/crud.registro.vacina.html',
                controller: 'RegistroVacina as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pVacinas = dsVacinas.getVacinasByPaciente($stateParams.id);
                pVacinas.then(function (result) {
                    vm.vacinaspaciente = result.data;
                });
                $q.all([pVacinas]).then(function () {

                })['finally'](function () {
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }

        function receituarioModal(paciente, id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/receituario/crud.receituario.html',
                controller: 'ReceituarioCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    paciente: function () {
                        return paciente;
                    },
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                var pReceituario = dsReceituario.getReceituarioByIdPaciente($stateParams.id);
                pReceituario.then(function (result) {
                    vm.receituarios = result.data;
                });

                $q.all([pReceituario]).then(function () {

                })['finally'](function () {

                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                });
            });
        }
    }
})();