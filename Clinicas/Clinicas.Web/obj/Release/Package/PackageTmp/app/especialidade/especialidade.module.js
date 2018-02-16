(function () {
    'use strict';

    var appEspecialidade = angular.module('app.especialidade', []);

    appEspecialidade.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("especialidades", {
                parent: 'app',
                url: "/especialidade",
                templateUrl: "app/especialidade/lista.especialidade.html",
                controller: 'ListaEspecialidades as vm'
            });

    }]);

})();