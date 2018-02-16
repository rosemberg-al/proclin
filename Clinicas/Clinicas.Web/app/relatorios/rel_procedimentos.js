(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelProcedimentos', RelProcedimentos);

    RelProcedimentos.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'relatorioservice'];

    function RelProcedimentos($scope, $http, blockUI, common, notification, relatorioservice) {

        var vm = this;
        common.setBreadcrumb('Relatório .Procedimentos');

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
                var blocker = blockUI.instances.get('blockReport');

                blocker.start();
                relatorioservice.printRelProcedimentos().then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });
            }else{
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }
    }
})();