(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('guiaservice', guiaservice); //Define o nome a função do seu .service

    guiaservice.$inject = ['$http', 'download', '$window', 'servicebase']; //Lista de dependências
    function guiaservice($http, download, $window, servicebase) {

        var service = this;

        var service = {
            saveGuiaConsulta: saveGuiaConsulta,
            printconsulta: printconsulta,
            printspsadt: printspsadt,
            getguiasspsadt: getguiasspsadt,
            getguiasconsultas: getguiasconsultas,
            getguiasbuscaavancada: getguiasbuscaavancada,
            getGuiaById: getGuiaById,
            pesquisar: pesquisar,
            cancelar: cancelar,
            //lotes
            addguiaslote: addguiaslote,
            salvarlote: salvarlote,
            listarlotesporconvenio: listarlotesporconvenio,
            obterloteporid: obterloteporid,
            listarlotes: listarlotes,
            excluirlote: excluirlote,
            gerarlote: gerarlote,
            listarguiaslote: listarguiaslote,
            xmlLoteConsulta: xmlLoteConsulta
        }
        return service;


        //Implementação das funções
        //***********************************   guias     ***************************************************

        function getGuiaById(id) {
            return $http.get(servicebase.urlApi() + "/guia/getGuiaById?id=" + id);
        }
        function getguiasconsultas() {
            return $http.get(servicebase.urlApi() + "/guia/getguiasconsultas");
        }

        function getguiasspsadt() {
            return $http.get(servicebase.urlApi() + "/guia/getguiasspsadt");
        }

        function saveGuiaConsulta(model) {
            return $http.post(servicebase.urlApi() + "/guia/saveGuiaConsulta", model);
        }

        function getguiasbuscaavancada(model) {
            return $http.post(servicebase.urlApi() + "/guia/getguiasbuscaavancada", model);
        }

        function pesquisar(idguia,nome) {
            return $http.get(servicebase.urlApi() + "/guia/pesquisar?idguia="+idguia+"&nome="+nome);
        }

        function cancelar(idguia) {
            return $http.get(servicebase.urlApi() + "/guia/cancelar?idguia="+idguia);
        }


        function printconsulta(id) {
            download.requestLoad({
                url: servicebase.urlApi() + "/guia/printconsulta?idGuia=" + id
            });
        }

        function printspsadt(id) {
            download.requestLoad({
                url: servicebase.urlApi() + "/guia/printspsadt?id=" + id
            });
        }

        //************************************   Lotes    ****************************************************
        function salvarlote(model) {
            return $http.post(servicebase.urlApi() + "/guia/salvarlote", model);
        }

        function addguiaslote(model) {
            return $http.post(servicebase.urlApi() + "/guia/addguiaslote", model);
        }

        function listarlotesporconvenio(idConvenio) {
            return $http.get(servicebase.urlApi() + "/guia/listarlotesporconvenio?idConvenio=" + idConvenio);
        }
        function obterloteporid(idLote) {
            return $http.get(servicebase.urlApi() + "/guia/obterloteporid?idLote=" + idLote);
        }
        function listarlotes() {
            return $http.get(servicebase.urlApi() + "/guia/listarlotes");
        }
        function excluirlote(idLote) {
            return $http.get(servicebase.urlApi() + "/guia/excluirlote?idLote=" + idLote);
        }
        function gerarlote(idLote) {
            return $http.get(servicebase.urlApi() + "/guia/gerarlote?idLote=" + idLote);
        }
        function xmlLoteConsulta(idLote) {
            download.requestSave({
                url: servicebase.urlApi() + "/guia/xmlLoteConsulta",
                dados: { idLote: idLote },
                contentType: 'text/xml; charset=utf-8',
                accept: 'text/xml; charset=utf-8',
                nomeDoArquivo: 'lote_guias_' + idLote + '.xml'
            });
        }
        function listarguiaslote(idConvenio, tipo) {
            return $http.get(servicebase.urlApi() + "/guia/listarguiaslote?idConvenio=" + idConvenio + "&tipo=" + tipo);
        }
        
        
    }
})();