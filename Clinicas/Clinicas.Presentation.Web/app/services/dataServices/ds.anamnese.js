(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.anamnese', dsAnamnese);

    dsAnamnese.$inject = ['$http', 'common', 'appConfig'];

    function dsAnamnese($http, common, appConfig) {

        var apiRoute = common.makeApiRoute('atendimento');
        var service = {
            save: save,
            getById: getById,
            getAnamneseByPaciente: getAnamneseByPaciente
        };

        return service;

        function save(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveAnamnese']), model);
        }
        function getById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getAnamneseById']), { params: { id: id } });
        }
        function getAnamneseByPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getAnamneseByPaciente']), { params: { id: id } });
        }

    }
})();
