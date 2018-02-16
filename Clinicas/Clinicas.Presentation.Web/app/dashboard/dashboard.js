(function () {
    'use strict';

    angular
        .module('app.dashboard')
        .controller('Dashboard', Dashboard);

    Dashboard.$inject = ['$scope', '$http', 'blockUI', 'common', '$modal', 'exception', 'ds.dashboard'];

    function Dashboard($scope, $http, blockUI, common, $modal, exception, DsDashboard) {

        common.setBreadcrumb('Página Inicial');
        var vm = this;

        //Funções
        vm.init = init;
        vm.edit = edit;
        vm.receituario = receituario;

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

        function edit(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/anamnese/crud.anamnese.html',
                controller: 'AnamneseCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }

        function receituario(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/receituario/crud.receituario.html',
                controller: 'ReceituarioCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }
    }
})();