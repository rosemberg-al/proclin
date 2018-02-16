(function () {
    'use strict';

    var appProntuario = angular.module('app.prontuario', []);

    appProntuario.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("prontuario", {
                parent: 'app',
                url: "/prontuario/listar",
                templateUrl: "app/prontuario/prontuario.html",
                controller: 'Prontuario as vm'
          })
         .state("modeloprontuario", {
             parent: 'app',
             url:  "/modeloprontuario",
             templateUrl: "app/prontuario/lista.modelo.prontuario.html",
             controller: 'ModeloProntuario as vm'
         });
    }]);

})();