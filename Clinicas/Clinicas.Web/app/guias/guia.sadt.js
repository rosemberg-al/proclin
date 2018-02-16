(function () {
    'use strict';

    angular
        .module('app.guias')
        .controller('GuiaSadtCrud', GuiaSadtCrud);

    GuiaSadtCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'pacienteservice', 'guiaservice', 'comumservice', 'id'];

    function GuiaSadtCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, pacienteservice, guiaservice, comumservice, id) {

        var vm = this;
        vm.State = "Incluir Guia de Sp-Sadt";
        vm.FormMessage = "";

        $scope.forms = {};
        vm.formValid = true;
        vm.guia = {
            Nome: "",
            CodigoProcedimento: "10101012"
        };
        vm.guia.IdGuia = id;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.getpaciente = getpaciente;
        vm.selecionarConvenio = selecionarConvenio;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";


            vm.tiposaidas = [
                { Key: "1", Value: "Retorno" },
                { Key: "2", Value: "Retorno SADT" },
                { Key: "3", Value: "Referência" },
                { Key: "4", Value: "Internação" },
                { Key: "5", Value: "Alta" }
            ];

            vm.tipodoencas = [
                { Key: "Aguda", Value: "Aguda" },
                { Key: "Cronica", Value: "Crônica" }
            ];

            vm.tempodoencas = [
                { Key: "A", Value: "A - Anos" },
                { Key: "M", Value: "M - Meses" },
                { Key: "D", Value: "D - Dias" }
            ];

            vm.tabelas = [
                { Key: "18", Value: "18 - Terminologia de diárias, taxas e gases medicinais" },
                { Key: "19", Value: "19 - Terminologia de Materiais e Órteses, Próteses e Materiais Especiais" },
                { Key: "20", Value: "20 - Terminologia de medicamentos" },
                { Key: "22", Value: "22 - Terminologia de procedimentos e eventos em saúde" },
                { Key: "90", Value: "90 - Tabela Própria Pacote Odontológico" },
                { Key: "98", Value: "98 - Tabela Própria de Pacotes" },
                { Key: "00", Value: "00 - Tabela Própria das Operadoras" }
            ];

            vm.tiposconsulta = [
                { Key: "1", Value: "1 - Primeira Consulta" },
                { Key: "2", Value: "2 - Retorno" },
                { Key: "3", Value: "3 - Pré-natal" },
                { Key: "4", Value: "4 - Por encaminhamento" }
            ];

            vm.indicacoesacidente = [
                { Key: "N", Value: "Não" },
                { Key: "S", Value: "Sim" }
            ];

            vm.conselhos = [
                { Key: "CRAS", Value: "CRAS" },
                { Key: "COREN", Value: "COREN" },
                { Key: "CRF", Value: "CRF" },
                { Key: "CRFA", Value: "CRFA" },
                { Key: "CREFITO", Value: "CREFITO" },
                { Key: "CRM", Value: "CRM" },
                { Key: "CRN", Value: "CRN" },
                { Key: "CRO", Value: "CRO" },
                { Key: "CRP", Value: "CRP" },
                { Key: "Outros", Value: "Outros" },
            ];

            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });


            var blocker = blockUI.instances.get('blockModalGuiaConsulta');
            blocker.start();

            $q.all([pEstados]).then(function () {

                if (id > 0) {

                    guiaservice
                        .getGuiaById(id)
                        .then(function (result) {
                            vm.guia = result.data;
                            /* vm.pacienteSelecionadoNome = vm.guia.Nome.toUpperCase();
                            listarConveniosByPaciente(vm.guia.IdBeneficiario, true);*/
                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                        });
                }
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save(fechar) {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.crudguia.$valid) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalGuiaConsulta');
                blocker.start();

                guiaservice
                    .saveGuiaConsulta(vm.guia)
                    .then(function (result) {
                        vm.guia = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Guia cadastrada com sucesso!");
                        else
                            notification.showSuccessBar("Guia alterada com sucesso!");

                        if (fechar == "S")
                            $modalInstance.close();
                        else
                            resetform();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
                notification.showError("Existem campos obrigatórios sem o devido preenchimento");
                $('body').animate({ scrollTop: "300px" }, 1000);
            }
        }

        function getpaciente() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/busca.paciente.html',
                controller: 'BuscaPaciente as vm',
                size: 'lg',
                backdrop: 'static'
            });
            modalInstance.result.then(function (item) {

                vm.pacienteSelecionado = item;
                vm.guia.NumeroCartaoSus = item.CartaoSus;
                vm.guia.Nome = item.Nome;
                vm.pacienteSelecionadoNome = item.Nome;
                listarConveniosByPaciente(item.IdPaciente, true);
                vm.guia.IdBeneficiario = item.IdPaciente;

                vm.convenioSelecionado = 0;

                // limpa formulario
                vm.guia.NumeroCarteira = "";
                vm.guia.ValidadeCarteira = "";
                vm.guia.RegistroANS = "";
                vm.guia.Plano = "";
            });
        }

        function selecionarConvenio() {

            var convenio = _.find(vm.convenios, { IdCarteira: vm.convenioSelecionado });
            vm.guia.NumeroCarteira = convenio.NumeroCarteira;
            vm.guia.ValidadeCarteira = convenio.ValidadeCarteira;
            vm.guia.RegistroANS = convenio.RegistroAns;
            vm.guia.Plano = convenio.Plano;
        }

        function listarConveniosByPaciente(id, selecionar) {
            pacienteservice
                .getConveniosByPaciente(id)
                .then(function (result) {
                    vm.convenios = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });
        }

        function resetform() {
            vm.guia = {
                Nome: "",
                CodigoProcedimento: "10101012"
            };
            vm.pacienteSelecionado = undefined;
            vm.pacienteSelecionadoNome = "";
            vm.convenioSelecionado = undefined;
            vm.convenios = [];
        }

    }
})();