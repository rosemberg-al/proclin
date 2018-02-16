(function () {
    'use strict';

    var appRelatorio = angular.module('app.relatorio', ['blockUI']);

    appRelatorio.config(["$stateProvider",  function ($stateProvider) {

        $stateProvider

            .state("relagendamedica", {
                parent: 'app',
                url:  "/relagendamedica",
                templateUrl: "app/relatorios/rel_agendamedica.html",
                controller: 'RelAgendaMedica as vm'
            })
            .state("relaniversariantes", {
                parent: 'app',
                url:  "/relaniversariantes",
                templateUrl: "app/relatorios/rel_aniversariantes.html",
                controller: 'RelAniversariantes as vm'
            })
            .state("relespecialidades", {
                 parent: 'app',
                 url:  "/relespecialidades",
                 templateUrl: "app/relatorios/rel_especialidades.html",
                 controller: 'RelEspecialidades as vm'
            })
            .state("relocupacoes", {
                 parent: 'app',
                 url:  "/relocupacoes",
                 templateUrl: "app/relatorios/rel_ocupacoes.html",
                 controller: 'RelOcupacoes as vm'
             })
            .state("relprocedimentos", {
                 parent: 'app',
                 url:  "/relprocedimentos",
                 templateUrl: "app/relatorios/rel_procedimentos.html",
                 controller: 'RelProcedimentos as vm'
             })
            .state("relconvenios", {
                 parent: 'app',
                 url:  "/relconvenios",
                 templateUrl: "app/relatorios/rel_convenios.html",
                 controller: 'RelConvenios as vm'
             })
            .state("relpaciente", {
                 parent: 'app',
                 url:  "/relpacientes",
                 templateUrl: "app/relatorios/rel_paciente.html",
                 controller: 'RelPacientes as vm'
             })
            .state("relfornecedores", {
                 parent: 'app',
                 url:  "/relfornecedores",
                 templateUrl: "app/relatorios/rel_fornecedores.html",
                 controller: 'RelFornecedores as vm'
             })
            .state("relcheques", {
                 parent: 'app',
                 url:  "/relcheques",
                 templateUrl: "app/relatorios/rel_cheques.html",
                 controller: 'RelCheques as vm'
             })
            .state("relfinanceiro", {
                 parent: 'app',
                 url:  "/relfinanceiro",
                 templateUrl: "app/relatorios/rel_financeiro.html",
                 controller: 'RelFinanceiro as vm'
             })
            .state("relqtdeprocedimentosrealizados", {
                 parent: 'app',
                 url:  "/relqtdeprocedimentosrealizados",
                 templateUrl: "app/relatorios/rel_procedimentos_realizados.html",
                 controller: 'RelQtdeProcedimentosRealizados as vm'
             })
            .state("relfaturamento", {
                 parent: 'app',
                 url:  "/relfaturamento/:tipo",
                 templateUrl: "app/relatorios/rel_faturamento.html",
                 controller: 'RelFaturamento as vm'
             })
            .state("relPlanoagenda", {
                 parent: 'app',
                 url:  "/relPlanoagenda",
                 templateUrl: "app/relatorios/rel_planoagenda.html",
                 controller: 'RelPlanoAgenda as vm'
             })
    }]);

})();