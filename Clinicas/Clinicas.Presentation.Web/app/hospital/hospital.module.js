(function () {
    'use strict';

    var appHospital = angular.module('app.hospital', ['angular-loading-bar', 'app.config']);

    appHospital.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("hospital", {
                parent: 'app',
                url: appConfig.routePrefix + "/hospital",
                templateUrl: "app/hospital/lista.hospital.html",
                controller: 'ListaHospital as vm'
            }).state("medicos", {
                parent: 'app',
                url: appConfig.routePrefix + "/medicos",
                templateUrl: "app/hospital/lista.medico.html",
                controller: 'ListaMedicos as vm'
            });

    }]);

})();