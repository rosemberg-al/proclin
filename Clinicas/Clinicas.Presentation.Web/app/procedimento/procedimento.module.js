(function () {
    'use strict';

    var appProcedimento = angular.module('app.procedimento', ['angular-loading-bar', 'app.config']);

    appProcedimento.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("procedimentos", {
                parent: 'app',
                url: appConfig.routePrefix + "/procedimento",
                templateUrl: "app/procedimento/lista.procedimento.html",
                controller: 'ListaProcedimentos as vm'
            });

    }]);

})();