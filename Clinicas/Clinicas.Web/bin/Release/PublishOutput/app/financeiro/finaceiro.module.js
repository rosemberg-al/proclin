(function () {
    'use strict';

    var appFinanceiro = angular.module('app.financeiro', []);

    appFinanceiro.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
        .state('receber', {
            parent: 'app',
            url: "/financeiro/:tipo",
            templateUrl: "app/financeiro/lista_financeiro.html",
            controller: "FinanceiroController as vm",
            data: { pageTitle: 'Contas a receber', specialClass: 'fixed-sidebar' }
        })
        .state('pagar', {
            parent: 'app',
            url: "/financeiro/:tipo",
            templateUrl: "app/financeiro/lista_financeiro.html",
            controller: "FinanceiroController as vm",
            data: { pageTitle: 'Contas a pagar', specialClass: 'fixed-sidebar' }
        })
        .state('addparcelas', {
            parent: 'app',
            url: "/novaConta/:id?tipo",
            templateUrl: "app/financeiro/crud_financeiro.html",
            controller: "CrudFinanceiro as vm",
            data: { pageTitle: 'Contas a pagar', specialClass: 'fixed-sidebar' }
        })
        .state('planocontas', {
            parent: 'app',
            url: "/planocontas",
            templateUrl: "app/financeiro/lista_planocontas.html",
            controller: "PlanoContas as vm",
            data: { pageTitle: 'Plano de Contas', specialClass: 'fixed-sidebar' }
        })
        .state('transferencias', {
            parent: 'app',
            url: "/transferencias",
            templateUrl: "app/financeiro/lista_transferencias.html",
            controller: "Transferencias as vm",
            data: { pageTitle: 'Transferencias entre contas', specialClass: 'fixed-sidebar' }
        })
        .state('editarparcela', {
            parent: 'app',
            url: "/Parcelas/:id?tipo",
            templateUrl: "app/financeiro/edit_parcela.html",
            controller: "EditarParcela as vm",
            data: { pageTitle: 'Editar Parcela', specialClass: 'fixed-sidebar' }
            })
        .state('cheques', {
            parent: 'app',
            url: "/cheques",
            templateUrl: "app/financeiro/lista.cheque.html",
            controller: "Cheques as vm"
        })
        .state('bancos', {
            parent: 'app',
            url: "/bancos",
            templateUrl: "app/financeiro/lista.banco.html",
            controller: "Bancos as vm"
        }) 
        .state('conta', {
            parent: 'app',
            url: "/conta",
            templateUrl: "app/financeiro/lista.conta.html",
            controller: "Conta as vm"
        })
        .state('dashboardfinanceiro', {
            parent: 'app',
            url: "/dashboardfinanceiro",
            templateUrl: "app/financeiro/dashboard_financeiro.html",
            controller: "DashboardFinanceiro as vm"
        })
    }]);
})();   