(function () {
    'use strict';

    var appConvenio = angular.module('app.convenio', ['angular-loading-bar', 'app.config']);

    appConvenio.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("convenios", {
                parent: 'app',
                url: appConfig.routePrefix + "/convenio",
                templateUrl: "app/convenios/lista.convenio.html",
                controller: 'ListaConvenios as vm'
            });

    }]);

})();