(function () {
    'use strict';

    angular
        .module('app.fornecedor')
        .controller('FornecedorCrud', FornecedorCrud);

    FornecedorCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros', 'ds.paciente', 'id'];

    function FornecedorCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, dsCadastro, dsPaciente, id) {

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

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";

            var pEstados = dsPaciente.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            combos();

            $q.all([pEstados]).then(function () {
                if (id > 0) {
                    vm.State = "Editar Fornecedor";
                    var blocker = blockUI.instances.get('blockModalCFornecedor');
                    blocker.start();
                    dsCadastro
                        .getFornecedorById(id)
                        .then(function (result) {
                            vm.fornecedor = result.data;
                            console.log(result.data);

                            if (vm.fornecedor.Estado != null || vm.fornecedor.Estado != "") {

                                var estado = _.find(vm.estados, { Uf: vm.fornecedor.Estado });
                                vm.estadoSelecionado = estado.Id;

                                if (estado != null)
                                    buscarCidadesByEstado(estado.Id, vm.fornecedor.Cidade);
                            }

                            if (vm.fornecedor.Situacao == "ATIVO")
                                vm.fornecedor.Situacao = "A";
                            else
                                vm.fornecedor.Situacao = "I";

                            if (vm.fornecedor.Sexo != undefined) {
                                var sexo = _.find(vm.sexos, { Value: vm.fornecedor.Sexo });
                                vm.sexoSelecionado = sexo.Key;
                            }

                            var tipo = _.find(vm.tipos, { Key: vm.fornecedor.Tipo });
                            vm.tipoSelecionado = tipo.Key;


                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
                else {
                    vm.fornecedor.Situacao = "A";
                    vm.fornecedor.DataNascimento = "";
                    vm.tipoSelecionado = "PF";
                }
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                exception.throwEx(ex);
            });
        }

        function buscarCidadesByEstado(idEstado, cidadeSelecionada) {
            dsPaciente
                .getCidadesByEstado(idEstado)
                .then(function (result) {
                    vm.cidades = result.data;
                    if (cidadeSelecionada != "") {
                        var cidade = _.find(vm.cidades, { Nome: cidadeSelecionada });
                        if (cidade != undefined)
                            vm.cidadeSelecionada = cidade.Id;
                    }
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });

        }

        $scope.$watch('vm.tipoSelecionado', function (newValue, oldValue) {
            if (newValue != oldValue) {
                console.log(newValue);
            }
        });

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
        });

        function combos() {
            vm.sexos = [{ Key: "Feminino", Value: "Feminino" }, { Key: "Masculino", Value: "Masculino" }];
            vm.tipos = [{ Key: "PF", Value: "Pessoa Fisica" }, { Key: "PJ", Value: "Pessoa Juridica" }];
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function validaAbas(a, b) {
            var aba1 = angular.element(document.querySelector('#tabdados'));
            a == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#lidados'));
            a == "A" ? li1.addClass("active") : li1.removeClass("active");

            var aba2 = angular.element(document.querySelector('#tabendereco'));
            b == "A" ? aba2.addClass("active") : aba2.removeClass("active");
            var li2 = angular.element(document.querySelector('#liEndereco'));
            b == "A" ? li2.addClass("active") : li2.removeClass("active");
        }

        function save() {

            var formEndereco = common.validateForm($scope.forms.dadosendereco);
            var formdados = common.validateForm($scope.forms.dadosfornecedores);

            if (!formdados) {
                validaAbas("A", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
            } else if (!formEndereco) {
                validaAbas("I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
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


                var blocker = blockUI.instances.get('blockModalCFornecedor');
                blocker.start();

                dsCadastro
                    .saveFornecedor(vm.fornecedor)
                    .then(function (result) {
                        vm.fornecedor = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Fornecedor cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Fornecedor alterado com sucesso!");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

    }
})();