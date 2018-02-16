(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('CrudPlanoContas', CrudPlanoContas);

    CrudPlanoContas.$inject = ['$scope', '$state', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.financeiro', 'id'];
    function CrudPlanoContas($scope, $state, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsFinanceiro, id) {
        var vm = this;
        vm.titulo = "Incluir plano de Contas";
        vm.FormMessage = "";
        vm.tipo = [];
        vm.planoconta = [];
        $scope.forms = {};
        vm.formValid = true;

        vm.init = init;
        vm.cancel = cancel;
        vm.save = save;

        init();

        function init() {
            vm.tipos = [{ Key: "Despesa", Value: "Despesa" }, { Key: "Receita", Value: "Receita" }, { Key: "Ativo", Value: "Ativo" }, { Key: "Passivo", Value: "Passivo" }, { Key: "Patrimonio Liquido", Value: "Patrimonio Liquido" }];

            if (id > 0) {
                vm.titulo = "Alterar plano de Contas";

                var blocker = blockUI.instances.get('blockModalPlanoContas');
                blocker.start();

                dsFinanceiro
                    .getplanodecontasporid(id)
                    .then(function (result) {
                        vm.planoconta = result.data;
                        var tp = _.find(vm.tipos, { Key: vm.planoconta.Tipo });


                        if (tp != null)
                            vm.tipoSelecionado = tp.Key;

                        if (vm.planoconta.Situacao == "A") {
                            vm.SituacaoA = true;
                            vm.SituacaoI = false;
                        }
                        else {
                            vm.SituacaoA = false;
                            vm.SituacaoI = true;
                        }
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });

            } else {
                vm.SituacaoA = true;
                vm.SituacaoI = false;
                vm.titulo = "Incluir plano de Contas";
            }
        }

        function cancel() {
            $modalInstance.dismiss();
        }

        function save() {
            var formvalido = common.validateForm($scope.forms.planocontas);

            if (formvalido) {

                if (vm.SituacaoA)
                    vm.planoconta.Situacao = "A";
                else
                    vm.planoconta.Situacao = "I";

                if (vm.tipoSelecionado != undefined)
                    vm.planoconta.Tipo = vm.tipoSelecionado;


                var blocker = blockUI.instances.get('blockModalPlanoContas');
                blocker.start();

                dsFinanceiro
                    .salvarplanocontas(vm.planoconta)
                    .then(function (result) {
                        vm.financeiro = result.data;
                        if (id == 0) {
                            notification.showSuccessBar("Plano de contas cadastrado com sucesso");
                        }
                        else {
                            notification.showSuccessBar("Plano de contas alterado com sucesso");
                        }
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });
                
            } else {
                vm.FormMessage = 'Existem campos obrigatórios sem o devido preenchimento';
                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        }

    }
})();