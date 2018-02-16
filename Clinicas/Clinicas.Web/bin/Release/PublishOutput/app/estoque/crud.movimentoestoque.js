(function () {
    'use strict';

    angular
        .module('app.estoque')
        .controller('CrudMovimentoEstoque', CrudMovimentoEstoque);

    CrudMovimentoEstoque.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification','cadastroservice', 'estoqueservice', 'id'];

    function CrudMovimentoEstoque($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice,estoqueservice, id) {

        var vm = this;
        vm.State = "Incluir Movimento Estoque";
        vm.FormMessage = "";

        $scope.forms = {};
        vm.formValid = true;
        vm.FormMessage = "";

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        vm.tipos = [{ Key: "Entrada", Value: "Entrada" }, { Key: "Saida", Value: "Saida" }];


        //Implementations
        function init() {



            estoqueservice
                .listaMateriais()
                .then(function (result) {
                    vm.materiais = result.data;
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() 
        {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.material.$valid) {

                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalMaterial');
                blocker.start();

                vm.material.IdMaterial = vm.materialSelecionado;
                vm.material.Tipo = vm.tipoSelecionado;

                estoqueservice
                    .salvarMovimentoEstoque(vm.material)
                    .then(function (result) {
                        vm.material = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }
    }
})();