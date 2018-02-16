(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.agenda', dsAgenda);

    dsAgenda.$inject = ['$http', 'common', 'appConfig'];

    function dsAgenda($http, common, appConfig) {

        var apiRoute = common.makeApiRoute('agenda');
        var service = {
            constants: {
                Datas: [{ date: '2017-04-26' }, { date: '2017-04-27' }, { date: '2017-04-28' }, { date: '2017-04-29' }, { date: '2017-04-30' }],
                Meses: [{ Meses: 5 }],//mes - 1
                Horarios: [{ Horario: '10:30' }, { Horario: '11:00' }, { Horario: '11:30' }, { Horario: '12:00' }, { Horario: '12:30' }]
            },
            listar: listar,
            salaespera: salaespera,
            obteragendaporid: obteragendaporid,
            marcado: marcado,
            realizado: realizado,
            cancelado: cancelado,
            listarAgendaPorProfissional: listarAgendaPorProfissional,
            listarHorariosDoPrestador: listarHorariosDoPrestador,
            listarDatas: listarDatas,
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
            liberaragenda: liberaragenda
        };

        return service;

        function valorAtendimento(tipo, idProcedimento, idConvenio) {
            return $http.get(common.makeUrl([apiRoute, 'valorAtendimento']), { params: { tipo: tipo, idProcedimento: idProcedimento, idConvenio: idConvenio } });
        }

        function tiposAtendimento() {
            return $http.get(common.makeUrl([apiRoute, 'tiposAtendimento']));
        }

        function especialidadesPorMedico(idMedico) {
            return $http.get(common.makeUrl([apiRoute, 'especialidadesPorMedico']), { params: { idMedico: idMedico } });
        }

        function procedimentoPorMedico(idMedico) {
            return $http.get(common.makeUrl([apiRoute, 'procedimentoPorMedico']), { params: { idMedico: idMedico } });
        }

        function conveniosPorProcedimento(idProcedimento) {
            return $http.get(common.makeUrl([apiRoute, 'conveniosPorProcedimento']), { params: { idProcedimento: idProcedimento } });
        }

        function agendaMedica(idMedico, data) {
            return $http.get(common.makeUrl([apiRoute, 'agendaMedica']), { params: { idMedico: idMedico, data: data } });
        }

        function especialidades() {
            return $http.get(common.makeUrl([apiRoute, 'especialidades']));
        }

        function convenios() {
            return $http.get(common.makeUrl([apiRoute, 'convenios']));
        }

        function agendar(model) {
            return $http.post(common.makeUrl([apiRoute, 'agendar']), model);
        }

        function datasdisponiveis(idMedico) {
            return $http.get(common.makeUrl([apiRoute, 'datasdisponiveis']), { params: { idMedico: idMedico } });
        }

        function horariosdisponiveis(idMedico, data) {
            return $http.get(common.makeUrl([apiRoute, 'horariosdisponiveis']), { params: { idMedico: idMedico, data: data } });
        }

        function listar(situacao, idprofissional) {
            return $http.get(common.makeUrl([apiRoute, 'listar']), { params: { situacao: situacao, idprofissional: idprofissional } });
        }

        function getUltimosAgendamentosPaciente(id) {
            return $http.get(common.makeUrl([apiRoute, 'getUltimosAgendamentosPaciente']), { params: { id: id } });
        }

        function listarDatas() {
            var datas = [{ Datas: '26/04/2017' }, { Datas: '27/04/2017' }, { Datas: '28/04/2017' }, { Datas: '29/04/2017' }, { Datas: '30/04/2017' }]
            return datas;
        }

        function listarHorariosDoPrestador(data) {
            var horarios = [{ Horario: '10:30' }, { Horario: '11:00' }, { Horario: '11:30' }, { Horario: '12:00' }, { Horario: '12:30' }]
            return horarios;
        }

        function salaespera() {
            return $http.get(common.makeUrl([apiRoute, 'salaespera']));
        }

        function obteragendaporid(id) {
            return $http.get(common.makeUrl([apiRoute, 'obteragendaporid']), { params: { id: id } });
        }

        function marcado(idagenda, idpaciente, idusuario) {
            return $http.get(common.makeUrl([apiRoute, 'marcado']), {
                params: {
                    idagenda: idagenda,
                    idpaciente: idpaciente,
                    idusuario:idusuario
                }
            });
        }

        function realizado(idagenda, idusuario) {
            return $http.get(common.makeUrl([apiRoute, 'realizado']), { params: { idagenda: idagenda, idusuario: idusuario } });
        }

        function cancelado(idagenda, idusuario) {
            return $http.get(common.makeUrl([apiRoute, 'cancelado']), { params: { idagenda: idagenda, idusuario: idusuario } });
        }

        function listarAgendaPorProfissional(idprof, data) {
            return $http.get(common.makeUrl([apiRoute, 'listarAgendaPorProfissional']), { params: { idprof: idprof, data: data } });
        }

        function pesquisar(pesq) {
            return $http.post(common.makeUrl([apiRoute, 'pesquisar']), pesq);
        }

        function liberaragenda(lib) {
            return $http.post(common.makeUrl([apiRoute, 'liberaragenda']), lib);
        }
    }
})();
