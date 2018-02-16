(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.vacinas', dsVacinas);

    dsVacinas.$inject = ['$http', 'common', 'appConfig'];

    function dsVacinas($http, common, appConfig) {

        var apiRoute = common.makeApiRoute('atendimento');
        var service = {
            saveRegistroVacina: saveRegistroVacina,
            getRegistroVacinaById: getRegistroVacinaById,
            getVacinasAtivas: getVacinasAtivas,
            getVacinasByPaciente: getVacinasByPaciente
        };

        return service;

        function saveRegistroVacina(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveRegistroVacina']), model);
        }
        function getRegistroVacinaById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getRegistroVacinaById']), { params: { id: id } });
        }
        function getVacinasAtivas(id) {
            return $http.get(common.makeUrl([apiRoute, 'getVacinasAtivas']));
        }
        function getVacinasByPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getVacinasByPaciente']), { params: { id: id } });
        }

    }
})();
