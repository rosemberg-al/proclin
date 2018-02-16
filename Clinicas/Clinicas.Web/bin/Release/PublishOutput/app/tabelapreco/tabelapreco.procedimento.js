(function () {
    'use strict';

    angular
        .module('app.tabelapreco')
        .controller('TabProcController', TabProcController);

    TabProcController.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'servicebase', 'tabela'];

    function TabProcController($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, comumservice, servicebase, tabela) {

        var vm = this;
        vm.State = "Incluir Procedimentos na tabela " + tabela.Nome;
        vm.FormMessage = "";
        vm.convenio = {};
        vm.novo = false;
        vm.procSelecionado = undefined;
        vm.valor = 0;
        vm.valorProfissional = 0;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.excluirproc = excluirproc;
        vm.getProcedimentos = getProcedimentos;
        vm.add = add;

        //Feature Start
        init();

        //Implementations
        function init() {
            
            vm.FormMessage = "";

            //var pProcs = cadastroservice.getAllProcedimentos();
            //pProcs.then(function (result) {
            //    vm.procedimentos = result.data;
            //});

            var blocker = blockUI.instances.get('blockModalProcTab');
            blocker.start();
            cadastroservice
                .getprocedimentosByTabela(tabela.IdTabelaPreco)
                .then(function (result) {
                    vm.procedimentostabela = result.data;
                })
                .catch(function (ex) {
                    vm.FormMessage = ex.Message;
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function getProcedimentos(search) {

            if (search.length < 3)
                return;

            cadastroservice
                .getProcedimentosPorNomeOuCodigo(search)
                .then(function (result) {
                    vm.procedimentos = result.data;
                })
                .catch(function (ex) {
                    vm.FormMessage = ex.Message;
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function add() {
            $scope.showErrorsCheckValidity = true;
            var formdados = $scope.forms.procs.$valid;
            if (formdados) {
                $scope.forms.procs.proecedimentoadd.$invalid = false;
                vm.FormMessage = "";
                var proc = _.find(vm.procedimentostabela, { IdProcedimento: vm.procSelecionado.IdProcedimento });
                if (proc != null) {
                    notification.showError("Procedimento já foi adicionado");
                }
                else {
                    //var procadd = _.find(vm.procedimentos, { IdProcedimento: vm.procSelecionado });
                    var model = {
                        IdTabelaPreco: tabela.IdTabelaPreco,
                        IdProcedimento: vm.procSelecionado.IdProcedimento,
                        Procedimento: vm.procSelecionado,
                        Valor: vm.valor,
                        ValorProfissional: vm.valorProfissional
                    };
                    vm.procedimentostabela.push(model);
                    vm.procSelecionado = undefined;
                    vm.valor = 0;
                    vm.valorProfissional = 0;
                }
            }
            else {
                $scope.forms.procs.proecedimentoadd.$invalid = true;
                vm.FormMessage = "";
                vm.FormMessage = "Você deve selecionar um procedimento para adicionar";
            }
        }

        function excluirproc(item) {
            _.remove(vm.procedimentostabela, item);
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }



        function save() {

            $scope.showErrorsCheckValidity = true;

            var formdados = $scope.forms.procs.$valid;

            if (vm.procedimentostabela.length == 0) {
                vm.FormMessage = "";
                vm.FormMessage = "Para salvar você tem que adicionar pelo menos um procediemento.";
            }
            else {
                var blocker = blockUI.instances.get('blockModalProcTab');
                blocker.start();

                var model = {
                    IdTabelaPreco: tabela.IdTabelaPreco,
                    IdClinica: tabela.IdClinica,
                    IdConvenio: tabela.IdConvenio,
                    Itens: vm.procedimentostabela
                };


                cadastroservice
                    .saveprocedimentostabela(model)
                    .then(function (result) {
                        vm.procedimentostabela = result.data;
                        notification.showSuccessBar("Procedimentos adicionados com sucesso");
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.data.Message;
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

    }
})();