(function () {
    'use strict';

    var appGuias = angular.module('app.guias', []);

    appGuias.config(["$stateProvider",  function ($stateProvider) {

        $stateProvider
            .state("guiasconsulta", {
                parent: 'app',
                url:  "/guias",
                templateUrl: "app/guias/lista.guia.consulta.html",
                controller: 'ListaGuiasConsultas as vm'
            })
            .state("guiassadt", {
                parent: 'app',
                url:  "/guias SPSADT",
                templateUrl: "app/guias/lista.guia.sadt.html",
                controller: 'ListaGuiasSadt as vm'
            })
            .state("lotes", {
                parent: 'app',
                url: "/lotes",
                templateUrl: "app/guias/lista.lotes.html",
                controller: 'ListaLotes as vm'
            });

    }]);

})();