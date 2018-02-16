(function () {
    'use strict';

    angular
        .module('app.estoque')
        .controller('CrduMaterial', CrduMaterial);

    CrduMaterial.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification','cadastroservice', 'estoqueservice', 'id'];

    function CrduMaterial($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice,estoqueservice, id) {

        var vm = this;
        vm.State = "Incluir Material";
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

        //Implementations
        function init() {

            estoqueservice
                .listaTipoMateriais()
                .then(function (result) {
                    vm.tipos = result.data;
                }).catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });

            
            if(id>0){
                vm.State = "Editar Material";
                estoqueservice.obterMaterialPorId(id)
                        .then(function (result) {
                            vm.material = result.data;
                            vm.tipoMaterialSelecionado = vm.material.IdTipoMaterial;
                        }).catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
            }else{
                vm.id = id;    
            
            }
               

            
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
                vm.material.IdTipoMaterial = vm.tipoMaterialSelecionado;

                estoqueservice
                    .salvarMaterial(vm.material)
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