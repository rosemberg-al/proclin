(function () {
    'use strict';

    var appPaciente = angular.module('app.paciente', ['angular-loading-bar', 'app.config']);

    appPaciente.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("pacientes", {
                parent: 'app',
                url: appConfig.routePrefix + "/paciente",
                templateUrl: "app/paciente/lista.paciente.html",
                controller: 'ListaPacientes as vm'
            });

    }]);

})();