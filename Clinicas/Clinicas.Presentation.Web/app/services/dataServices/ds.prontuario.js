(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.prontuario', dsProntuario);

    dsProntuario.$inject = ['$http', 'common', 'download', 'appConfig'];

    function dsProntuario($http, common, download, appConfig) {

        var apiRoute = common.makeApiRoute('prontuario');
        var service = {
            saveModelo: saveModelo,
            getModeloById: getModeloById,
            getModelos: getModelos,
            getHistoriaById: getHistoriaById,
            saveHistoria: saveHistoria,
            getHistoriaByIdPaciente: getHistoriaByIdPaciente,
            getHospitais: getHospitais,
            getHospitalById: getHospitalById,
            saveHospital: saveHospital,
            saveMedidas: saveMedidas,
            getMedidasById: getMedidasById,
            getMedidasByPaciente: getMedidasByPaciente,
            getVacinas: getVacinas,
            getVacinaById: getVacinaById,
            saveVacina: saveVacina,
            saveRequisicaoExame: saveRequisicaoExame,
            getRequisicaoById: getRequisicaoById,
            getRequisicaoByPaciente: getRequisicaoByPaciente,
            excluirMedidas: excluirMedidas,
            printRequisicao: printRequisicao
        };

        return service;
        //requisição de exames
        function saveRequisicaoExame(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveRequisicaoExame']), model);
        }

        function getRequisicaoById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getRequisicaoById']), { params: { id: id } });
        }

        function getRequisicaoByPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getRequisicaoByPaciente']), { params: { id: id } });
        }

        function printRequisicao(id) {
            download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRequisicao', id])
            });
        }

        //----------------------------------------

        function saveVacina(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveVacina']), model);
        }

        function saveMedidas(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveMedidas']), model);
        }

        function getVacinaById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getVacinaById']), { params: { id: id } });
        }

        function getMedidasById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getMedidasById']), { params: { id: id } });
        }

        function getVacinas() {
            return $http.get(common.makeUrl([apiRoute, 'getVacinas']));
        }

        function getMedidasByPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getMedidasByPaciente']), { params: { id: id } });
        }

        function saveModelo(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveModelo']), model);
        }
        function saveHospital(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveHospital']), model);
        }

        function getModeloById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getModeloById']), { params: { id: id } });
        }

        function getHospitalById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getHospitalById']), { params: { id: id } });
        }

        function getHospitais() {
            return $http.get(common.makeUrl([apiRoute, 'getHospitais']));
        }

        function getModelos() {
            return $http.get(common.makeUrl([apiRoute, 'getModelos']));
        }


        function saveHistoria(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveHistoria']), model);
        }
        function getHistoriaById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getHistoriaById']), { params: { id: id } });
        }

        function getHistoriaByIdPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getHistoriaByIdPaciente']), { params: { id: id } });
        }

        function excluirMedidas(id) {
            return $http.get(common.makeUrl([apiRoute, 'excluirMedidas']), { params: { id: id } });
        }

    }
})();
