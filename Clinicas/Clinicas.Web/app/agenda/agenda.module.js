(function () {
    'use strict';

    var appAgenda = angular.module('app.agenda', []);

    appAgenda.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("agenda", {
                parent: 'app',
                url: "/agenda/listar",
                templateUrl: "app/agenda/listar.html",
                controller: 'Agenda as vm'
            })
            .state("planoagenda", {
                parent: 'app',
                url: "/agenda/planoagenda",
                templateUrl: "app/agenda/planoagenda.html",
                controller: 'PlanoAgenda as vm'
            })
            .state("salaespera", {
                parent: 'app',
                url: "/agenda/salaespera",
                templateUrl: "app/agenda/salaespera.html",
                controller: 'SalaEspera as vm'
            }).state("agendapaciente", {
                parent: 'app',
                url: "/agenda/agendapaciente",
                templateUrl: "app/agenda/agendapaciente.html",
                controller: 'AgendaPaciente as vm'
            }).state("liberaragenda", {
                parent: 'app',
                url:  "/agenda/liberaragenda",
                templateUrl: "app/agenda/liberaragenda.html",
                controller: 'LiberarAgenda as vm'
            })
             .state("wizard", {
                 parent: 'app',
                 url: "/agenda/wizard",
                 templateUrl: "app/agenda/wizard.html",
                 controller: 'Wizard as vm'
             })

            .state("listaragenda", {
                parent: 'app',
                url:  "/agenda/listaragenda/:tipo",
                templateUrl: "app/agenda/listaragenda.html",
                controller: 'ListarAgenda as vm'
            })
            .state("consultaagenda", {
                parent: 'app',
                url:  "/agenda/consultaagenda",
                templateUrl: "app/agenda/consultaagenda.html",
                controller: 'ConsultaAgenda as vm'
            })
            .state("tvespera", {
               // parent: 'app',
                url:  "/agenda/TvEspera",
                templateUrl: "app/agenda/tvespera.html",
                controller: 'TvEspera as vm'
            });



            /*:id*/
    }]);

})();