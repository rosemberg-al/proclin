(function () {
    'use strict';

    angular
        .module('app.clinica')
        .controller('CrudUnidadeController', CrudUnidadeController);

    CrudUnidadeController.$inject = ['$scope', '$http', '$q', 'blockUI', 'servicebase', 'common', 'notification', '$modal', '$modalInstance', 'cadastroservice', 'comumservice', 'id'];

    function CrudUnidadeController($scope, $http, $q, blockUI, servicebase, common, notification, $modal, $modalInstance, cadastroservice, comumservice, id) {

        var vm = this;
        vm.State = "Incluir Undiade de Atendimento";

        vm.unidade = {};

        vm.especialidadesAdicionadas = [];
        vm.unidade.Especialidades = {};

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.excluirespecialidade = excluirespecialidade;
        vm.add = add;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockCrudUnidade');
            blocker.start();

            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            var pEspecialidades = cadastroservice.getAllEspecialidades();
            pEspecialidades.then(function (result) {
                vm.especialidades = result.data;
            });
            blocker.stop();


            if (id > 0) {
                vm.State = "Alterar Unidade de Atendimento";
                var blocker = blockUI.instances.get('blockCrudUnidade');
                blocker.start();
                cadastroservice
                    .getDadosUnidade(id)
                    .then(function (result) {
                        vm.unidade = result.data;
                        vm.estadoSelecionado = vm.unidade.IdEstado;
                        buscarCidadesByEstado(vm.estadoSelecionado, vm.unidade.IdCidade);
                        vm.especialidadesAdicionadas = vm.unidade.Especialidades;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                var blocker = blockUI.instances.get('blockCrudUnidade');
                blocker.start();
                cadastroservice
                    .getDadosClinica()
                    .then(function (result) {
                        vm.unidade = {
                            Clinica: result.data.Nome
                        };
                        vm.especialidadesAdicionadas = result.data.Especialidades;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }

        }

        function cancel() {
           $modalInstance.dismiss('cancel');
        }


        function excluirespecialidade(item) {
            _.remove(vm.especialidadesAdicionadas, item);
        }

        function add(item) {
            var espec = _.find(vm.especialidadesAdicionadas, { IdEspecialidade: vm.especSelecionada });
            if (espec != null) {
                notification.showError("Especialidade já foi adicionada!");
            }
            else {
                var especadd = _.find(vm.especialidades, { IdEspecialidade: vm.especSelecionada });
                vm.especialidadesAdicionadas.push(especadd);
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

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockCrudUnidade');
            blocker.start();
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
            blocker.stop();
        });

        function validaAbas(a, b, c) {

            var aba1 = angular.element(document.querySelector('#tabunidade'));
            a == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#liunidade'));
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

            var formEndereco = $scope.forms.dadosenderecounidade.$valid;
            var formdados = $scope.forms.dadosunidade.$valid;
            var formContato = $scope.forms.dadoscontatounidade.$valid;
            var formContato = $scope.forms.especialidades.$valid;

            if (!formdados) {
                validaAbas("A", "I", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
                return;
            } else if (!formEndereco) {
                validaAbas("I", "A", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
                return;
            }
            else if (!formContato) {
                validaAbas("I", "I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
                return;
            }
            else {
                vm.FormMessage = "";
                if (vm.cidadeSelecionada != null)
                    vm.unidade.IdCidade = vm.cidadeSelecionada;

                if (vm.estadoSelecionado != null)
                    vm.unidade.IdEstado = vm.estadoSelecionado;

                if (vm.especialidadesAdicionadas != null)
                    vm.unidade.Especialidades = vm.especialidadesAdicionadas;


                var blocker = blockUI.instances.get('blockCrudUnidade');
                blocker.start();

                cadastroservice
                    .addUnidade(vm.unidade)
                    .then(function (result) {
                        vm.unidade = result.data;
                        if(id == 0)
                            notification.showSuccessBar("Inclusão realizada com sucesso!");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso!");

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