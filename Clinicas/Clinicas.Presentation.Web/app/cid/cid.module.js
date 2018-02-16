(function () {
    'use strict';

    var appHospital = angular.module('app.cid', ['angular-loading-bar', 'app.config']);

    appHospital.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("cid", {
                parent: 'app',
                url: appConfig.routePrefix + "/cid",
                templateUrl: "app/cid/lista.cid.html",
                controller: 'ListaCid as vm'
            });

    }]);

})();