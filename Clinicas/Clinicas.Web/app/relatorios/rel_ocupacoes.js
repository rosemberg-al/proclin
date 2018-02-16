(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelOcupacoes', RelOcupacoes);

    RelOcupacoes.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'relatorioservice'];

    function RelOcupacoes($scope, $http, blockUI, common, notification, relatorioservice) {

        var vm = this;
        common.setBreadcrumb('Relatório .Ocupações');

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;

        //Feature Start
        init();

        function init() {
            vm.FormMessage = "";
        }

        function printRelatorio() {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockRelAniversariantes');
                blocker.start();

                relatorioservice.printRelOcupacoes().then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });
            }else{
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }
    }
})();