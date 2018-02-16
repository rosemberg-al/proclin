(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelEspecialidades', RelEspecialidades);

    RelEspecialidades.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'exception', 'ds.relatorios', 'ds.session'];

    function RelEspecialidades($scope, $http, blockUI, common, notification, exception, dsRelatorios, dsSession) {
        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatórios .especialidades');

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
            var blocker = blockUI.instances.get('blockRelEspecialidades');
            blocker.start();

            dsRelatorios.printRelEspecialidades(operador.UserName)
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