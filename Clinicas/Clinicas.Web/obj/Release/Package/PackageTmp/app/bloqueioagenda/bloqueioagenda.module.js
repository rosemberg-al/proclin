(function () {
    'use strict';

    var appAusencia = angular.module('app.bloqueio', []);

    appAusencia.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("bloqueio", {
                parent: 'app',
                url: "/bloqueioagenda",
                templateUrl: "app/bloqueioagenda/listar.bloqueioagenda.html",
                controller: 'AusenciaController as vm'
            });

    }]);

})();