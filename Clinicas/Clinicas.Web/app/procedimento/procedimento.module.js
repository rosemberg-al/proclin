(function () {
    'use strict';

    var appProcedimento = angular.module('app.procedimento', []);

    appProcedimento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("procedimentos", {
                parent: 'app',
                url: "/procedimento",
                templateUrl: "app/procedimento/lista.procedimento.html",
                controller: 'ListaProcedimentos as vm'
            });

    }]);

})();