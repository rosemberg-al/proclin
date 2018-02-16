(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelQtdeProcedimentosRealizados', RelQtdeProcedimentosRealizados);

    RelQtdeProcedimentosRealizados.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'relatorioservice'];

    function RelQtdeProcedimentosRealizados($scope, $http, blockUI, common, notification, relatorioservice) {

        var vm = this;
        common.setBreadcrumb('Relatório .Quantidade de Atendimento Realizados');


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

                vm.dtIn =   moment(vm.dataInicio).format("YYYY-MM-DD");
                vm.dtTerm =  moment(vm.dataTermino).format("YYYY-MM-DD");

             
                relatorioservice.printRelQtdeProcedimentosRealizados(vm.dtIn, vm.dtTerm).then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });
            }else{
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }
    }
})();