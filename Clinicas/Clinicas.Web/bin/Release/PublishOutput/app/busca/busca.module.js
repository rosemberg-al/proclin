(function () {
    'use strict';

    var appMedicamento = angular.module('app.busca', []);

    appMedicamento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("busca", {
                parent: 'app',
                 url: "/busca/:param",
                templateUrl: "app/busca/lista.busca.html",
                controller: 'Busca as vm'
            });

    }]);

})();