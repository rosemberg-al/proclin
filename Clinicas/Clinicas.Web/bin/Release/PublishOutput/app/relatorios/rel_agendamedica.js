(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelAgendaMedica', RelAgendaMedica);

    RelAgendaMedica.$inject = ['$scope','$modal', 'notification', '$http','$q','DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', 'relatorioservice', 'cadastroservice'];

    function RelAgendaMedica($scope,$modal, notification, $http, $q,DTInstances, DTOptionsBuilder, blockUI, common,  relatorioservice,  cadastroservice) 
    {

        var vm = this;
        common.setBreadcrumb('Relatório .Agenda');

        $scope.forms = {};
        vm.formValid = true;


        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        vm.buscarPaciente = buscarPaciente;
        vm.limparPaciente = limparPaciente;

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
           
            var pProfissionais = cadastroservice.listarProfissionaisSaude();
            pProfissionais.then(function (result) {

                result.data.unshift({ IdFuncionario: 0, Nome: 'Todos' })
                vm.pSelecionado = 0;
                vm.profissionais = result.data;
            });

            vm.situacaoSelecionada ="Todos";

            $q.all([pProfissionais]).then(function () {
                    
            });
        }

        function printRelatorio() { 
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {

                var blocker = blockUI.instances.get('blockReport');
                blocker.start();
                vm.dtIn =   moment(vm.dataInicio).format("YYYY-MM-DD");
                vm.dtTerm =  moment(vm.dataTermino).format("YYYY-MM-DD");

                relatorioservice.printRelAgendaMedica(vm.dtIn, vm.dtTerm, vm.pSelecionado, vm.pacienteSelecionado, vm.situacaoSelecionada)
                    .then(function (result) {
                        //if (result.status == 202) {
                        //    vm.AlertClassI = 'fa fa-exclamation-triangle';
                        //    vm.AlertClassDiv = 'alert alert-danger';
                        //    vm.AlertMessage = "Não foram encontrados registros para a data pesquisada. Tente refazer a busca.";
                        //} else {
                        //    vm.AlertMessage = "";
                        //}
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }

        function buscarPaciente() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/busca.paciente.html',
                controller: 'BuscaPaciente as vm',
                size: 'lg',
                backdrop: 'static'
            });

            modalInstance.result.then(function (item) {
                vm.pacienteSelecionado = item.IdPaciente;
                vm.pessoaselecionada = item.Nome;
            });
        }

        function limparPaciente() {
            vm.pacienteSelecionado =0;
            vm.pessoaselecionada ="";
        }
    }


})();