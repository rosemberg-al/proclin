(function () {
    'use strict';

    angular.module('app.dataServices')
		.factory('ds.funcionario', dsFuncionario);

    dsFuncionario.$inject = ['$http', 'common', 'appConfig'];
    function dsFuncionario($http, common, appConfig) {

        var apiRoute = common.makeApiRoute('funcionario');

        var service = {
            getFuncionarioById: getFuncionarioById,
            listarProfissionaisAtivos: listarProfissionaisAtivos,
            listarProfissionais: listarProfissionais,
            getMedicoById:getMedicoById,
            listarMedicos: listarMedicos,
            saveMedico: saveMedico,
            saveFuncionario: saveFuncionario,
            listarfuncionarios: listarfuncionarios,
            listarfuncionariosPorNome: listarfuncionariosPorNome
        }
        return service;

        //Implementação das funções
        function getFuncionarioById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getFuncionarioById']), { params: { id: id } });
        }
        function listarProfissionaisAtivos() {
            return $http.get(common.makeUrl([apiRoute, 'listarProfissionaisAtivos']));
        }
        function listarProfissionais() {
            return $http.get(common.makeUrl([apiRoute, 'listarProfissionais']));
        }
        function listarfuncionarios() {
            return $http.get(common.makeUrl([apiRoute, 'listarfuncionarios']));
        }
        function saveFuncionario(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveFuncionario']), model);
        }
        function listarfuncionariosPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'listarfuncionariosPorNome']), { params: { nome: nome } });
        }
        //dados do médico
        function getMedicoById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getMedicoById']), { params: { id: id } });
        }
        function listarMedicos() {
            return $http.get(common.makeUrl([apiRoute, 'listarMedicos']));
        }
        function saveMedico(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveMedico']), model);
        }

    }
})();