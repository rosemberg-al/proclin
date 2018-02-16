(function () {
    'use strict';

    angular
        .module('app.procedimento')
        .controller('ProcedimentoCrud', ProcedimentoCrud);

    ProcedimentoCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'id'];

    function ProcedimentoCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice, id) {

        var vm = this;
        vm.State = "Incluir Procedimento";
        vm.FormMessage = "";
        vm.procedimento = {};

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
            combos();
            if (id > 0) {
                vm.State = "Editar Procedimento";
                var blocker = blockUI.instances.get('blockModalCProc');
                blocker.start();
                cadastroservice
                    .getProcedimentoById(id)
                    .then(function (result) {
                        vm.procedimento = result.data;

                        var sexo = _.find(vm.sexos, { Value: vm.procedimento.Sexo });
                        if (sexo != null)
                            vm.sexoSelecionado = sexo.Key;

                        var odonto = _.find(vm.odonto, { Key: vm.procedimento.Odontologico });
                        if (odonto != null)
                            vm.odontoSelecionado = odonto.Key;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function combos() {
            vm.sexos = [{ Key: "A", Value: "Ambos" }, { Key: "F", Value: "Feminino" }, { Key: "M", Value: "Masculino" }];
            vm.odonto = [{ Key: "Sim", Value: "Sim" }, { Key: "Nao", Value: "Não" }];
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.proc.$valid) {
                vm.FormMessage = "";


                if (vm.odontoSelecionado != undefined)
                    vm.procedimento.Odontologico = vm.odontoSelecionado;

                if (vm.sexoSelecionado != undefined)
                    vm.procedimento.Sexo = vm.sexoSelecionado;

                var blocker = blockUI.instances.get('blockModalCProc');
                blocker.start();

                cadastroservice
                    .saveProcedimento(vm.procedimento)
                    .then(function (result) {
                        vm.procedimento = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Procedimento cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Procedimento alterado com sucesso!");

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