(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('AddModeloController', AddModeloController);

    AddModeloController.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'textAngularManager', 'notification', 'prontuarioservice', 'descricao'];

    function AddModeloController($scope, $http, $q, $modal, $modalInstance, blockUI, textAngularManager, notification, prontuarioservice, descricao) {

        var vm = this;
        vm.FormMessage = "";
        vm.modelo = {};

        $scope.forms = {};
        vm.formValid = true;

        $scope.version = textAngularManager.getVersion();
        $scope.versionNumber = $scope.version.substring(1);
        $scope.orightml = "";
        $scope.htmlcontent = $scope.orightml;
        $scope.disabled = false;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.modelo.Descricao = descricao;
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {
            $scope.showErrorsCheckValidity = true;
            var formdados = $scope.forms.addmodelos.$valid;
            if (formdados) {
               
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalAddModelo');
                blocker.start();

                vm.modelo.Tipo = "Receituario";
                vm.modelo.Situacao = "Ativo";

                prontuarioservice
                    .saveModelo(vm.modelo)
                    .then(function (result) {
                        vm.modelo = result.data;
                        notification.showSuccessBar("Modelo cadastrado com sucesso!");
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }

    }
})();