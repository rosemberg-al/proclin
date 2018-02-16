(function () {
    'use strict';

    angular.module('app.dataServices')
		.factory('ds.financeiro', dsFinanceiro);

    dsFinanceiro.$inject = ['$http', 'common', 'appConfig'];
    function dsFinanceiro($http, common, appConfig) {

    	var apiRoute = common.makeApiRoute('financeiro');

    	var service = {
            getContasApagarReceber: getContasApagarReceber,
            getById: getById,
            salvar: salvar,
            getTransferencias: getTransferencias,
            getTransferenciaById: getTransferenciaById,
            saveTransferencia: saveTransferencia,
            excluirTransferencia: excluirTransferencia,
            alterarParcela: alterarParcela,
            excluirParcela: excluirParcela,
            pesquisar: pesquisar
        }
    	return service;

        //Implementação das funções
    	function getContasApagarReceber(tipo) {
    	    return $http.get(common.makeUrl([apiRoute, 'getContasApagarReceber']), { params: { tipo: tipo } });
    	}

    	function excluirParcela(idFinanceiro, idParcela) {
    	    return $http.get(common.makeUrl([apiRoute, 'excluirParcela']), { params: { idFinanceiro: idFinanceiro, idParcela: idParcela } });
    	}

    	function getTransferencias() {
    	    return $http.get(common.makeUrl([apiRoute, 'getTransferencias']));
    	}

    	function pesquisar(tipo, descricao, tipoConta) {
    	    return $http.get(common.makeUrl([apiRoute, 'pesquisar']), { params: { tipo: tipo, descricao: descricao, tipoConta: tipoConta } });
    	}

    	function getTransferenciaById(id) {
    	    return $http.get(common.makeUrl([apiRoute, 'getTransferenciaById']), { params: { id: id} });
    	}

    	function excluirTransferencia(id) {
    	    return $http.get(common.makeUrl([apiRoute, 'excluirTransferencia']), { params: { id: id } });
    	}


    	function getById(id) {
    	    return $http.get(common.makeUrl([apiRoute, 'getById']), { params: { id: id } });
    	}

    	function saveTransferencia(model) {
    	    return $http.post(common.makeUrl([apiRoute, 'saveTransferencia']), model);
    	}

    	function alterarParcela(model) {
    	    return $http.post(common.makeUrl([apiRoute, 'alterarParcela']), model);
    	}

    	function salvar(model) {
    	    return $http.post(common.makeUrl([apiRoute, 'salvar']), model);
    	}

       

    }
})();