(function () {
    'use strict';

    angular
        .module('app.guias')
        .controller('ListaLotes', ListaLotes);

    ListaLotes.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'guiaservice'];

    function ListaLotes($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, guiaservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('Guia.Lote');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addLote = addLote;
        vm.excluirLote = excluirLote;
        vm.gerarXml = gerarXml;
        vm.addGuias = addGuias;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                .newOptions()
                .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListalotes');
            blocker.start();

            guiaservice
                .listarlotes()
                .then(function (result) {
                    vm.lotes = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function gerarXml(id) {
            guiaservice.xmlLoteConsulta(id);
        }

        function excluirLote(id) {

            vm.askOptions = { Title: 'Cancelar', Text: 'Tem certeza que deseja excluir o lote selecionada?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    guiaservice
                        .excluirlote(id)
                        .then(function (result) {
                            notification.showSuccessBar("Lote excluído com sucesso!");
                            init();
                        })
                        .catch(function (ex) {
                        })['finally'](function () {
                            init();
                        });
                }
            });
        }

       

        function addLote(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/guias/lote.crud.html',
                controller: 'NovoLote as vm',
                size: 'lg',
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

        function addGuias(id, idConvenio) {

            var modalInstance = $modal.open({
                templateUrl: 'app/guias/lote.guias.html',
                controller: 'LoteGuias as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                    idConvenio: function () {
                        return idConvenio;
                    },
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }


        function buscar() {
            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockListaGuiaSpsadt');
                blocker.start();

                if (vm.pesq.Codigo == undefined) {
                    vm.pesq.Codigo = 0;
                }

                guiaservice
                    .pesquisar(vm.pesq.Codigo, vm.pesq.Nome)
                    .then(function (result) {
                        vm.guiasconsultas = result.data;
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