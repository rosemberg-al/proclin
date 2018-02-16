(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('relatorioservice', relatorioservice); //Define o nome a função do seu .service

    relatorioservice.$inject = ['$http', 'download', 'servicebase']; //Lista de dependências
    function relatorioservice($http, download, servicebase) {

        var service = this;

        var service = {
            printRelAgendaMedica: printRelAgendaMedica,
            printRelAniversariantes: printRelAniversariantes,
            printRelOcupacoes: printRelOcupacoes,
            printRelEspecialidades: printRelEspecialidades,
            printRelProcedimentos:printRelProcedimentos,
            printRelConvenios:printRelConvenios,
            printRelPacientes:printRelPacientes,
            printRelFornecedores:printRelFornecedores,
            printRelCheques:printRelCheques,
            printRelFinanceiro:printRelFinanceiro,
            printRelQtdeProcedimentosRealizados:printRelQtdeProcedimentosRealizados,
            printRelFaturamento:printRelFaturamento,
            printRelPlanoAgenda: printRelPlanoAgenda
        }

        return service;

        //Implementação das funções
        function printRelFaturamento(datainicio, datatermino, idprofissional,idpaciente,situacao,tipo) {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelFaturamento?datainicio=" + datainicio + "&datatermino=" + datatermino + "&idprofissional=" + idprofissional+ "&idpaciente=" + idpaciente+ "&situacao=" + situacao+ "&tipo=" + tipo
            });
        }
        function printRelPlanoAgenda(datainicio, datatermino, idprofissional,idUnidade) {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelPlanoAgenda?datainicio=" + datainicio + "&datatermino=" + datatermino + "&idprofissional=" + idprofissional + "&idUnidade=" + idUnidade
            });
        }


        function printRelAgendaMedica(datainicio, datatermino, idprofissional,idpaciente,situacao) {
            return  download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelAgendaMedica?datainicio=" + datainicio + "&datatermino=" + datatermino + "&idprofissional=" + idprofissional+ "&idpaciente=" + idpaciente+ "&situacao=" + situacao
            });
        }

        function printRelAniversariantes(mes) {
            return  download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelAniversariantes?mes=" + mes
            });
        }
        function printRelOcupacoes() {
            return  download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelOcupacoes"
            });
        }

        function printRelEspecialidades() {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelEspecialidades"
            });
        }

        function printRelProcedimentos() {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelProcedimentos"
            });
        }

        function printRelConvenios() {
           return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelConvenios"
            });
        }

        function printRelPacientes() {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelPacientes"
            });
        }

        function printRelFornecedores() {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelFornecedores"
            });
        }
        
        function printRelCheques(datainicio,datatermino,situacao) {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelCheques?datainicio=" + datainicio + "&datatermino=" + datatermino + "&situacao=" + situacao
            });
        }

        function printRelFinanceiro(datainicio,datatermino,tipo,situacao,idpessoa) {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelFinanceiro?datainicio=" + datainicio + "&datatermino=" + datatermino + "&tipo=" + tipo+"&situacao=" + situacao+"&idpessoa="+idpessoa
            });
        }

        function printRelQtdeProcedimentosRealizados(datainicio,datatermino) {
            return download.requestLoad({
                url: servicebase.urlApi() + "/relatorio/printRelQtdeProcedimentosRealizados?datainicio=" + datainicio + "&datatermino=" + datatermino 
            });
        }
    }
})();