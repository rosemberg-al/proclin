(function () {
    'use strict';

    var appHistoria = angular.module('app.historia', ['angular-loading-bar', 'app.config']);

    appHistoria.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("historiapregressa", {
                parent: 'app',
                url: appConfig.routePrefix + "/historiapregressa",
                templateUrl: "app/historiapregressa/list.historiapregressa.html",
                controller: 'ListaHistoria as vm'
            });

    }]);

})();