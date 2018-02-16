(function () {
    'use strict';

    angular
        .module('app.tabelapreco')
        .controller('TabCrudController', TabCrudController);

    TabCrudController.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'servicebase', 'id'];

    function TabCrudController($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, comumservice, servicebase, id) {

        var vm = this;
        vm.State = "Incluir Tabela de preço";
        vm.FormMessage = "";
        vm.convenio = {};
        vm.novo = false;

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
            vm.tipos = [
                { Key: "P", Value: "Particular" },
                { Key: "C", Value: "Convênio" }
            ];

            var pConvenios = cadastroservice.listarConvenios();
            pConvenios.then(function (result) {
                vm.convenios = result.data;
            });

            vm.FormMessage = "";
            if (id > 0) {
                vm.State = "Editar Tabela de preço";
                var blocker = blockUI.instances.get('blockModalCrudTab');
                blocker.start();
                cadastroservice
                    .getTabelaById(id)
                    .then(function (result) {
                        vm.tabela = result.data;
                        var tipo = _.find(vm.tipos, { Key: vm.tabela.Tipo });
                        if (tipo != null)
                            vm.tabela.Tipo = tipo.Key;

                        var convenio = _.find(vm.convenios, { IdConvenio: vm.tabela.IdConvenio });
                        if (convenio != null)
                            vm.tabela.IdConvenio = convenio.IdConvenio;
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.Message;
                    })['finally'](function () {
                        blocker.stop();
                    });
            }

        }


        function cancel() {
            $modalInstance.dismiss('cancel');
        }


        function save() {

            $scope.showErrorsCheckValidity = true;

            var formdados = $scope.forms.crudtabela.$valid;

            if (!formdados) {
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalCrudTab');
                blocker.start();

                cadastroservice
                    .saveTabela(vm.tabela)
                    .then(function (result) {
                        vm.tabela = result.data;
                        if (id == 0) {
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                            $modalInstance.close();
                        }
                        else {
                            notification.showSuccessBar("Alteração realizada com sucesso");
                            $modalInstance.close();
                        }
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