(function () {
    'use strict';

    var appVacina = angular.module('app.vacinas', ['angular-loading-bar', 'app.config']);

    appVacina.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("vacinas", {
                parent: 'app',
                url: appConfig.routePrefix + "/vacinas",
                templateUrl: "app/vacinas/lista.vacinas.html",
                controller: 'ListarVacinas as vm'
            });

    }]);

})();