(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.atestado', dsAtestado);

    dsAtestado.$inject = ['$http', 'download', 'common', 'appConfig'];

    function dsAtestado($http, download, common, appConfig) {

        var apiRoute = common.makeApiRoute('prontuario');
        var service = {
            save: save,
            getById: getById,
            getAtestadosByPaciente: getAtestadosByPaciente,
            getModelosAtestados: getModelosAtestados,
            printAtestado: printAtestado
        };

        return service;

        function save(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveAtestado']), model);
        }
        function getById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getAtestadoById']), { params: { id: id } });
        }

        function printAtestado(id) {
            download.requestLoad({
                url: common.makeUrl([apiRoute, 'printAtestado', id])
            });
        }

        function getModelosAtestados() {
            return $http.get(common.makeUrl([apiRoute, 'getModelosAtestados']));
        }

        function getAtestadosByPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getAtestadosByPaciente']), { params: { id: id } });
        }

    }
})();
