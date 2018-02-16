(function () {
    'use strict';

    var appRelatorio = angular.module('app.relatorio', ['angular-loading-bar', 'app.config']);

    appRelatorio.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("relpaciente", {
                parent: 'app',
                url: appConfig.routePrefix + "/relpaciente",
                templateUrl: "app/relatorios/rel_paciente.html",
                controller: 'Relatorio as vm'
            })
             .state("relespecialidades", {
                 parent: 'app',
                 url: appConfig.routePrefix + "/relespecialidades",
                 templateUrl: "app/relatorios/rel_especialidades.html",
                 controller: 'RelEspecialidades as vm'
             })
             .state("relfornecedores", {
                 parent: 'app',
                 url: appConfig.routePrefix + "/relfornecedores",
                 templateUrl: "app/relatorios/rel_fornecedores.html",
                 controller: 'RelFornecedores as vm'
             })
             .state("relocupacoes", {
                 parent: 'app',
                 url: appConfig.routePrefix + "/relocupacoes",
                 templateUrl: "app/relatorios/rel_ocupacoes.html",
                 controller: 'RelOcupacoes as vm'
             })
              .state("relaniversariantes", {
                  parent: 'app',
                  url: appConfig.routePrefix + "/relaniversariantes",
                  templateUrl: "app/relatorios/rel_aniversariantes.html",
                  controller: 'RelAniversariantes as vm'
              })
            .state("relconvenios", {
                parent: 'app',
                url: appConfig.routePrefix + "/relconvenios",
                templateUrl: "app/relatorios/rel_convenios.html",
                controller: 'RelConvenios as vm'
            }).state("relagendamedica", {
                parent: 'app',
                url: appConfig.routePrefix + "/relagendamedica",
                templateUrl: "app/relatorios/rel_agendamedica.html",
                controller: 'RelAgendaMedica as vm'
            })
            .state("relprocedimentos", {
                parent: 'app',
                url: appConfig.routePrefix + "/relprocedimentos",
                templateUrl: "app/relatorios/rel_procedimentos_realizados.html",
                controller: 'RelProcedimentos as vm'
            });

        
        
    }]);

})();