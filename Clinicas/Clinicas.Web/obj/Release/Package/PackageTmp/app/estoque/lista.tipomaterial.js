(function () {
    'use strict';

    angular
        .module('app.estoque')
        .controller('ListaTipoMaterial', ListaTipoMaterial);

    ListaTipoMaterial.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'DTInstances', 'DTOptionsBuilder', 'common', 'notification', 'cadastroservice','estoqueservice'];

    function ListaTipoMaterial($scope, $http, $q, $modal, blockUI, DTInstances, DTOptionsBuilder, common, notification, cadastroservice,estoqueservice) {

        var vm = this;
        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('Estoque .Tipo de Material');

        //Funções
        vm.init = init;
        vm.addTipoMaterial = addTipoMaterial;
        vm.excluirTipoMaterial = excluirTipoMaterial;


        init();

        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('block');
            blocker.start();

              estoqueservice
                .listaTipoMateriais()
                .then(function (result) {
                    vm.tipomateriais = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function addTipoMaterial(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/estoque/crud.tipomaterial.html',
                controller: 'CrduTipoMaterial as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function excluirTipoMaterial(id) {
            vm.askOptions = { Title: 'Excluir ', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block');
                    blocker.start();
                    estoqueservice.excluirTipoMaterial(id).then(function (result) {
                        notification.showSuccessBar("Exclusão realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }
    }
})();