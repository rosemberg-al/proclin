(function () {
    'use strict';

    var appAusencia = angular.module('app.bloqueio', ['angular-loading-bar', 'app.config']);

    appAusencia.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("ausencia", {
                parent: 'app',
                url: appConfig.routePrefix + "/ausencia",
                templateUrl: "app/ausenciamedica/listar.ausencia.html",
                controller: 'AusenciaController as vm'
            });

    }]);

})();