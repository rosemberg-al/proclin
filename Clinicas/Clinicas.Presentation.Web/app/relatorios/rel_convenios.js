(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelConvenios', RelConvenios);

    RelConvenios.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'exception', 'ds.relatorios', 'ds.cadastros', 'ds.session'];

    function RelConvenios($scope, $http, blockUI, common, notification, exception, dsRelatorios, dsCadastros, dsSession) {
        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatórios .convênios');

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        var operador = dsSession.getUsuario();


        //Feature Start
        init();

        function init() {

            dsCadastros
              .getAllConvenios()
              .then(function (result) {
                  result.data.unshift({ IdConvenio: 0, NomeConvenio: 'Todos' })
                  vm.convenios = result.data;
                  vm.convenioSelecionado = 0;
              })
              .catch(function (ex) {
                  notification.showError(ex.data.Message);
              })['finally'](function () {
              });

        }

        function printRelatorio() {
            vm.FormMessage = "";

            var blocker = blockUI.instances.get('blockRelConvenios');
            blocker.start();

            dsRelatorios.printRelConvenios(operador.UserName, vm.convenioSelecionado)
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