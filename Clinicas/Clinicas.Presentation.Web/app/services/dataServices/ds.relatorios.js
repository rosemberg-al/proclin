(function () {
    'use strict';

    angular.module('app.dataServices')
		.factory('ds.relatorios', dsRelatorios);

    dsRelatorios.$inject = ['$http', 'common', 'download', 'appConfig'];
    function dsRelatorios($http, common, download, appConfig) {

        var apiRoute = common.makeApiRoute('relatorio');

        var service = {
            printRelEspecialidades: printRelEspecialidades,
            printRelFornecedores: printRelFornecedores,
            printRelOcupacoes: printRelOcupacoes,
            printRelAgendaMedica: printRelAgendaMedica,
            printRelOcupacoes: printRelOcupacoes,
            printRelAniversariantes: printRelAniversariantes,
            printRelConvenios: printRelConvenios,
            printRelProcedimentosRealizados: printRelProcedimentosRealizados,
            printRelPacientes: printRelPacientes
        }
        return service;

        //Implementação das funções de relatórios
        //************************************************************************************************************************

        function printRelEspecialidades(operador) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelEspecialidades']),
                params: {
                    operador: operador
                }
            });
        }

        function printRelAniversariantes(operador) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelAniversariantes']),
                params: {
                    operador: operador
                }
            });
        }

        function printRelConvenios(operador, convenio) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelConvenios']),
                params: {
                    operador: operador,
                    convenio: convenio
                }
            });
        }

        function printRelFornecedores(operador) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelFornecedores']),
                params: {
                    operador: operador
                }
            });
        }

        function printRelOcupacoes(operador) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelOcupacoes']),
                params: {
                    operador: operador
                }
            });
        }

        function printRelAgendaMedica(operador, datainicio, datatermino, idprofissional) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelAgendaMedica']),
                params: {
                    operador: operador,
                    datainicio: datainicio,
                    datatermino: datatermino,
                    idprofissional: idprofissional
                }
            });
        }

        function printRelProcedimentosRealizados(operador, datainicio, datatermino) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelProcedimentosRealizados']),
                params: {
                    operador: operador,
                    datainicio: datainicio,
                    datatermino: datatermino
                }
            });
        }

        function printRelPacientes(operador) {
            return download.requestLoad({
                url: common.makeUrl([apiRoute, 'printRelPacientes']),
                params: {
                    operador: operador
                }
            });
        }


        

    }
})();