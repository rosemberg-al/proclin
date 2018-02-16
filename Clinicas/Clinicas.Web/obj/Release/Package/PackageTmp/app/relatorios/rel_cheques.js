(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelCheques', RelCheques);

    RelCheques.$inject = ['$scope', '$http', 'blockUI','$modal','common', 'notification', 'relatorioservice'];

    function RelCheques($scope, $http, blockUI,$modal, common, notification, relatorioservice) {

        var vm = this;
        common.setBreadcrumb('Relatório .Cheque');

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        vm.buscarPaciente = buscarPaciente;
        vm.limparPaciente = limparPaciente;

        vm.situacoes = [
            {
                id: 1,
                text: "Todos"
            },
            {
                id: 2,
                text: "Emitido"
            },
            {
                id: 3,
                text: "Depositado"
            },
            {
                id: 4,
                text: "Devolvido"
            }];

        
        //Feature Start
        init();

        function init() {
            vm.FormMessage = "";
            vm.situacaoSelecionada ="Todos";
        }

        function printRelatorio() {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockReport');
                blocker.start();
                
                vm.dtIn =   moment(vm.dataInicio).format("YYYY-MM-DD");
                vm.dtTerm =  moment(vm.dataTermino).format("YYYY-MM-DD");


                relatorioservice.printRelCheques(vm.dtIn,vm.dtTerm,vm.situacaoSelecionada).then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });

            }else{
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
                vm.pacienteSelecionado = item.Pessoa.IdPessoa;
                vm.pessoaselecionada = item.Pessoa.Nome;
            });
        }

        function limparPaciente() {
            vm.pacienteSelecionado =0;
            vm.pessoaselecionada ="";
        }




    }
})();