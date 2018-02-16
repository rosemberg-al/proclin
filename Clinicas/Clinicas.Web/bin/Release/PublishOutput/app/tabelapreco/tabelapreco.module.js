(function () {
    'use strict';

    var appProcedimento = angular.module('app.tabelapreco', ['angularFileUpload']);

    appProcedimento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("tabelapreco", {
                parent: 'app',
                url: "/tabela-preco",
                templateUrl: "app/tabelapreco/lista.tabelapreco.html",
                controller: 'TabelaController as vm'
            });

    }]);

})();