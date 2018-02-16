(function () {
    'use strict';

    angular
        .module('app.usuario')
        .controller('PrimeiroAcesso', PrimeiroAcesso);

    PrimeiroAcesso.$inject = ['$scope', '$state', '$http', '$q', '$injector', 'localStorageService', 'blockUI', 'servicebase', 'common', 'notification', '$modal', '$modalInstance', 'segurancaservice', 'comumservice', 'cadastroservice', 'usuario'];

    function PrimeiroAcesso($scope, $state, $http, $q, $injector, localStorageService, blockUI, servicebase, common, notification, $modal, $modalInstance, segurancaservice, comumservice, cadastroservice, usuario) {

        var vm = this;
        $scope.forms = {};
        vm.passo1 = true;
        vm.passo2 = false;
        vm.passo3 = false;
        vm.tipoFuncionarioSelecionado = "Profissional de Saúde";
        vm.tipoSelecionado = "PF";


        //Funções
        vm.init = init;
        vm.save = save;
        vm.sair = sair;
        vm.saveclinica = saveclinica;
        vm.savefuncionario = savefuncionario;
        vm.saveespecialidades = saveespecialidades;
        vm.add = add;
        vm.excluirespecialidade = excluirespecialidade;

        init();

        function init() {
            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            var pEspecialidades = cadastroservice.getAllEspecialidades();
            pEspecialidades.then(function (result) {
                vm.especialidades = result.data;
            });

            combos();
        }
        function excluirespecialidade(item) {
            _.remove(vm.funcionario.Especialidades, item);
        }


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

        function buscarCidadesByEstado(idEstado) {
            comumservice
                .getCidadesByEstado(idEstado)
                .then(function (result) {
                    vm.cidades = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });
        }

        $scope.$watch('vm.clinica.IdEstado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockModalPasso1');
            blocker.start();
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue);
            }
            blocker.stop();
        });

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function saveespecialidades() {
            $scope.showErrorsCheckValidity = true;
            var form = $scope.forms.primeiroacessoespecialidades.$valid;
            if (vm.funcionario.Especialidades.length > 0) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalPasso3');
                blocker.start();

                vm.funcionario.PrimeiroAcesso = false;
                vm.funcionario.FinalizarPrimeiroAcesso = true;

                cadastroservice
                    .saveFuncionario(vm.funcionario)
                    .then(function (result) {
                        notification.showSuccessBar("Proto! Agora você já pode usar o ProClin.");
                        vm.funcionario = result.data;
                        cancel();
                    })
                    .catch(function (ex) {
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Para finalizar você deve adicionar as especialidades!";
            }
        }

        function savefuncionario() {
            $scope.showErrorsCheckValidity = true;
            var form = $scope.forms.primeiroacessofunc.$valid;
            if (form) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalPasso2');
                blocker.start();

                if (vm.sexoSelecionado != undefined)
                    vm.funcionario.Sexo = vm.sexoSelecionado;

                if (vm.tipoSelecionado != undefined)
                    vm.funcionario.Tipo = vm.tipoSelecionado;

                vm.funcionario.TipoFuncionario = vm.tipoFuncionarioSelecionado

                vm.funcionario.Situacao = "Ativo";
                vm.funcionario.PrimeiroAcesso = true;
                vm.funcionario.FinalizarPrimeiroAcesso = false;

                cadastroservice
                    .saveFuncionario(vm.funcionario)
                    .then(function (result) {
                        notification.showSuccessBar("Dados adicionados com sucesso, agora vamos para o 3º passo.!");
                        vm.funcionario = result.data;
                        vm.funcionario.Especialidades = [];
                        vm.passo1 = false;
                        vm.passo2 = false;
                        vm.passo3 = true;
                        var btn2 = angular.element(document.querySelector('#btndadospro'));
                        btn2.removeClass("btn-primary");
                        var btn3 = angular.element(document.querySelector('#btndadosespec'));
                        btn3.addClass("btn-primary");
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.data.Message;


                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Para prosseguir, preencha os campos obrigatórios!";
            }
        }

        function saveclinica() {
            $scope.showErrorsCheckValidity = true;
            var formEndereco = $scope.forms.primeiroenderecoclinica.$valid;
            if (formEndereco) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalPasso1');
                blocker.start();

                vm.clinica.IdClinica = usuario.IdClinica;
                vm.clinica.PrimeiroAcesso = true;

                cadastroservice
                    .saveClinica(vm.clinica)
                    .then(function (result) {
                        vm.clinica = result.data;
                        notification.showSuccessBar("Dados adicionados com sucesso, agora vamos para o 2º passo.!");
                        vm.passo1 = false;
                        vm.passo2 = true;

                        var btn1 = angular.element(document.querySelector('#btndadosclinica'));
                        btn1.removeClass("btn-primary");
                        var btn2 = angular.element(document.querySelector('#btndadospro'));
                        btn2.addClass("btn-primary");
                       
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Para prosseguir, preencha os campos obrigatórios!";
            }
        }

        function sair() {
            localStorageService.clearAll();
            localStorage.removeItem('userName');
            localStorage.removeItem('apptoken');
            var modalInstance = $injector.get('$modalStack');
            modalInstance.dismissAll('close');
            $state.go('login');
        }

        function save() {

            //$scope.showErrorsCheckValidity = true;
            //if ($scope.forms.dadosusuario.$valid) {

            //    var blocker = blockUI.instances.get('blockModalUsuario');
            //    blocker.start();

            //    segurancaservice
            //        .alterarSenha(vm.usuario.NovaSenha,vm.usuario.Confirmar)
            //        .then(function (result) {
            //          notification.showSuccessBar("Alteração realizada com sucesso");
            //          vm.usuario.NovaSenha = "";
            //          vm.usuario.Confirmar = "";
            //        })
            //        .catch(function (ex) {
            //            vm.FormMessage = ex.data.Message;
            //        })['finally'](function () {
            //            blocker.stop();
            //        });

            //}else {
            //    vm.FormMessage = "Existem campos obrigatórios sem devido preenchimento.";
            //}
        }
    }
})();