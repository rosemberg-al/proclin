(function () {
    'use strict';

    var appMedicamento = angular.module('app.medicamento', []);

    appMedicamento.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("medicamento", {
                parent: 'app',
                url: "/consulta/medicamento",
                templateUrl: "app/medicamento/lista.medicamento.html",
                controller: 'ListarMedicamento as vm'
            });

    }]);

})();