(function () {
    'use strict';

    var appConsultorio = angular.module('app.consultorio', []);

    appConsultorio.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("consultorio", {
                parent: 'app',
                url: "/consultorio",
                templateUrl: "app/consultorio/lista.consultorio.html",
                controller: 'ListaConsultorios as vm'
            });

    }]);

})();