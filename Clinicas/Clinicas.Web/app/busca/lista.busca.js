(function () {
    'use strict';

    angular
        .module('app.busca')
        .controller('Busca', Busca);

    Busca.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification','buscaservice', '$stateParams'];

    function Busca($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification,buscaservice, $stateParams) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Pesquisa .Busca');

        //Funções
        vm.init = init;
        
        //Feature Start
        init();

        //Implementations
        function init() {
            
            vm.busca = $stateParams.param;
            
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaFor');
            blocker.start();

            buscaservice
                .busca($stateParams.param)
                .then(function (result) {
                    vm.dados = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }
    }
})();