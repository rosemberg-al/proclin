(function () {
    'use strict';

    angular
        .module('app.estoque')
        .controller('ListaMaterial', ListaMaterial);

    ListaMaterial.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'DTInstances', 'DTOptionsBuilder', 'common', 'notification', 'cadastroservice','estoqueservice'];

    function ListaMaterial($scope, $http, $q, $modal, blockUI, DTInstances, DTOptionsBuilder, common, notification, cadastroservice,estoqueservice) {

        var vm = this;
        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('Estoque .Material');

        //Funções
        vm.init = init;
        vm.addMaterial = addMaterial;
        vm.excluirMaterial = excluirMaterial;


        init();

        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('block');
            blocker.start();

              estoqueservice
                .listaMateriais()
                .then(function (result) {
                    vm.materiais = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function addMaterial(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/estoque/crud.material.html',
                controller: 'CrduMaterial as vm',
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

        function excluirMaterial(id) {
            vm.askOptions = { Title: 'Excluir ', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block');
                    blocker.start();
                    estoqueservice.excluirMaterial(id).then(function (result) {
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