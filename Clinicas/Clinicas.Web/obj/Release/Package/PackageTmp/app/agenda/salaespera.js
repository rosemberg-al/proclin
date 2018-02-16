(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('SalaEspera', SalaEspera)

    SalaEspera.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'common', '$timeout', 'notification', 'localStorageService', 'ds.session', 'DTInstances', 'DTOptionsBuilder', 'agendaservice', 'cadastroservice', '$stateParams'];

    function SalaEspera($scope, $http, $q, $modal, blockUI, common, $timeout, notification, localStorageService, dsSession, DTInstances, DTOptionsBuilder, agendaservice, cadastroservice, $stateParams) {

        common.setBreadcrumb('Atendimento .Sala de Espera');
        var vm = this;

        //Funções
        vm.init = init;
        vm.visualizar = visualizar;
        vm.confirmaratendimento = confirmaratendimento;
        vm.cancelar = cancelar;
        vm.addpaciente = addpaciente;
        vm.bloquear = false;
        //Feature Start

        /*vm.dtOptions = DTOptionsBuilder
                       .newOptions()
                       .withOption('order', [[0, 'asc']]);*/

        var usuario = dsSession.getUsuario();

        var blocker = blockUI.instances.get('block');
        blocker.start();
        cadastroservice
            .listarProfissionaisSaude()
            .then(function (result) {
                result.data.unshift({ IdFuncionario: 0, Nome: 'Todos' })
                vm.pSelecionado = 0;
                vm.profissionais = result.data;
                if (usuario != null) {
                    if (usuario.IdFuncionario > 0) {
                        if (usuario.Tipo == "Profissional de Saude") {
                            vm.pSelecionado = usuario.IdFuncionario;
                            vm.bloquear = true;
                        }
                    }
                }
            })
            .catch(function (ex) {
                notification.showError(ex)
            })['finally'](function () {
                blocker.stop();
            });

        init();

        //Implementations
        function init() {

            console.log(usuario);
            var blocker = blockUI.instances.get('block');
            blocker.start();

            if (vm.pSelecionado == undefined) {
                vm.pSelecionado = 0;
            }
            agendaservice
                .salaespera(vm.pSelecionado)
                .then(function (result) {
                    vm.dados = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex);
                })['finally'](function () {
                    blocker.stop();
                });

            $scope.time = $timeout(function () { init(); }, 30000); // 30 segundos
        }

        $scope.$on("$destroy", function () {
            if ($scope.time) {
                $timeout.cancel($scope.time);
            }
        });

        $scope.$watch('vm.pSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                //seta o paciente
                init();
            }
        });

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
            });
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

        function confirmaratendimento(id) {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            agendaservice
                .realizado(id)
                .then(function (result) {
                    notification.showSuccessBar("Atendimento realizado com sucesso");
                    init();
                })
                .catch(function (ex) {
                    notification.showError(ex.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function cancelar(id) {

            vm.askOptions = { Title: 'Confirmar', Text: 'Tem certeza que deseja cancelar a agenda selecionada ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block');
                    blocker.start();

                    agendaservice
                        .cancelado(id)
                        .then(function (result) {
                            notification.showSuccessBar("Cancelamento realizado com sucesso");
                            init();
                        })
                        .catch(function (ex) {
                            notification.showError(ex);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
            });
        }
    }
})();