(function () {
    'use strict';

    var appProcedimento = angular.module('app.funcionario', []);

    appProcedimento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("funcionario", {
                parent: 'app',
                url: "/funcionario",
                templateUrl: "app/funcionario/lista.funcionario.html",
                controller: 'Listarfuncionario as vm'
            });

    }]);

})();