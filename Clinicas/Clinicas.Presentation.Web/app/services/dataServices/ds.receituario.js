(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.receituario', dsReceituario);

    dsReceituario.$inject = ['$http', 'download', 'common', 'appConfig'];

    function dsReceituario($http, download, common, appConfig) {

        var apiRoute = common.makeApiRoute('prontuario');
        var service = {
            saveReceituario: saveReceituario,
            getReceituarioById: getReceituarioById,
            getReceituarioByIdPaciente: getReceituarioByIdPaciente,
            getModelosReceituarios: getModelosReceituarios,
            printReceituario: printReceituario
        };

        return service;

        function saveReceituario(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveReceituario']), model);
        }

        function printReceituario(id) {
            download.requestLoad({
                url: common.makeUrl([apiRoute, 'printReceituario', id])
            });
        }

        function getReceituarioById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getReceituarioById']), { params: { id: id } });
        }
        function getReceituarioByIdPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getReceituarioByIdPaciente']), { params: { id: id } });
        }
        function getModelosReceituarios() {
            return $http.get(common.makeUrl([apiRoute, 'getModelosReceituarios']));
        }


    }
})();
