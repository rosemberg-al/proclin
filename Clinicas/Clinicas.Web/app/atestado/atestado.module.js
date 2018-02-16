(function () {
    'use strict';

    var appAtestado = angular.module('app.atestado', []);

    appAtestado.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("atestado", {
                parent: 'app',
                url: "/atestado",
                templateUrl: "app/atestado/list.atestado.html",
                controller: 'ListaAtestado as vm'
            });

    }]);

})();