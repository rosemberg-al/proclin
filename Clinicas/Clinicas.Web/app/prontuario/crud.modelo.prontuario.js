(function () {
    'use strict';

    angular
        .module('app.prontuario')
        .controller('ModeloCrud', ModeloCrud);

    ModeloCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'textAngularManager', 'common', 'notification', 'DTInstances', 'prontuarioservice', 'id'];

    function ModeloCrud($scope, $http, $q, $modal, $modalInstance, blockUI, textAngularManager, common, notification, DTInstances, prontuarioservice, id) {

        
        var vm = this;
        vm.State = "Incluir Modelo";
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
            vm.FormMessage = "";

            vm.tiposmodelos = [
                { Key: "Atestado", Value: "Atestado" },
                { Key: "Declaracao", Value: "Declaração" },
                { Key: "Receituario", Value: "Receituário" }
            ];

            if (id > 0) {
                vm.State = "Editar Modelo";
                var blocker = blockUI.instances.get('blockModalModelo');
                blocker.start();
                prontuarioservice
                    .getModeloById(id)
                    .then(function (result) {
                        vm.modelo = result.data;

                        if (result.data.Situacao == 'Ativo')
                            vm.modelo.Situacao = "A";
                        else
                            vm.modelo.Situacao = "I";

                        //seta o modelo
                        var modelo = _.find(vm.tiposmodelos, { Key: vm.modelo.Tipo });
                        vm.modeloSelecionado = modelo.Key;

                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.modelo.Situacao = "A";
            }

        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.modelos.$valid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalModelo');
                blocker.start();

                if (vm.modeloSelecionado != undefined)
                    vm.modelo.Tipo = vm.modeloSelecionado;

                prontuarioservice
                    .saveModelo(vm.modelo)
                    .then(function (result) {
                        vm.modelo = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Modelo cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Modelo alterado com sucesso!");

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