(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelAniversariantes', RelAniversariantes);

    RelAniversariantes.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'exception', 'ds.relatorios', 'ds.session'];

    function RelAniversariantes($scope, $http, blockUI, common, notification, exception, dsRelatorios, dsSession) {
        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatórios .aniversariantes');
        vm.mesSelecionado = undefined;

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        var operador = dsSession.getUsuario();

        //Feature Start
        init();

        function init() {
            vm.FormMessage = "";
            vm.meses = [
                { Key: "1", Value: "Janeiro" },
                { Key: "2", Value: "Fevereiro" },
                { Key: "3", Value: "Março" },
                { Key: "4", Value: "Abril" },
                { Key: "5", Value: "Maio" },
                { Key: "6", Value: "Junho" },
                { Key: "7", Value: "Julho" },
                { Key: "8", Value: "Agosto" },
                { Key: "9", Value: "Setembro" },
                { Key: "10", Value: "Outubro" },
                { Key: "11", Value: "Novembro" },
                { Key: "12", Value: "Dezembro" }
            ];
        }

        function printRelatorio() {
            if (vm.mesSelecionado == undefined) {
                vm.FormMessage = "Selecione o mês para gerar o relatório!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockRelAniversariantes');
                blocker.start();

                dsRelatorios.printRelAniversariantes(operador.UserName, vm.mesSelecionado)
                .then(function (result) {
                    if (result.status == 202) {
                        vm.FormMessage = "Não foram encontrados registros para o mês pesquisado. Tente refazer a busca.";
                    } else {
                        vm.FormMessage = "";
                    }
                })['finally'](function () {
                    blocker.stop();
                });
            }
        }


    }
})();