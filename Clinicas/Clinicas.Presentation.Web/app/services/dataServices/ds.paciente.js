(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.paciente', dsPaciente);

    dsPaciente.$inject = ['$http', 'common', 'appConfig'];

    function dsPaciente($http, common, appConfig) {

        var apiRoute = common.makeApiRoute('paciente');
        var service = {
            getById: getById,
            listar: listar,
            salvarPaciente: salvarPaciente,
            getEstados: getEstados,
            getCidadesByEstado: getCidadesByEstado,
            getPacientesPorNome: getPacientesPorNome,
            pesquisar: pesquisar,
            getConveniosByPaciente: getConveniosByPaciente
        };

        return service;

        function getEstados() {
            return $http.get(common.makeUrl([apiRoute, 'getEstados']));
        }

        function getPacientesPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'getPacientesPorNome']), { params: { nome: nome } });
        }

        function getCidadesByEstado(id) {
            return $http.get(common.makeUrl([apiRoute, 'getCidadesByEstado']), { params: { id: id } });
        }

        function getConveniosByPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getConveniosByPaciente']), { params: { id: id } });
        }

        function getById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getById']), { params: { id: id } });
        }

        function salvarPaciente(model) {
            return $http.post(common.makeUrl([apiRoute, 'salvarPaciente']), model);
        }

        function listar() {
            return $http.get(common.makeUrl([apiRoute, 'listar']));
        }

        function pesquisar(model) {
            return $http.post(common.makeUrl([apiRoute, 'pesquisar']), model);
        }

    }
})();
