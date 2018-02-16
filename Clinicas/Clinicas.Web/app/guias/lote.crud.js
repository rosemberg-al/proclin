(function () {
    'use strict';

    angular
        .module('app.guias')
        .controller('NovoLote', NovoLote)

    NovoLote.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'DTInstances', 'DTOptionsBuilder', 'guiaservice', 'cadastroservice', 'pacienteservice', '$stateParams'];

    function NovoLote($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, DTInstances, DTOptionsBuilder, guiaservice, cadastroservice, pacienteservice, $stateParams) {

        var vm = this;
        vm.convenioselecionado = undefined;

        //funções paciente

        $scope.forms = {};
        vm.formValid = true;
        vm.tiposelecionado = "Consulta";

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancelar = cancelar;

        //Feature Start
        init();

        function cancelar() {
            $modalInstance.dismiss('cancel');
        }


        //Implementations
        function init() {
            //vm.tipoguias = [
            //    { Key: "Consulta", Value: "Consulta" },
            //    { Key: "SPSADT", Value: "SPSADT" }
            //];
            var pConvenios = cadastroservice.listarConvenios();
            pConvenios.then(function (result) {
                vm.convenios = result.data;
            });

            vm.tipos = [
                { Key: "Consulta", Value: "Consulta" },
                { Key: "Procedimentos", Value: "Procedimentos" },
            ];
            vm.tiposelecionado = "Consulta";
        }

        function save() {
            $scope.showErrorsCheckValidity = true;

            var formLote = $scope.forms.lote.$valid;
            if (formLote) {
                vm.FormMessage = "";

                var model = {
                    Situacao: "Enviado",
                    IdConvenio: vm.convenioselecionado
                };
                var blocker = blockUI.instances.get('blockModalloteGuias');
                blocker.start();
                guiaservice
                    .salvarlote(model)
                    .then(function (result) {
                        notification.showSuccessBar("O lote nº " + result.data.IdLote + " foi gerado com sucesso!");
                        vm.numerolote = result.data.IdLote;
                        vm.askOptions = { Title: 'Adicionar Guias', Text: 'Deseja adicionar guias ao lote ' + result.data.IdLote + '?', Yes: 'Sim', No: 'Não' };
                        notification.ask(vm.askOptions, function (confirm) {
                            if (confirm) {
                                var modalInstance = $modal.open({
                                    templateUrl: 'app/guias/lote.guias.html',
                                    controller: 'LoteGuias as vm',
                                    size: 'lg',
                                    backdrop: 'static',
                                    resolve: {
                                        id: function () {
                                            return result.data.IdLote;
                                        },
                                        idConvenio: function () {
                                            return vm.convenioselecionado;
                                        },
                                    }
                                });
                                modalInstance.result.then(function () {
                                    $modalInstance.close();
                                });
                            }
                            else {
                                console.log('confirm');
                                console.log(confirm);
                                $modalInstance.close();
                            }
                        });
                    })
                    .catch(function (ex) {
                        blocker.stop();
                        notification.showError(ex.data.Message);
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }
    }
})();