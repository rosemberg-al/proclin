(function () {
    'use strict';

    angular
        .module('app.convenio')
        .controller('TabelaConvenio', TabelaConvenio);

    TabelaConvenio.$inject = ['$scope', '$http', '$q', '$timeout', '$modal', '$modalInstance', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros','cadastroservice', 'convenio'];

    function TabelaConvenio($scope, $http, $q, $timeout, $modal, $modalInstance, DTOptionsBuilder, blockUI, common, notification, exception, dsCadastros,cadastroservice, convenio) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;
        vm.tabela = {};
        vm.procedimentoSelecionado = undefined;
        vm.hideLoader = false;

        //Funções
        vm.init = init;
        vm.addProcedimento = addProcedimento;
        vm.excluirItem = excluirItem;
        vm.cancel = cancel;
        //Feature Start
        init();

        //Implementations
        function init() {

            vm.nomeConvenio = convenio.NomeConvenio;
            vm.dtOptions = DTOptionsBuilder
                       .newOptions()
                       .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaTabProc');
            blocker.start();

            var pProc = dsCadastros.getAllProcedimentos();
            pProc.then(function (result) {
                vm.procedimentos = result.data;
            });

            var pTabelas = dsCadastros.getTabelaConvenio(convenio.IdConvenio);
            pTabelas.then(function (result) {
                vm.tabelas = result.data;
            });

            $q.all([pProc, pTabelas]).then(function () {

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function excluirItem(item) {
            vm.askOptions = { Title: 'Excluir Tabela de Preço', Text: 'Deseja mesmo excluir a tabela de preço?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    var blocker = blockUI.instances.get('blockModalListaTabProc');
                    blocker.start();

                    dsCadastros
                        .excluirTabelaConvenio(item.IdConvenio, item.IdProcedimento)
                        .then(function (result) {
                            vm.tabela = result.data;
                            //notification.showSuccessBar("Tabela de preço excluida com sucesso!");
                            vm.AlertClassI = 'fa fa-check';
                            vm.AlertClassDiv = 'alert alert-success';
                            vm.FormMessage = "Tabela de preço excluida com sucesso!";
                            vm.hideLoader = true;

                            $timeout(function () {
                                vm.hideLoader = false;
                            }, 4000);

                            reset();
                            init();
                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                } 
            });
        }



        function reset() {
            vm.tabela = {};
            vm.procedimentoSelecionado = undefined;
        }

        function addProcedimento(id) {
            vm.formValid = common.validateForm($scope.forms.formAddProc);

            if (vm.formValid) {
                vm.FormMessage = "";
                vm.hideLoader = false;
                if (vm.procedimentoSelecionado != undefined)
                    vm.tabela.IdProcedimento = vm.procedimentoSelecionado;

                vm.tabela.IdConvenio = convenio.IdConvenio;

                var blocker = blockUI.instances.get('blockModalListaTabProc');
                blocker.start();

                dsCadastros
                    .saveTabelaConvenio(vm.tabela)
                    .then(function (result) {
                        vm.tabela = result.data;
                        //notification.showSuccessBar("Tabela de preço cadastrada com sucesso!");
                        vm.AlertClassI = 'fa fa-check';
                        vm.AlertClassDiv = 'alert alert-success';
                        vm.FormMessage = "Tabela de preço cadastrada com sucesso!";

                        vm.hideLoader = true;

                        $timeout(function () {
                            vm.hideLoader = false;
                        }, 4000);

                        reset();
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.hideLoader = true;
                vm.AlertClassI = 'fa fa-exclamation-triangle';
                vm.AlertClassDiv = 'alert alert-danger';
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }


    }
})();