(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('PrescricaoCrudController', PrescricaoCrudController);

    PrescricaoCrudController.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'pacienteservice', 'medicamentoservice', 'id'];

    function PrescricaoCrudController($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, pacienteservice, medicamentoservice, id) {

        var vm = this;
        vm.init = init;
        vm.add = add;
        vm.getprescricao = getprescricao;
        vm.deletar = deletar;
        vm.cancel = cancel;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        vm.prescricaoSelecionada = undefined;
        vm.unidades = 1;
        vm.adicionados = [];

        init();

        function init() {
            var d = new Date();
            vm.data = moment(d).format("DD/MM/YYYY");

            if (id > 0) {
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function deletar(item) {
            _.remove(vm.adicionados, item);
        }

        function getprescricao(nome) {

            if (nome.length < 3)
                return;

            medicamentoservice
                .pesquisarMedicamentos(nome)
                .then(function (result) {
                    vm.medicamentos = result.data;
                })
                .catch(function (ex) {
                    vm.FormMessage = ex.Message;
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function add() {
            $scope.showErrorsCheckValidity = true;
            var formdados = $scope.forms.medicamentos.$valid;
            if (formdados) {
                $scope.forms.medicamentos.prescricaoSelecionada.$invalid = false;
                vm.FormMessage = "";
                var proc = _.find(vm.adicionados, { IdMedicamento: vm.prescricaoSelecionada.IdMedicamento });
                if (proc != null) {
                    notification.showError("Medicamento já foi adicionado!");
                }
                else {
                    var model = {
                        Medicamento: vm.prescricaoSelecionada,
                        Quantidade: vm.unidades,
                        IdMedicamento: vm.prescricaoSelecionada.IdMedicamento
                    };
                    vm.adicionados.push(model);
                    vm.prescricaoSelecionada = undefined;
                    vm.unidades = 1;
                }
            }
            else {
                $scope.forms.medicamentos.prescricaoSelecionada.$invalid = true;
                vm.FormMessage = "";
                vm.FormMessage = "Você deve selecionar um medicamento para adicionar!";
            }
        }

    }
})();