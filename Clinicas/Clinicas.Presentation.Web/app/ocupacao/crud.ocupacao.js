(function () {
    'use strict';

    angular
        .module('app.ocupacao')
        .controller('OcupacaoCrud', OcupacaoCrud);

    OcupacaoCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros', 'id'];

    function OcupacaoCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsCadastros, id) {

        var vm = this;
        vm.State = "Incluir Ocupação";
        vm.FormMessage = "";
        vm.ocupacao = {};

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";

            if (id > 0) {
                vm.State = "Editar Ocupação";
                var blocker = blockUI.instances.get('blockModalCrudOcupacao');
                blocker.start();
                dsCadastros
                    .getOcupacaoById(id)
                    .then(function (result) {
                        vm.ocupacao = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.ocupacoes);

            if (vm.formValid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalCrudOcupacao');
                blocker.start();

                dsCadastros
                    .saveOcupacao(vm.ocupacao)
                    .then(function (result) {
                        vm.ocupacao = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Ocupação cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Ocupação alterada com sucesso!");

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