(function () {
    'use strict';

    var appAtendimento = angular.module('app.atendimento', ['angular-loading-bar', 'app.config']);

    appAtendimento.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("atendimento", {
                parent: 'app',
                url: appConfig.routePrefix + "/atendimento/:id",
                templateUrl: "app/atendimento/atendimento.html",
                controller: 'Atendimento as vm'
            });
    }]);

})();