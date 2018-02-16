(function () {
    'use strict';

    angular
        .module('app.financeiro')
        .controller('DashboardFinanceiro', DashboardFinanceiro);

    DashboardFinanceiro.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'dashboardservice', '$stateParams'];
    function DashboardFinanceiro($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, dashboardservice, $stateParams) {
        var vm = this;

        common.setBreadcrumb('Financeiro .Dashboard');

        vm.init = init;

        init();

        function init() {

            var blocker = blockUI.instances.get('blockModaldadosfinanceiros');
            blocker.start();
            var pContas = dashboardservice.getTotaisfinanceiro();
            pContas.then(function (result) {
                vm.apagar = result.data.TotalaPagar;
                vm.areceber = result.data.TotalaReceber;
                vm.recebidas = result.data.TotalContasRecebidas;
                vm.pagas = result.data.TotalaContasPagas;
            });

            var pPie = dashboardservice.getdespesas();
            pPie.then(function (result) {
                grafico(result.data);
            });

            var pBarras = dashboardservice.getResceitaxDespesas();
            pBarras.then(function (result) {
                graficoBarras(result.data);
            });

            $q.all([pContas, pPie, pBarras]).then(function () {

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
            });
        }

        function graficoBarras(data) {

            console.log(data);
            var receita = [];
            var despesa = [];

            var desp = _.sortBy(data.Despesas, 'Mes');
            console.log(desp);

            var rec = _.sortBy(data.Receitas, 'Mes');
            console.log(rec);

            _.forEach(desp, function (item) {
                despesa.push(item.Valor);
            });

            _.forEach(rec, function (item) {
                receita.push(item.Valor);
            });

            
            
            Highcharts.chart('container', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Receita x Despesa'
                },
                subtitle: {
                    text: 'ano atual'
                },
                xAxis: {
                    categories: [
                        'Jan',
                        'Fev',
                        'Mar',
                        'Abr',
                        'Mai',
                        'Jun',
                        'Jul',
                        'Aug',
                        'Set',
                        'Out',
                        'Nov',
                        'Dez'
                    ],
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'R$'
                    }
                },
                colors: ["#3CB371", "#cc0000"],
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.,1f} </b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    {
                        name: 'Receita',
                        data: receita

                    },
                    {
                        name: 'Despesa',
                        data: despesa
                    }
                ]
            });
        }

        function grafico(dados) {

            var data = [];

            angular.forEach(dados, function (value) {
                var item = {
                    name: value.PlanoConta,
                    y: parseFloat(value.Valor),
                };

                data.push(item);
            });

            Highcharts.chart('container2', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Despesas por categoria'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Despesas',
                    colorByPoint: true,
                    data: data
                }]
            });
        }

    }
})();