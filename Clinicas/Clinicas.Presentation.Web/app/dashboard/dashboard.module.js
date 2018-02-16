(function () {
    'use strict';

    var appDashboard = angular.module('app.dashboard', ['angular-loading-bar', 'app.config']);

    appDashboard.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("dashboard", {
                parent: 'app',
                url: appConfig.routePrefix + "/pagina-inicial",
                templateUrl: "app/dashboard/dashboard.html",
                controller: 'Dashboard as vm'
            });
    }]);

})();