(function () {
    'use strict';

    angular
        .module('app.estoque')
        .controller('CrduTipoMaterial', CrduTipoMaterial);

    CrduTipoMaterial.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification','cadastroservice', 'estoqueservice', 'id'];

    function CrduTipoMaterial($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice,estoqueservice, id) {

        var vm = this;
        vm.State = "Incluir Tipo de Material";
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
            
            if(id>0){
                vm.State = "Editar Tipo de Material";
                estoqueservice.obterTipoMaterialPorId(id)
                        .then(function (result) {
                            vm.tipomaterial = result.data;
                            /* if (result.data.Situacao == "Ativo")
                                    vm.SituacaoA = "Ativo";
                                else
                                    vm.SituacaoI = "Inativo"; */

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

                /*
                if (vm.SituacaoA == 'Ativo')
                    vm.tipomaterial.Situacao = "Ativo";
                else
                    vm.tipomaterial.Situacao = "Inativo";*/

                estoqueservice
                    .salvarTipoMaterial(vm.tipomaterial)
                    .then(function (result) {
                        vm.tipomaterial = result.data;

                        /*
                        if (result.data.Situacao == "Ativo")
                                    vm.SituacaoA = "Ativo";
                                else
                                    vm.SituacaoI = "Inativo";
                        */


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