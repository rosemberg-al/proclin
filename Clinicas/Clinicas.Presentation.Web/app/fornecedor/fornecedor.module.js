(function () {
    'use strict';

    var appProcedimento = angular.module('app.fornecedor', ['angular-loading-bar', 'app.config']);

    appProcedimento.config(["$stateProvider", "appConfig", function ($stateProvider, appConfig) {

        $stateProvider
            .state("fornecedores", {
                parent: 'app',
                url: appConfig.routePrefix + "/fornecedor",
                templateUrl: "app/fornecedor/lista.fornecedor.html",
                controller: 'ListarFornecedor as vm'
            });

    }]);

})();