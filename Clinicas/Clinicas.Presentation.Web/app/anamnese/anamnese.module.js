(function () {
    'use strict';

    var appAnamnese = angular.module('app.anamnese', ['angular-loading-bar', 'app.config']);

    appAnamnese.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("anamnese", {
                parent: 'app',
                url: appConfig.routePrefix + "/anamnese",
                templateUrl: "app/anamnese/list.anamnese.html",
                controller: 'ListaAnamnese as vm'
            });
            
    }]);

})();