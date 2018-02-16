(function () {
    'use strict';

    var appProntuario = angular.module('app.prontuario', ['angular-loading-bar', 'app.config']);

    appProntuario.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("modeloprontuario", {
                parent: 'app',
                url: appConfig.routePrefix + "/modeloprontuario",
                templateUrl: "app/prontuario/lista.modelo.prontuario.html",
                controller: 'ModeloProntuario as vm'
            })
             .state("prontuario", {
                 parent: 'app',
                 url: appConfig.routePrefix + "/prontuario/listar",
                 templateUrl: "app/prontuario/prontuario.html",
                 controller: 'Prontuario as vm'
             });

    }]);

})();