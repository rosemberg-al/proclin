(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelFinanceiro', RelFinanceiro);

    RelFinanceiro.$inject = ['$scope','$modal', 'notification', '$http','$q','DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', 'relatorioservice', 'cadastroservice'];

    function RelFinanceiro($scope,$modal, notification, $http, $q,DTInstances, DTOptionsBuilder, blockUI, common,  relatorioservice,  cadastroservice) 
    {

        var vm = this;
        common.setBreadcrumb('Relatório .Financeiro');

        $scope.forms = {};
        vm.formValid = true;


        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        vm.buscarPaciente = buscarPaciente;
        vm.limparPaciente = limparPaciente;

        vm.situacoes = [
            {
                id: 1,
                text: "Todos"
            },{
                id: 2,
                text: "Aberto"
            }, {
                id: 3,
                text: "Baixado"
            }];

        vm.tipos = [
            {
                id: 1,
                text: "Contas a Pagar"
            },{
                id: 2,
                text: "Contas a Receber"
            }];

        //Feature Start
        init();

        function init() {
            vm.situacaoSelecionada ="Todos";
            vm.pacienteSelecionado = 0;
        }

        function printRelatorio() { 
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {

                var blocker = blockUI.instances.get('blockReport');
                blocker.start();
                vm.dtIn =   moment(vm.dataInicio).format("YYYY-MM-DD");
                vm.dtTerm =  moment(vm.dataTermino).format("YYYY-MM-DD");
                
                relatorioservice.printRelFinanceiro(vm.dtIn, vm.dtTerm, vm.tipoSelecionado, vm.situacaoSelecionada, vm.pacienteSelecionado)
                    .then(function (result) {
                    })['finally'](function () {
                        blocker.stop();
                    });

            }else {
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