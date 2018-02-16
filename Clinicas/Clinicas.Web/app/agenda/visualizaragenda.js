(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('VisualizarAgenda', VisualizarAgenda)

    VisualizarAgenda.$inject = ['$scope', 'notification', '$http', '$modalInstance', '$q', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', '$modal', 'agendaservice', 'id'];

    function VisualizarAgenda($scope, notification, $http, $modalInstance, $q, DTInstances, DTOptionsBuilder, blockUI, common, $modal, agendaservice, id) {

        var vm = this;

        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.confirmaratendimento = confirmaratendimento;
        vm.crudPaciente = crudPaciente;
        vm.cancelar = cancelar;
        vm.encaminharsalaespera = encaminharsalaespera;
        vm.convocarPaciente = convocarPaciente;

        //Feature Start
        init();

        //Implementations
        function init() {

             var blocker = blockUI.instances.get('block1');
            blocker.start();

            agendaservice
                .obteragendaporid(id)
              .then(function (result) {
                  vm.agenda = result.data;

                  if (result.data.Foto.length > 0)
                      vm.agenda.Foto = 'data:image/png;base64,' + result.data.Foto;
              })
                .catch(function (ex) {
                    notification.showError(ex.Message);
              })['finally'](function () {
                  blocker.stop();
              });
        }

        function confirmaratendimento() {
            var blocker = blockUI.instances.get('block1');
            blocker.start();
            agendaservice
                .realizado(vm.agenda.IdAgenda)
                .then(function (result) {
                    notification.showSuccessBar("Atendimento realizado com sucesso");
                    $modalInstance.close();
                })
                .catch(function (ex) {
                    notification.showError(ex.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }


        function convocarPaciente() {
            var blocker = blockUI.instances.get('block1');
            blocker.start();
            agendaservice
                .convocarPaciente(vm.agenda.IdAgenda)
                .then(function (result) {
                    notification.showSuccessBar("Paciente convocado com sucesso ");
                    $modalInstance.close();
                })
                .catch(function (ex) {
                    notification.showError(ex.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function encaminharsalaespera() {
            var blocker = blockUI.instances.get('block1');
            blocker.start();
            agendaservice
                .encaminharsalaespera(vm.agenda.IdAgenda)
                .then(function (result) {
                    notification.showSuccessBar("Paciente encaminhado para sala de espera");
                    $modalInstance.close();
                })
                .catch(function (ex) {
                    notification.showError(ex.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function crudPaciente(id) {
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

        function cancelar() {

            vm.askOptions = { Title: 'Confirmar', Text: 'Tem certeza que deseja cancelar a agenda selecionada ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block1');
                    blocker.start();

                    agendaservice
                        .cancelado(vm.agenda.IdAgenda)
                        .then(function (result) {
                            notification.showSuccessBar("Cancelamento realizado com sucesso");
                            $modalInstance.close();
                        })
                        .catch(function (ex) {
                            notification.showError(ex);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
            });
            
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }
        
    }
})();