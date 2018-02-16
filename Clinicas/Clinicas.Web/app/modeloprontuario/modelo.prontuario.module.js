(function () {
    'use strict';

    var appProntuario = angular.module('app.modeloprontuario', []);

    appProntuario.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("modeloprontuario", {
                parent: 'app',
                url: "/modeloprontuario",
                templateUrl: "app/modeloprontuario/lista.modelo.html",
                controller: 'ModeloProntuario as vm'
            });
    }]);

})();