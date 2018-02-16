(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.guia', dsGuia);

    dsGuia.$inject = ['$http', 'download', 'common', 'appConfig'];

    function dsGuia($http, download, common, appConfig) {

        var apiRoute = common.makeApiRoute('guia');
        var service = {
            saveGuiaConsulta: saveGuiaConsulta,
            printconsulta: printconsulta,
            printspsadt: printspsadt,
            getguiasconsultas: getguiasconsultas,
            getguiasbuscaavancada: getguiasbuscaavancada,
            getGuiaById: getGuiaById
        };

        return service;

        function saveGuiaConsulta(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveGuiaConsulta']), model);
        }

        function getguiasbuscaavancada(model) {
            return $http.post(common.makeUrl([apiRoute, 'getguiasbuscaavancada']), model);
        }

        function getGuiaById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getGuiaById']), { params: { id: id } });
        }

        function getguiasconsultas() {
            return $http.get(common.makeUrl([apiRoute, 'getguiasconsultas']));
        }

        function printconsulta(id) {
            download.requestLoad({
                url: common.makeUrl([apiRoute, 'printconsulta', id])
            });
        }

        function printspsadt(id) {
            download.requestLoad({
                url: common.makeUrl([apiRoute, 'printspsadt', id])
            });
        }


    }
})();
