(function () {
    'use strict';

    var appUsers = angular.module('app.users', ['angular-loading-bar', 'app.config']);

    appUsers.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
        .state("users", {
            parent: 'app',
            url: appConfig.routePrefix + "/users",
            views: {
                'content': {
                    templateUrl: "app/users/list.html",
                    controller: "Users as vm"
                }
            },
            data: { requireAuth: true }
        });
    }]);

})();