(function () {
    'use strict';

    var appProcedimento = angular.module('app.funcionario', ['angular-loading-bar', 'app.config']);

    appProcedimento.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("funcionarios", {
                parent: 'app',
                url: appConfig.routePrefix + "/funcionario",
                templateUrl: "app/funcionario/lista.funcionario.html",
                controller: 'ListarFuncionario as vm'
            });

    }]);

})();