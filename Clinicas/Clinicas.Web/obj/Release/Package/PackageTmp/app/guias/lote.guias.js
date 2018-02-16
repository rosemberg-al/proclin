(function () {
    'use strict';

    angular
        .module('app.guias')
        .controller('LoteGuias', LoteGuias)

    LoteGuias.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'guiaservice', 'cadastroservice', 'id', 'idConvenio'];

    function LoteGuias($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, DTInstances, DTOptionsBuilder, guiaservice, cadastroservice, id, idConvenio) {

        var vm = this;
        $scope.forms = {};
        vm.formValid = true;
        vm.guias = [];
        vm.guiasselecionadas = [];
        vm.adicionados = [];
        vm.escolherguias = false;
        vm.checkAll = false;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancelar = cancelar;
        vm.checkAllItems = checkAllItems;
        vm.selecionar = selecionar;

        //Feature Start
        init();

        function cancelar() {
            $modalInstance.dismiss('cancel');
        }


        //Implementations
        function init() {
            var blocker = blockUI.instances.get('blockModalAddGuias');
            blocker.start();
            guiaservice
                .listarguiaslote(idConvenio, "Consulta")
                .then(function (result) {
                    vm.guias = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex)
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function checkAllItems() {
            if (angular.isDefined(vm.guias)) {
                _.forEach(vm.guias, function (item) {
                    item.Selected = vm.checkAll;
                    if (vm.checkAll) {
                        vm.adicionados.push(item);
                    } else {
                        vm.adicionados = [];
                    }
                });
            }
        }

        function selecionar(item) {
            if (item.Selected) {
                vm.adicionados.push(item);
            } else {
                var index = vm.adicionados.indexOf(item);
                vm.adicionados.splice(index, 1);
            }
        }


        $scope.$watchCollection('vm.adicionados', function (newValue, oldValue) {
            if (vm.adicionados.length > 0)
                vm.selecionados = false;
            else {
                vm.selecionados = true;
                vm.checkAll = false;
            }
            if (vm.adicionados.length == vm.guias.length)
                vm.checkAll = true;
        });

        function save() {

            var guias = [];
            _.forEach(vm.adicionados, function (item) {
                guias.push(item.IdGuia);
            });


            var model = {
                IdLote: id,
                Guias: guias
            };

            var blocker = blockUI.instances.get('blockModalAddGuias');
            blocker.start();
            guiaservice
                .addguiaslote(model)
                .then(function (result) {
                    notification.showSuccessBar("As guias foram adicionadas ao lote nº " + vm.numerolote + " com sucesso!");
                    blocker.stop();
                    $modalInstance.close();
                })
                .catch(function (ex) {
                    blocker.stop();
                    notification.showError(ex.data.Message);
                });

        }
    }
})();