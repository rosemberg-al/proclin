(function () {
    'use strict';

    angular
        .module('app.especialidade')
        .controller('EspecialidadeCrud', EspecialidadeCrud);

    EspecialidadeCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros', 'id'];

    function EspecialidadeCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsCadastros, id) {

        var vm = this;
        vm.State = "Incluir Especialidade";
        vm.FormMessage = "";
        vm.especialidades = {};

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
                vm.State = "Editar Especialidade";
                var blocker = blockUI.instances.get('blockModalCEspec');
                blocker.start();
                dsCadastros
                    .getEspecialidadeById(id)
                    .then(function (result) {
                        vm.especialidade = result.data;
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

            vm.formValid = common.validateForm($scope.forms.especialidades);

            if (vm.formValid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalCEspec');
                blocker.start();

                dsCadastros
                    .saveEspecialidade(vm.especialidade)
                    .then(function (result) {
                        vm.especialidade = result.data;
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
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }

    }
})();