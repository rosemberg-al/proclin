(function () {
    'use strict';

    angular
        .module('app.home')
        .controller('Home', Home);

    Home.$inject = ['$scope', '$http', 'blockUI', 'common', '$modal', 'exception', 'ds.dashboard'];

    function Home($scope, $http, blockUI, common, $modal, exception, DsDashboard) {

        common.setBreadcrumb('Página Inicial');
        var vm = this;

        //Funções
        vm.init = init;
        //Feature Start
        init();

        //Implementations
        function init() {

            vm.pacientesemespera = [
                { Id: 12, Nome: 'MARINA FERREIRA LOPES DE OLIVEIRA', Idade: '28', Sexo: 'Feminino', Convenio: 'Unimed BH' },
                { Id: 24, Nome: 'MAUROSSEZ DORNELAS FRANCO', Idade: '29', Sexo: 'Masculino', Convenio: 'Unimed BH' },
                { Id: 36, Nome: 'MICHELLY GOMES CONDE', Idade: '28', Sexo: 'Feminino', Convenio: 'Unimed BH' },
                { Id: 35, Nome: 'MARIA DE LOURDES GOMES CONDE', Idade: '28', Sexo: 'Feminino', Convenio: 'Promed' },
                { Id: 38, Nome: 'LIDIANE VIANA NOGUEIRA', Idade: '28', Sexo: 'Feminino', Convenio: 'Bradesco' }
            ];

        }

        
        
    }
})();