(function () {
    'use strict';

    angular
        .module('app.convenio')
        .controller('ConvenioCrud', ConvenioCrud);

    ConvenioCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros', 'ds.paciente', 'id'];

    function ConvenioCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, dsCadastros, dsPaciente, id) {

        var vm = this;
        vm.State = "Incluir Convênio";
        vm.FormMessage = "";
        vm.convenio = {};

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



            var blocker = blockUI.instances.get('blockModalCrudConvenio');
            blocker.start();

            var pEstados = dsPaciente.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            $q.all([pEstados]).then(function () {

                if (id > 0) {
                    vm.State = "Editar Convênio";
                    dsCadastros
                        .getConvenioById(id)
                        .then(function (result) {
                            vm.convenio = result.data;

                            if (vm.convenio.Estado != null || vm.convenio.Estado != "") {

                                var estado = _.find(vm.estados, { Uf: vm.convenio.Estado });
                                vm.estadoSelecionado = estado.Id;

                                if (estado != null)
                                    buscarCidadesByEstado(estado.Id, vm.convenio.Cidade);
                            }
                           
                            if (result.data.Situacao == "ATIVO")
                                vm.convenio.Situacao = "A";
                            else
                                vm.convenio.Situacao = "I";

                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                        });
                }
                else {
                    vm.convenio.Situacao = "A";
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

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function validaAbas(a, b) {
            var aba1 = angular.element(document.querySelector('#tabdados'));
            a == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#liConvenio'));
            a == "A" ? li1.addClass("active") : li1.removeClass("active");

            var aba2 = angular.element(document.querySelector('#tabendereco'));
            b == "A" ? aba2.addClass("active") : aba2.removeClass("active");
            var li2 = angular.element(document.querySelector('#liEndereco'));
            b == "A" ? li2.addClass("active") : li2.removeClass("active");
        }

        function save() {
            var formEndereco = common.validateForm($scope.forms.dadosendereco);
            var formConvenio = common.validateForm($scope.forms.dadosConvenio);

            if (!formConvenio) {
                validaAbas("A", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
            } else if (!formEndereco) {
                validaAbas("I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
            else{

                vm.FormMessage = "";

                var est = _.find(vm.estados, { Id: vm.estadoSelecionado });
                if (est != undefined)
                    vm.convenio.Estado = est.Uf;

                var cid = _.find(vm.cidades, { Id: vm.cidadeSelecionada });
                if (cid != undefined)
                    vm.convenio.Cidade = cid.Nome;


                var blocker = blockUI.instances.get('blockModalCrudConvenio');
                blocker.start();

                dsCadastros
                    .saveConvenio(vm.convenio)
                    .then(function (result) {
                        vm.convenio = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Convênio cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Convênio alterado com sucesso!");

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