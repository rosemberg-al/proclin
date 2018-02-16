(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('ConsultaAgenda', ConsultaAgenda)

      ConsultaAgenda.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'agendaservice','cadastroservice', '$stateParams'];
   
    function ConsultaAgenda($scope, $http, $q, $modal, blockUI, common, notification, DTInstances, DTOptionsBuilder, agendaservice, cadastroservice, $stateParams) {
        
        common.setBreadcrumb('Atendimento .Agenda .Pesquisar ');
        var vm = this;
        vm.pesq = {};

        //Funções
        vm.init = init;
        vm.novoagendamento = novoagendamento;
        vm.visualizar = visualizar;
        vm.buscar = buscar;
        $scope.forms = {};

        vm.situacoes = [
            {
                id: 4,
                text: "Todos"
            },{
                id: 1,
                text: "Marcado"
            }, {
                id: 2,
                text: "Realizado"
            }, {
                id: 3,
                text: "Cancelado"
            }];  

        //Feature Start
        init();

        function init() {
            vm.situacaoSelecionada = "Marcado"; // Todos
            cadastroservice
                .listarProfissionaisSaude()
                .then(function (result) {
                    result.data.unshift({ IdFuncionario: 0, Nome: 'Todos' })
                    vm.profissionais = result.data;
                    vm.pSelecionado = 0;
            })
            .catch(function (ex) {
                notification.showError(ex.data.Message);
            })['finally'](function () {
                blocker.stop();
            });
        }

        function buscar() {

         $scope.showErrorsCheckValidity = true;
        if ($scope.forms.dados.$valid) {
        

            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {
                vm.msgalert = "";
                if (vm.pesq.paciente == undefined) {
                    vm.pesq.paciente = "";
                }

                var blocker = blockUI.instances.get('block');
                blocker.start();

                vm.dataInicio =   moment(vm.pesq.dataInicio).format("YYYY-MM-DD");
                vm.dataTermino =  moment(vm.pesq.dataTermino).format("YYYY-MM-DD");

                vm.pesq.situacao = vm.situacaoSelecionada;
                vm.pesq.idprofissional = vm.pSelecionado;
        
                agendaservice
                   .pesquisarAgenda(vm.pesq.idagenda, vm.pesq.idprofissional, vm.pesq.paciente, vm.dataInicio, vm.dataTermino,vm.pesq.situacao) 
                   .then(function (result) {
                       vm.agenda = result.data;
                   })
                    .catch(function (ex) {
                        console.log(ex);
                        vm.msgalert = ex.data.Message;
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }else{
            // vm.msgalert = "Existem campos obrigatórios sem o devido preenchimento";
    
        }
     }

        function novoagendamento() {
            var modalInstance = $modal.open({
                templateUrl: 'app/agenda/novoagendamento.html',
                controller: 'NovoAgendamento as vm',
                size: 'xl',
                backdrop: 'static'
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
                buscar();
            });
        }

    }

})();