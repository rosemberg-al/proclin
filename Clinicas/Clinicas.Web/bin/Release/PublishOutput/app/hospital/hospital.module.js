(function () {
    'use strict';

    var appHospital = angular.module('app.hospital', []);

    appHospital.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("hospital", {
                parent: 'app',
                url:  "/hospital",
                templateUrl: "app/hospital/lista.hospital.html",
                controller: 'ListaHospital as vm'
            }).state("medicos", {
                parent: 'app',
                url:  "/medicos",
                templateUrl: "app/hospital/lista.medico.html",
                controller: 'ListaMedicos as vm'
            });

    }]);

})();