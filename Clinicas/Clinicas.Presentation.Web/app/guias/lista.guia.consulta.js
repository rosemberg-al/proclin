(function () {
    'use strict';

    angular
        .module('app.guias')
        .controller('ListaGuiasConsultas', ListaGuiasConsultas);

    ListaGuiasConsultas.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.guia'];

    function ListaGuiasConsultas($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsGuia) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .cadastro .guias de consultas');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addGuiaConsulta = addGuiaConsulta;
        vm.print = print;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListaGuiaConsulta');
            blocker.start();

            dsGuia
                .getguiasconsultas()
                .then(function (result) {
                    vm.guiasconsultas = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function print(id){
            dsGuia.printconsulta(id);
        }

        function addGuiaConsulta(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/guias/guia.consulta.html',
                controller: 'GuiaConsultaCrud as vm',
                size: 'xl',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }

        function buscar() {
            if (vm.busca == undefined) {
                vm.FormMessage = "Para realizar a busca você deve preencher pelo menos um campo!";
            }
            else {
                vm.FormMessage = "";
               
                var model = { NomePaciente: vm.busca.NomePaciente, DataInicio: vm.busca.DataInicio, DataFim: vm.busca.DataFim, NumeroGuia: vm.busca.NumeroGuia, Profissional: vm.busca.Profissional };
                var blocker = blockUI.instances.get('blockListaGuiaConsulta');
                blocker.start();
                dsGuia
                   .getguiasbuscaavancada(model)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else {
                           vm.FormMessage = "";
                           vm.guiasconsultas = result.data;
                       }
                       vm.busca = undefined;
                   })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }
    }
})();