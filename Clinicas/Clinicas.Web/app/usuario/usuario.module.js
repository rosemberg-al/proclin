(function () {
    'use strict';

    var appUsuario = angular.module('app.usuario', []);

    appUsuario.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("usuario", {
                parent: 'app',
                url: "/usuario",
                templateUrl: "app/usuario/lista.usuario.html",
                controller: 'ListarUsuario as vm'
            })
            .state("alterarsenha", {
                parent: 'app',
                url: "/alterarsenha",
                templateUrl: "app/usuario/alterarsenha.html",
                controller: 'AlterarSenha as vm'
            });

    }]);

})();