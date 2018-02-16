(function () {
    'use strict';

    var appHospital = angular.module('app.clinica', ['angularFileUpload']);

    appHospital.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("clinica", {
                parent: 'app',
                url: "/clinica",
                templateUrl: "app/clinica/clinica.html",
                controller: 'ClinicaController as vm'
            })
            .state("unidades", {
                parent: 'app',
                url: "/unidade_atendimento",
                templateUrl: "app/clinica/unidades.atendimento.html",
                controller: 'UnidadeController as vm'
            });
    }]);

})();