(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('RelAgendaMedica', RelAgendaMedica);

    RelAgendaMedica.$inject = ['$scope', '$http','$q', 'blockUI', 'common', 'notification', 'exception', 'ds.relatorios', 'ds.session', 'ds.funcionario'];

    function RelAgendaMedica($scope, $http,$q, blockUI, common, notification, exception, dsRelatorios, dsSession, dsFuncionario) {
        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatórios .agenda médica');

        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.printRelatorio = printRelatorio;
        var operador = dsSession.getUsuario();


        //Feature Start
        init();

        function init() {
            var blocker = blockUI.instances.get('block');
            blocker.start();

            var pProfissionais = dsFuncionario.listarProfissionaisAtivos();
            pProfissionais.then(function (result) {

                result.data.unshift({ IdFuncionario: 0, NmFuncionario: 'TODOS' })
                vm.profissionalSelecionado = 0;
                vm.profissionais = result.data;

            });

            //var pPacientes = dsPaciente.listar();
            //pPacientes.then(function (result) {
            //    vm.pacientes = result.data;
            //});


            $q.all([pProfissionais]).then(function () {

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
        }

        function printRelatorio(datainicio, datatermino, idprofissional) {
           

            dsRelatorios.printRelAgendaMedica(operador.UserName, vm.agenda.DataInicio, vm.agenda.DataTermino, vm.profissionalSelecionado);
        }


    }
})();