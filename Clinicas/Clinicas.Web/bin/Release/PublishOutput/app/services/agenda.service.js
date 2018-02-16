(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('agendaservice', agendaservice); //Define o nome a função do seu .service

    agendaservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function agendaservice($http, servicebase) {

        var service = this;

        var service = {
            constants: {
                Datas: [{ date: '2017-04-26' }, { date: '2017-04-27' }, { date: '2017-04-28' }, { date: '2017-04-29' }, { date: '2017-04-30' }],
                Meses: [{ Meses: 5 }],//mes - 1
                Horarios: [{ Horario: '10:30' }, { Horario: '11:00' }, { Horario: '11:30' }, { Horario: '12:00' }, { Horario: '12:30' }]
            },

            listarAgendaAguardando: listarAgendaAguardando,
            listarAgendaProfissionalSaudePorMes: listarAgendaProfissionalSaudePorMes,
            listar: listar,
            realizado: realizado,
            cancelado: cancelado,
            marcado: marcado,
            salaespera: salaespera,
            encaminharsalaespera: encaminharsalaespera,
            obteragendaporid: obteragendaporid,
            pesquisarAgenda:pesquisarAgenda,
            
            
            listarAgendaPorProfissional: listarAgendaPorProfissional,
            getUltimosAgendamentosPaciente: getUltimosAgendamentosPaciente,
            datasdisponiveis: datasdisponiveis,
            horariosdisponiveis: horariosdisponiveis,
            agendar: agendar,
            agendaMedica: agendaMedica,
            especialidades: especialidades,
            convenios: convenios,
            especialidadesPorMedico: especialidadesPorMedico,
            procedimentoPorMedico: procedimentoPorMedico,
            conveniosPorProcedimento: conveniosPorProcedimento,
            tiposAtendimento: tiposAtendimento,
            valorAtendimento: valorAtendimento,
            pesquisar: pesquisar,
            liberaragenda: liberaragenda,
            gerarAgenda: gerarAgenda,

            listarBloqueioAgenda: listarBloqueioAgenda,
            obterBloqueioAgenda: obterBloqueioAgenda,
            salvarBloqueioAgenda: salvarBloqueioAgenda,
            excluirBloqueioAgenda: excluirBloqueioAgenda,


            convocarPaciente:convocarPaciente,
            listarPacientesConvocados: listarPacientesConvocados

        }

        return service;


        //Implementação das funções
        //***********************************   agenda    ***************************************************

        function convocarPaciente(idagenda) {
            return $http.get(servicebase.urlApi() + "/agenda/convocarPaciente?idagenda=" + idagenda);
        }

        function listarPacientesConvocados() {
            return $http.get(servicebase.urlApi() + "/agenda/listarPacientesConvocados");
        }


        function listarAgendaAguardando(idprofissional) {
            return $http.get(servicebase.urlApi() + "/agenda/listarAgendaAguardando?idprofissional=" + idprofissional);
        }

        function encaminharsalaespera(idagenda) {
            return $http.get(servicebase.urlApi() + "/agenda/encaminharsalaespera?idagenda=" + idagenda);
        }

        function salaespera(idprofissional) {
            return $http.get(servicebase.urlApi() + "/agenda/salaespera?idprofissional="+idprofissional);
        }

        function listarAgendaProfissionalSaudePorMes(mes, idprofisssional) {
            return $http.get(servicebase.urlApi() + "/agenda/listarAgendaProfissionalSaudePorMes?mes=" + mes + "&idprofissional=" + idprofisssional);
        }

        function listar(situacao,idprofissional) {
            return $http.get(servicebase.urlApi() + "/agenda/listarAgenda?situacao=" + situacao + "&idprofissional=" + idprofissional);
        }

        function pesquisarAgenda(idagenda, idprofissional, paciente, dataInicio, dataTermino,situacao) {
            return $http.get(servicebase.urlApi() + "/agenda/pesquisarAgenda/?idagenda=" + idagenda + "&idprofissional=" + idprofissional+ "&paciente=" + paciente+ "&dataInicio=" + dataInicio+ "&dataTermino=" + dataTermino+"&situacao=" + situacao);
        }















        function valorAtendimento(tipo, idProcedimento, idConvenio) {
            return $http.get(servicebase.urlApi() + "/agenda/valorAtendimento?tipo=" + tipo + "&idProcedimento=" + idProcedimento + "&idConvenio=" + idConvenio);
        }

        function tiposAtendimento() {
            return $http.get(servicebase.urlApi() + "/agenda/tiposAtendimento");
        }

        function especialidadesPorMedico(idMedico) {
            return $http.get(servicebase.urlApi() + "/agenda/especialidadesPorMedico?idMedico=" + idMedico);
        }

        function procedimentoPorMedico(idMedico) {
            return $http.get(servicebase.urlApi() + "/agenda/procedimentoPorMedico?idMedico=" + idMedico);
        }

        function conveniosPorProcedimento(idProcedimento) {
            return $http.get(servicebase.urlApi() + "/agenda/conveniosPorProcedimento?idProcedimento=" + idProcedimento);
        }

        function agendaMedica(idMedico, data) {
            return $http.get(servicebase.urlApi() + "/agenda/agendaMedica?idMedico=" + idMedico + "&data=" + data);
        }

        function especialidades() {
            return $http.get(servicebase.urlApi() + "/agenda/especialidades");
        }

        function convenios() {
            return $http.get(servicebase.urlApi() + "/agenda/convenios");
        }

        function agendar(model) {
            return $http.post(servicebase.urlApi() + "/agenda/agendar", model);
        }
        
        function datasdisponiveis(idMedico) {
            return $http.get(servicebase.urlApi() + "/agenda/datasdisponiveis?idMedico=" + idMedico);
        }

        function horariosdisponiveis(idMedico, data) {
            return $http.get(servicebase.urlApi() + "/agenda/horariosdisponiveis?idMedico=" + idMedico + "&data=" + data);
        }

    

       
        function getUltimosAgendamentosPaciente(id) {
            return $http.get(servicebase.urlApi() + "/agenda/getUltimosAgendamentosPaciente?id=" + id);
        }

      

        function obteragendaporid(id) {
            return $http.get(servicebase.urlApi() + "/agenda/obteragendaporid?id=" + id);
        }

        function marcado(idagenda, idpaciente, idusuario) {
            return $http.get(servicebase.urlApi() + "/agenda/marcado?idagenda=" + idagenda + "&idpaciente=" + idpaciente + "&idusuario=" + idusuario);
        }

        function realizado(idagenda) {
            return $http.get(servicebase.urlApi() + "/agenda/realizado?idagenda=" + idagenda);
        }


        function cancelado(idagenda) {
            return $http.get(servicebase.urlApi() + "/agenda/cancelado?idagenda=" + idagenda);
        }

        function listarAgendaPorProfissional(idprof, data) {
            return $http.get(servicebase.urlApi() + "/agenda/listarAgendaPorProfissional?idprof=" + idprof + "&data=" + data);
        }

        function pesquisar(pesq) {
            return $http.get(servicebase.urlApi() + "/agenda/pesquisar?pesq=" + pesq);
        }

        function liberaragenda(obj) {
            return $http.post(servicebase.urlApi() + "/agenda/liberaragenda", obj);
        }

        function gerarAgenda(model) {
            return $http.post(servicebase.urlApi() + "/agenda/gerarAgenda", model);
        }


        function listarBloqueioAgenda() {
            return $http.get(servicebase.urlApi() + "/agenda/listarBloqueioAgenda");
        }
        function excluirBloqueioAgenda(id) {
            return $http.get(servicebase.urlApi() + "/agenda/excluirBloqueioAgenda?id=" + id);
        }
        function obterBloqueioAgenda(id) {
            return $http.get(servicebase.urlApi() + "/agenda/obterBloqueioAgenda?id=" + id);
        }

        function salvarBloqueioAgenda(model) {
            return $http.post(servicebase.urlApi() + "/agenda/salvarBloqueioAgenda", model);
        }

    }
})();