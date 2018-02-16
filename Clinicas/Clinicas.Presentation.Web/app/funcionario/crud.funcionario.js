(function () {
    'use strict';

    angular
        .module('app.funcionario')
        .controller('FuncionarioCrud', FuncionarioCrud);

    FuncionarioCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.funcionario', 'ds.paciente', 'id'];

    function FuncionarioCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, dsFuncionario, dsPaciente, id) {

        var vm = this;
        vm.State = "Incluir Funcionário";
        vm.FormMessage = "";
        vm.funcionario = {};

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
                    vm.State = "Editar Funcionário";
                    var blocker = blockUI.instances.get('blockModalCFuncionario');
                    blocker.start();
                    dsFuncionario
                        .getFuncionarioById(id)
                        .then(function (result) {
                            vm.funcionario = result.data;
                            

                            if (vm.funcionario.Estado != null || vm.funcionario.Estado != "") {

                                var estado = _.find(vm.estados, { Uf: vm.funcionario.Estado });
                                vm.estadoSelecionado = estado.Id;

                                if (estado != null)
                                    buscarCidadesByEstado(estado.Id, vm.funcionario.Cidade);
                            }

                            if (vm.funcionario.Situacao == "ATIVO")
                                vm.funcionario.Situacao = "A";
                            else
                                vm.funcionario.Situacao = "I";

                            if (vm.funcionario.Sexo != undefined) {
                                var sexo = _.find(vm.sexos, { Value: vm.funcionario.Sexo });
                                vm.sexoSelecionado = sexo.Key;
                            }

                            var tipo = _.find(vm.tipos, { Key: vm.funcionario.Tipo });
                            vm.tipoSelecionado = tipo.Key;


                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
                else {
                    vm.funcionario.Situacao = "A";
                    vm.funcionario.DtNascimento = "";
                }
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
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

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
        });

        function combos() {
            vm.sexos = [{ Key: "Feminino", Value: "Feminino" }, { Key: "Masculino", Value: "Masculino" }];
            vm.tipos = [{ Key: "Atendente", Value: "Atendente" }, { Key: "Profissional de Saude", Value: "Profissional de Saude" }, { Key: "Gestor", Value: "Gestor" }];
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
            var formFuncionario = common.validateForm($scope.forms.dadosfuncionario);

            if (!formFuncionario) {
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
                    vm.funcionario.Sexo = vm.sexoSelecionado;

                var est = _.find(vm.estados, { Id: vm.estadoSelecionado });
                if (est != undefined)
                    vm.funcionario.Estado = est.Uf;

                var cid = _.find(vm.cidades, { Id: vm.cidadeSelecionada });
                if (cid != undefined)
                    vm.funcionario.Cidade = cid.Nome;

                if (vm.tipoSelecionado != undefined)
                    vm.funcionario.Tipo = vm.tipoSelecionado;


                var blocker = blockUI.instances.get('blockModalCFuncionario');
                blocker.start();

                dsFuncionario
                    .saveFuncionario(vm.funcionario)
                    .then(function (result) {
                        vm.funcionario = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Funcionário cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Funcionário alterado com sucesso!");

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