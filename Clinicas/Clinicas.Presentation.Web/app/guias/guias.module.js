(function () {
    'use strict';

    var appGuias = angular.module('app.guias', ['angular-loading-bar', 'app.config']);

    appGuias.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("guiasconsulta", {
                parent: 'app',
                url: appConfig.routePrefix + "/guias",
                templateUrl: "app/guias/lista.guia.consulta.html",
                controller: 'ListaGuiasConsultas as vm'
            });

    }]);

})();