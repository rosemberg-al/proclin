(function () {
    'use strict';

    var appAtendimento = angular.module('app.atendimento', []);

    appAtendimento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("atendimento", {
                parent: 'app',
                url: "/atendimento/:id",
                templateUrl: "app/atendimento/atendimento.html",
                controller: 'Atendimento as vm'
            });
    }]);

})();