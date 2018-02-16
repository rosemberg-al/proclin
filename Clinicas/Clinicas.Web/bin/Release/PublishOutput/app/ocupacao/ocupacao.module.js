(function () {
    'use strict';

    var appOcupacao = angular.module('app.ocupacao', []);

    appOcupacao.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("ocupacoes", {
                parent: 'app',
                url: "/ocupacao",
                templateUrl: "app/ocupacao/lista.ocupacao.html",
                controller: 'ListaOcupacoes as vm'
            });

    }]);

})();