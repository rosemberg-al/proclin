(function () {
    'use strict';

    var appProcedimento = angular.module('app.fornecedor', []);

    appProcedimento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("fornecedores", {
                parent: 'app',
                url:"/fornecedor",
                templateUrl: "app/fornecedor/lista.fornecedor.html",
                controller: 'ListarFornecedor as vm'
            });

    }]);

})();