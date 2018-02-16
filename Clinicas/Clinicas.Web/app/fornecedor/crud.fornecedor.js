(function () {
    'use strict';

    angular
        .module('app.fornecedor')
        .controller('FornecedorCrud', FornecedorCrud);

    FornecedorCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'id'];

    function FornecedorCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, comumservice, id) {

        var vm = this;
        vm.State = "Incluir Fornecedor";
        vm.FormMessage = "";
        vm.fornecedor = {};

        $scope.forms = {};
        vm.formValid = true;
        
        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.alterartipo = alterartipo; // Alterar tipo pessoa 

        //Feature Start
        init();

        
        //Implementations
        function init() {
            vm.FormMessage = "";

            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            combos();

            if (id > 0) {
                vm.State = "Editar Fornecedor";
                var blocker = blockUI.instances.get('blockModalCFornecedor');
                blocker.start();
                cadastroservice
                            .getFornecedorById(id)
                            .then(function (result) {
                                vm.fornecedor = result.data;

                                var tipo = _.find(vm.tipos, { Key: result.data.Tipo });
                                vm.tipoSelecionado = tipo.Key;

                                if (result.data.Situacao == "Ativo")
                                    vm.SituacaoA = "Ativo";
                                else
                                    vm.SituacaoI = "Inativo";

                                vm.estadoSelecionado = vm.fornecedor.EstadoSelecionado
                                buscarCidadesByEstado(vm.estadoSelecionado, vm.fornecedor.CidadeSelecionada);

                                if (vm.fornecedor.Sexo != undefined) {
                                    vm.sexoSelecionado = vm.fornecedor.Sexo;
                                }
                            })
                            .catch(function (ex) {
                                vm.FormMessage = ex.Message;
                            })['finally'](function () {
                                blocker.stop();
                            });
            }
            else {
                vm.SituacaoA = "Ativo";
                vm.fornecedor.DataNascimento = "";
                vm.tipoSelecionado = "PJ";
            }
        }

        function buscarCidadesByEstado(idEstado, cidadeSelecionada) {
            comumservice
                .getCidadesByEstado(idEstado)
                .then(function (result) {
                    vm.cidades = result.data;
                    if (cidadeSelecionada != "") {
                        var cidade = _.find(vm.cidades, { Id: cidadeSelecionada });
                        if (cidade != undefined)
                            vm.cidadeSelecionada = cidade.Id;
                    }
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });
        }

        function alterartipo() {
            if (vm.tipoSelecionado == "PF") {
                vm.fornecedor.CPF = "";
                vm.fornecedor.Nome = "";
                vm.fornecedor.DataNascimento = "";
                vm.fornecedor.Sexo = "";
            } else {
                if (vm.tipoSelecionado == "PJ") {
                    vm.fornecedor.CNPJ = "";
                    vm.fornecedor.Nome = "";
                    vm.fornecedor.RazaoSocial = "";
                    vm.fornecedor.InscricaoEstadual = "";
                    vm.fornecedor.NomeFantasia = "";
                }
            }
        }

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockModalCFornecedor');
            blocker.start();
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
            blocker.stop();
        });

        function combos() {
            vm.sexos = [ { Key: "M", Value: "Masculino" }, { Key: "F", Value: "Feminino" }];
            vm.tipos = [{ Key: "PJ", Value: "Pessoa Juridica" }, { Key: "PF", Value: "Pessoa Fisica" }];
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function validaAbas(a, b,c) {
            var aba1 = angular.element(document.querySelector('#tabdados'));
            a == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#lidados'));
            a == "A" ? li1.addClass("active") : li1.removeClass("active");

            var aba2 = angular.element(document.querySelector('#tabendereco'));
            b == "A" ? aba2.addClass("active") : aba2.removeClass("active");
            var li2 = angular.element(document.querySelector('#liEndereco'));
            b == "A" ? li2.addClass("active") : li2.removeClass("active");

            var aba3 = angular.element(document.querySelector('#tabcontato'));
            c == "A" ? aba3.addClass("active") : aba3.removeClass("active");
            var li3 = angular.element(document.querySelector('#liContato'));
            c == "A" ? li3.addClass("active") : li3.removeClass("active");
        }

        function save() {

            $scope.showErrorsCheckValidity = true;

            var formEndereco = $scope.forms.dadosendereco.$valid;
            var formdados = $scope.forms.dadosfornecedores.$valid;
            var formcontato = $scope.forms.dadoscontato.$valid;

            if (!formdados) {
                validaAbas("A", "I","C");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formEndereco) {
                validaAbas("I", "A","C");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formcontato) {
                validaAbas("C", "I","A");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
            else {

                if (vm.sexoSelecionado != undefined)
                    vm.fornecedor.Sexo = vm.sexoSelecionado;

                var est = _.find(vm.estados, { Id: vm.estadoSelecionado });
                if (est != undefined)
                    vm.fornecedor.Estado = est.Uf;

                var cid = _.find(vm.cidades, { Id: vm.cidadeSelecionada });
                if (cid != undefined)
                    vm.fornecedor.Cidade = cid.Nome;

                if (vm.tipoSelecionado != undefined)
                    vm.fornecedor.Tipo = vm.tipoSelecionado;

                vm.fornecedor.CidadeSelecionada = vm.cidadeSelecionada;
                vm.fornecedor.EstadoSelecionado = vm.estadoSelecionado;

                if (vm.SituacaoA == 'Ativo')
                    vm.fornecedor.Situacao = "Ativo";
                else
                    vm.fornecedor.Situacao = "Inativo";

                var blocker = blockUI.instances.get('blockModalCFornecedor');
                blocker.start();

                cadastroservice
                    .saveFornecedor(vm.fornecedor)
                    .then(function (result) {
                        vm.fornecedor = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso");

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