(function () {
    'use strict';

    var appProcedimento = angular.module('app.convenio', ['angularFileUpload']);

    appProcedimento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("convenio", {
                parent: 'app',
                url: "/convenio",
                templateUrl: "app/convenio/lista.convenio.html",
                controller: 'ListarConvenio as vm'
            });

    }]);

})();