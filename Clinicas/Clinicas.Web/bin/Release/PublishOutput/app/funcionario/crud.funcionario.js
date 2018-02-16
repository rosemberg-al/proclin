(function () {
    'use strict';

    angular
        .module('app.funcionario')
        .controller('funcionarioCrud', funcionarioCrud);

    funcionarioCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'id'];

    function funcionarioCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, comumservice, id) {

        var vm = this;
        vm.State = "Incluir Funcionário";
        vm.FormMessage = "";
        vm.funcionario = {
            Especialidades: []
        };
        vm.esepcialidadesadicionadas = [];

        $scope.forms = {};
        vm.formValid = true;
        vm.exibirespec = false;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.excluirespecialidade = excluirespecialidade;
        vm.add = add;
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

            var pEspecialidades = cadastroservice.getAllEspecialidades();
            pEspecialidades.then(function (result) {
                vm.especialidades = result.data;
            });

            combos();

            if (id > 0) {
                vm.State = "Editar Funcionário";
                var blocker = blockUI.instances.get('blockModalCfuncionario');
                blocker.start();
                cadastroservice
                    .getFuncionarioById(id)
                    .then(function (result) {
                        vm.funcionario = result.data;

                        var tipo = _.find(vm.tipos, { Key: result.data.Tipo });
                        vm.tipoSelecionado = tipo.Key;

                        if (result.data.Situacao == "Ativo")
                            vm.SituacaoA = "Ativo";
                        else
                            vm.SituacaoI = "Inativo";

                        vm.tipoFuncionarioSelecionado = vm.funcionario.TipoFuncionario;

                        if (vm.funcionario.TipoFuncionario == 'Profissional de Saúde')
                            vm.exibirespec = true;

                        vm.estadoSelecionado = vm.funcionario.EstadoSelecionado
                        buscarCidadesByEstado(vm.estadoSelecionado, vm.funcionario.CidadeSelecionada);

                        if (vm.funcionario.Sexo != undefined) {
                            vm.sexoSelecionado = vm.funcionario.Sexo;
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
                vm.tipoFuncionarioSelecionado = "Atendente";
                vm.funcionario.DataNascimento = "";
                vm.tipoSelecionado = "PF";
            }
        }

        function excluirespecialidade(item) {
            _.remove(vm.funcionario.Especialidades, item);
        }

        $scope.$watch('vm.tipoFuncionarioSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (newValue == 'Profissional de Saúde')
                    vm.exibirespec = true;
                else
                    vm.exibirespec = false;
            }
        });


        function add(item) {
            var espec = _.find(vm.funcionario.Especialidades, { IdEspecialidade: vm.especSelecionada });
            if (espec != null) {
                notification.showError("Especialidade já foi adicionada!");
            }
            else {
                var especadd = _.find(vm.especialidades, { IdEspecialidade: vm.especSelecionada });
                vm.funcionario.Especialidades.push(especadd);
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
                vm.funcionario.CPF = "";
                vm.funcionario.Nome = "";
                vm.funcionario.DataNascimento = "";
                vm.funcionario.Sexo = "";
            } else {
                if (vm.tipoSelecionado == "PJ") {
                    vm.funcionario.CNPJ = "";
                    vm.funcionario.Nome = "";
                    vm.funcionario.RazaoSocial = "";
                    vm.funcionario.InscricaoEstadual = "";
                    vm.funcionario.NomeFantasia = "";
                }
            }
        }

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockModalCfuncionario');
            blocker.start();
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
            blocker.stop();
        });

        function combos() {
            vm.sexos = [{ Key: "M", Value: "Masculino" }, { Key: "F", Value: "Feminino" }];
            vm.tipos = [{ Key: "PF", Value: "Pessoa Fisica" }, { Key: "PJ", Value: "Pessoa Juridica" }];
            vm.tiposFuncionario = [
                { Key: "Atendente", Value: "Atendente" },
                { Key: "Profissional de Saúde", Value: "Profissional de Saúde" },
                { Key: "Administrativo", Value: "Administrativo" },
                { Key: "Gestor", Value: "Gestor" }
            ];
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function validaAbas(a, b, c) {
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
            var formdados = $scope.forms.dadosfuncionarioes.$valid;
            var formcontato = $scope.forms.dadoscontato.$valid;

            if (!formdados) {
                validaAbas("A", "I", "C");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formEndereco) {
                validaAbas("I", "A", "C");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formcontato) {
                validaAbas("C", "I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
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

                vm.funcionario.CidadeSelecionada = vm.cidadeSelecionada;
                vm.funcionario.EstadoSelecionado = vm.estadoSelecionado;
                vm.funcionario.TipoFuncionario = vm.tipoFuncionarioSelecionado

                if (vm.SituacaoA == 'Ativo')
                    vm.funcionario.Situacao = "Ativo";
                else
                    vm.funcionario.Situacao = "Inativo";

                var blocker = blockUI.instances.get('blockModalCfuncionario');
                blocker.start();

                cadastroservice
                    .saveFuncionario(vm.funcionario)
                    .then(function (result) {
                        vm.funcionario = result.data;
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