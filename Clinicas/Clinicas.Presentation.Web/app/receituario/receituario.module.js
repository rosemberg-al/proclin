(function () {
    'use strict';

    var appReceituario = angular.module('app.receituario', ['angular-loading-bar', 'app.config']);

    appReceituario.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("receituario", {
                parent: 'app',
                url: appConfig.routePrefix + "/receituario",
                templateUrl: "app/receituario/list.receituario.html",
                controller: 'ListaReceituarios as vm'
            });

    }]);

})();