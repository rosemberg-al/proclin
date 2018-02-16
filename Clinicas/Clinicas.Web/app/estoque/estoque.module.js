(function () {
    'use strict';

    var appEstoque = angular.module('app.estoque', []);

    appEstoque.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("material", {
                parent: 'app',
                url: "/estoque/materiais",
                templateUrl: "app/estoque/lista.material.html",
                controller: 'ListaMaterial as vm'
            }) 
            .state("tipomaterial", {
                parent: 'app',
                url: "/estoque/tipomaterial",
                templateUrl: "app/estoque/lista.tipomaterial.html",
                controller: 'ListaTipoMaterial as vm'
            }) 
            .state("movimentoestoque", {
                parent: 'app',
                url: "/estoque/movimentoestoque",
                templateUrl: "app/estoque/movimentoestoque.html",
                controller: 'MovimentoEstoque as vm'
            });

    }]);

})();