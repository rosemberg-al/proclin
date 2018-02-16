(function () {
    'use strict';

    var appVacina = angular.module('app.vacinas', []);

    appVacina.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("vacinas", {
                parent: 'app',
                url: "/vacinas",
                templateUrl: "app/vacinas/lista.vacinas.html",
                controller: 'ListarVacinas as vm'
            });

    }]);

})();

