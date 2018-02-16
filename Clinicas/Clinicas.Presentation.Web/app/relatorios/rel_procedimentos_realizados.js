(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelProcedimentos', RelProcedimentos);

    RelProcedimentos.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', 'exception', 'ds.relatorios', 'ds.session', 'ds.funcionario'];

    function RelProcedimentos($scope, $http, $q, blockUI, common, notification, exception, dsRelatorios, dsSession, dsFuncionario) {
        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatórios .procedimentos realizados');

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

            if (vm.DataInicio == undefined && vm.DataTermino == undefined) {
                vm.FormMessage = "Preencha o período para gerar o relatório!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockRelProc');
                blocker.start();

                dsRelatorios.printRelProcedimentosRealizados(operador.UserName, vm.DataInicio, vm.DataTermino)
                .then(function (result) {
                    if (result.status == 202) {
                        vm.FormMessage = "Não foram encontrados registros para o período pesquisado. Tente refazer a busca.";
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