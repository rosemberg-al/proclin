(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('prontuarioservice', prontuarioservice); //Define o nome a função do seu .service

    prontuarioservice.$inject = ['$http', 'servicebase', 'download']; //Lista de dependências
    function prontuarioservice($http, servicebase, download) {

        var service = this;

        var service = {

            getUltimosAtendimentos:getUltimosAtendimentos,
            listarAnamneses:listarAnamneses,
            obterAnamnesePorId:obterAnamnesePorId,
            salvarAnamnese:salvarAnamnese,
            excluirAnamnese:excluirAnamnese,

            listarHistoriaPregressa: listarHistoriaPregressa,
            obterHistoriaPregressaPorIdPaciente: obterHistoriaPregressaPorIdPaciente,
            salvarHistoriaPregressa:salvarHistoriaPregressa,
            excluirHistoriaPregressa:excluirHistoriaPregressa,
                          
            salvarMedidas: salvarMedidas,
            obterMedidas: obterMedidas,
            listarMedidas: listarMedidas,
            excluirMedidas: excluirMedidas,

            saveAtestado: saveAtestado,
            getAtestadoById: getAtestadoById,
            listarAtestados: listarAtestados,
            getModelosAtestados: getModelosAtestados,
            excluirAtestado:excluirAtestado,
            printAtestado: printAtestado,



            saveReceituario: saveReceituario,
            getReceituarioById: getReceituarioById,
            excluirReceituario :excluirReceituario,
            listarReceituario: listarReceituario,
            getModelosReceituarios: getModelosReceituarios,
            printReceituario: printReceituario,
            
            saveModelo: saveModelo,
            getModeloById: getModeloById,
            getModelos: getModelos,
            excluirModelo: excluirModelo,
            pesquisarModelos: pesquisarModelos,
            listarModeloProntuarioPorTipo:listarModeloProntuarioPorTipo,
            
            getHospitais: getHospitais,
            getHospitalById: getHospitalById,
            saveHospital: saveHospital,
            listarHospitaisPorNome: listarHospitaisPorNome,
          

            getVacinas: getVacinas,
            getVacinaById: getVacinaById,
            saveVacina: saveVacina,
            
            saveRequisicaoExame: saveRequisicaoExame,
            getRequisicaoById: getRequisicaoById,
            getRequisicaoByPaciente: getRequisicaoByPaciente,
            
            printRequisicao: printRequisicao,




            obterOdontogramaPorId: obterOdontogramaPorId,
            listarOdontogramaPorIdPaciente: listarOdontogramaPorIdPaciente,
            salvarOdontograma: salvarOdontograma,
            excluirOdontograma: excluirOdontograma,
            printOrcamento: printOrcamento
            
        }
        return service;

        function getUltimosAtendimentos(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getUltimosAtendimentos?id=" + id);
        }

        //***********************************   Odontograma     ***************************************************
        function obterOdontogramaPorId(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/obterOdontogramaPorId?id=" + id);
        }
        function listarOdontogramaPorIdPaciente(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarOdontogramaPorIdPaciente?id=" + id);
        }

        function printOrcamento(id) {
            download.requestLoad({
                url: servicebase.urlApi() + "/prontuario/printOrcamento?id=" + id
            });
        }

        function salvarOdontograma(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/salvarOdontograma", model);
        }

        function excluirOdontograma(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirOdontograma?id=" + id);
        }

        //***********************************   história     ***************************************************
        function listarHistoriaPregressa(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarHistoriaPregressa?id=" + id);
        }
        function obterHistoriaPregressaPorIdPaciente(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/obterHistoriaPregressaPorIdPaciente?id=" + id);
        }

        function salvarHistoriaPregressa(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/salvarHistoriaPregressa", model);
        }

        function excluirHistoriaPregressa(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirHistoriaPregressa?id=" + id);
        }

        //***********************************   anamnese     ***************************************************
        function listarAnamneses (id) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarAnamneses?id=" + id);
        }
        function obterAnamnesePorId(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/obterAnamnesePorId?id=" + id);
        }
        function salvarAnamnese(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/salvarAnamnese", model);
        }
        function excluirAnamnese(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirAnamnese?id=" + id);
        }
        
        //***********************************   receituario pacinte     ***************************************************

        function getReceituarioById(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getReceituarioById?id=" + id);
        }

        function listarReceituario(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarReceituario?id=" + id);
        }

        function getModelosReceituarios() {
            return $http.get(servicebase.urlApi() + "/prontuario/getModelosReceituarios");
        }
       
        function saveReceituario(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/saveReceituario", model);
        }

         function excluirReceituario(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirReceituario?id=" + id);
        }

        function printReceituario(id) {
            download.requestLoad({
                url: servicebase.urlApi() + "/prontuario/printReceituario?id=" + id
            });
        }

        //***********************************   atestado pacinte     ***************************************************
        function listarAtestados(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarAtestados?id=" + id);
        }

        function getAtestadoById(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getAtestadoById?id=" + id);
        }

        function excluirAtestado(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirAtestado?id=" + id);
        }


        function getModelosAtestados() {
            return $http.get(servicebase.urlApi() + "/prontuario/getModelosAtestados");
        }

        function saveAtestado(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/saveAtestado", model);
        }

        function printAtestado(id) {
            download.requestLoad({
                url: servicebase.urlApi() + "/prontuario/printAtestado?id=" + id
            });
        }

        //***********************************   vacinas pacinte     ***************************************************
        function getVacinaById(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getVacinaById?id=" + id);
        }

        function getVacinas() {
            return $http.get(servicebase.urlApi() + "/prontuario/getVacinas");
        }

        function saveVacina(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/saveVacina", model);
        }

        //***********************************   medidas pacinte     ***************************************************
        function listarMedidas(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarMedidas?id=" + id);
        }
        function obterMedidas(idmedida) {
            return $http.get(servicebase.urlApi() + "/prontuario/obterMedidas?idmedida=" + idmedida);
        }

        
        function excluirMedidas (id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirMedidas?id=" + id);
        }

        function salvarMedidas(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/salvarMedidas", model);
        }

        //***********************************   modelo     ***************************************************
        function getVacinas() {
            return $http.get(servicebase.urlApi() + "/prontuario/getVacinas");
        }


        function getModeloById(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getModeloById?id=" + id);
        }

        function getModelos() {
            return $http.get(servicebase.urlApi() + "/prontuario/getModelos");
        }

        function saveModelo(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/saveModelo", model);
        }

        function excluirModelo(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/excluirModelo?id=" + id);
        }

        function pesquisarModelos(nome) {
            return $http.get(servicebase.urlApi() + "/prontuario/pesquisarModelos?nome=" + nome);
        }

        function listarModeloProntuarioPorTipo(tipo) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarModeloProntuarioPorTipo?tipo=" + tipo);
        }

        //***********************************   hospital     ***************************************************
        function getHospitalById(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getHospitalById?id=" + id);
        }

        function getHospitais() {
            return $http.get(servicebase.urlApi() + "/prontuario/getHospitais");
        }

        function saveHospital(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/saveHospital", model);
        }

        function listarHospitaisPorNome(nome) {
            return $http.get(servicebase.urlApi() + "/prontuario/listarHospitaisPorNome?nome=" + nome);
        }

        //***********************************   requisição de exames     ***************************************************

        function getRequisicaoById(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getRequisicaoById?id=" + id);
        }
        function getRequisicaoByPaciente(id) {
            return $http.get(servicebase.urlApi() + "/prontuario/getRequisicaoByPaciente?id=" + id);
        }

        function saveRequisicaoExame(model) {
            return $http.post(servicebase.urlApi() + "/prontuario/saveRequisicaoExame", model);
        }

        function printRequisicao(id) {
            download.requestLoad({
                url: servicebase.urlApi() + "/prontuario/printRequisicao?id=" + id
            });
        }
    }
})();