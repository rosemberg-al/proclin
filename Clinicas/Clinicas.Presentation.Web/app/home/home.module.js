(function () {
    'use strict';

    var appDashboard = angular.module('app.home', ['angular-loading-bar', 'app.config']);

    appDashboard.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("home", {
                parent: 'app',
                url: appConfig.routePrefix + "/pagina-inicial",
                templateUrl: "app/home/home.html",
                controller: 'Home as vm'
            });
    }]);

})();