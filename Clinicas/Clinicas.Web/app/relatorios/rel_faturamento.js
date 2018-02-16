(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelFaturamento',RelFaturamento);

    RelFaturamento.$inject = ['$scope','$modal', 'notification', '$http','$q','DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', 'relatorioservice','cadastroservice','$stateParams'];

    function RelFaturamento($scope,$modal, notification, $http, $q,DTInstances, DTOptionsBuilder, blockUI, common,  relatorioservice,cadastroservice,$stateParams) 
    {
        var vm = this;
        

        $scope.forms = {};
        vm.formValid = true;

        vm.tipo = "";
        if( $stateParams.tipo=="C"){
            vm.tipo = "Convênio";
        }else{
            vm.tipo = "Particular";
        }
        var str = "Faturamento ."+vm.tipo;
        common.setBreadcrumb(str);

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

            vm.situacaoSelecionada ="Realizado";

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

                relatorioservice.printRelFaturamento(vm.dtIn, vm.dtTerm, vm.pSelecionado, vm.pacienteSelecionado, vm.situacaoSelecionada, $stateParams.tipo).then(function (result) {
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