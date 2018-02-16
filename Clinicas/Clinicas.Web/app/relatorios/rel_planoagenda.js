(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelPlanoAgenda', RelPlanoAgenda);

    RelPlanoAgenda.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', 'relatorioservice', 'cadastroservice'];

    function RelPlanoAgenda($scope, $http, $q, blockUI, common, notification, relatorioservice, cadastroservice) {

        var vm = this;
        common.setBreadcrumb('Relatório .Plano de Agenda');

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        vm.pSelecionado = 0;
        vm.pUnidade = 0;

        //Feature Start
        init();

        function init() {

            var blocker = blockUI.instances.get('blockReportPlano');
            blocker.start();
            var pProfissionais = cadastroservice.listarProfissionaisSaude();
            pProfissionais.then(function (result) {
                result.data.unshift({ IdFuncionario: 0, Nome: 'Todos' })
                vm.profissionais = result.data;
            });
            var pUnidades = cadastroservice.listarUnidadesAtendimento();
            pUnidades.then(function (result) {
                vm.unidades = result.data;
            });

            $q.all([pProfissionais, pUnidades]).then(function () {

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                exception.throwEx(ex);

            });

            vm.FormMessage = "";
        }

        function printRelatorio() {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dadosplano.$valid) {

                var dtIn = moment(vm.dataInicio).format("YYYY-MM-DD");
                var dtTerm = moment(vm.dataTermino).format("YYYY-MM-DD");

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockReportPlano');
                blocker.start();

                relatorioservice.printRelPlanoAgenda(dtIn, dtTerm, vm.pSelecionado, vm.pUnidade).then(function (result) {
                })['finally'](function () {
                    blocker.stop();
                });
            }else{
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
        }
    }
})();