(function () {
    'use strict';

    var appOcupacao = angular.module('app.ocupacao', ['angular-loading-bar', 'app.config']);

    appOcupacao.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("ocupacoes", {
                parent: 'app',
                url: appConfig.routePrefix + "/ocupacao",
                templateUrl: "app/ocupacao/lista.ocupacao.html",
                controller: 'ListaOcupacoes as vm'
            });

    }]);

})();