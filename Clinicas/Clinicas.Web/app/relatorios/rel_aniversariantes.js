(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelAniversariantes', RelAniversariantes);

    RelAniversariantes.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'relatorioservice'];

    function RelAniversariantes($scope, $http, blockUI, common, notification, relatorioservice) {
        var vm = this;
        common.setBreadcrumb('Relatório .Aniversariantes');
        vm.mesSelecionado = undefined;

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;

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
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockRelAniversariantes');
                blocker.start();

                relatorioservice.printRelAniversariantes(vm.mesSelecionado).then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });

            }else{
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }



        }


    }
})();