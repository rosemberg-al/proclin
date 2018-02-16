(function () {
    'use strict';

    var appEspecialidade = angular.module('app.especialidade', ['angular-loading-bar', 'app.config']);

    appEspecialidade.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("especialidades", {
                parent: 'app',
                url: appConfig.routePrefix + "/especialidade",
                templateUrl: "app/especialidade/lista.especialidade.html",
                controller: 'ListaEspecialidades as vm'
            });

    }]);

})();