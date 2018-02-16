(function () {
    'use strict';

    var appHospital = angular.module('app.cid', []);

    appHospital.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("cid", {
                parent: 'app',
                url:"/cid",
                templateUrl: "app/cid/lista.cid.html",
                controller: 'ListaCid as vm'
            });

    }]);

})();