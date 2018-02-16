(function () {
    'use strict';

    var appCalendario = angular.module('app.dashboard', ['angular-loading-bar', 'app.config']);

    appCalendario.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("calendario", {
                parent: 'app',
                url: appConfig.routePrefix + "/calendario",
                templateUrl: "app/calendario/calendario.html",
                controller: 'Calendario as vm'
            });
    }]);

})();