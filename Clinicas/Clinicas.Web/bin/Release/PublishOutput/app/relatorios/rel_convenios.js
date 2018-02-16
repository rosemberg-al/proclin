(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelConvenios', RelConvenios);

    RelConvenios.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'relatorioservice'];

    function RelConvenios($scope, $http, blockUI, common, notification, relatorioservice) {

        var vm = this;
        common.setBreadcrumb('Relatório .Convênio');

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

                relatorioservice.printRelConvenios().then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });
            }else{
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }
    }
})();