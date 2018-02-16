(function () {
    'use strict';

    var appAgenda = angular.module('app.agenda', ['angular-loading-bar', 'app.config']);

    appAgenda.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("agenda", {
                parent: 'app',
                url: appConfig.routePrefix + "/agenda/listar",
                templateUrl: "app/agenda/listar.html",
                controller: 'Agenda as vm'
            })
            .state("salaespera", {
                parent: 'app',
                url: appConfig.routePrefix + "/agenda/salaespera",
                templateUrl: "app/agenda/salaespera.html",
                controller: 'SalaEspera as vm'
            }).state("agendapaciente", {
                parent: 'app',
                url: appConfig.routePrefix + "/agenda/agendapaciente",
                templateUrl: "app/agenda/agendapaciente.html",
                controller: 'AgendaPaciente as vm'
            }).state("liberaragenda", {
                parent: 'app',
                url: appConfig.routePrefix + "/agenda/liberaragenda",
                templateUrl: "app/agenda/liberaragenda.html",
                controller: 'LiberarAgenda as vm'
            });
            /*:id*/
    }]);

})();