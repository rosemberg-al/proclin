(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelOcupacoes', RelOcupacoes);

    RelOcupacoes.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'exception', 'ds.relatorios', 'ds.session'];

    function RelOcupacoes($scope, $http, blockUI, common, notification, exception, dsRelatorios, dsSession) {
        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatórios .ocupações');

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        var operador = dsSession.getUsuario();


        //Feature Start
        init();

        function init() {

        }

        function printRelatorio() {
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockRelOcupacoes');
            blocker.start();

            dsRelatorios.printRelOcupacoes(operador.UserName)
            .then(function (result) {
                if (result.status == 202) {
                    vm.FormMessage = "Não foram encontrados registros para este relatório.";
                } else {
                    vm.FormMessage = "";
                }
            })['finally'](function () {
                blocker.stop();
            });
        }


    }
})();