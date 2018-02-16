(function () {
    'use strict';

    var appAnamnese = angular.module('app.medidasantropometricas', ['angular-loading-bar', 'app.config']);

    appAnamnese.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("medidasantropometricas", {
                parent: 'app',
                url: appConfig.routePrefix + "/medidasantropometricas",
                templateUrl: "app/medidasantropometricas/lista.medidasantropometricas.html",
                controller: 'ListaMedidas as vm'
            });

    }]);

})();