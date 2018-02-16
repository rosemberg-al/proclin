(function () {
    'use strict';

    var appAtestado = angular.module('app.atestado', ['angular-loading-bar', 'app.config']);

    appAtestado.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("atestado", {
                parent: 'app',
                url: appConfig.routePrefix + "/atestado",
                templateUrl: "app/atestado/list.atestado.html",
                controller: 'ListaAtestado as vm'
            });

    }]);

})();