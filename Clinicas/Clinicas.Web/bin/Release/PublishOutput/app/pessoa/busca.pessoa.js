﻿(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('BuscaPessoa', BuscaPessoa);

    BuscaPessoa.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function BuscaPessoa($scope, $http, $q, $modal, $modalInstance, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.selecionar = selecionar;
        vm.buscar = buscar;
        
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);
        }

        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome";
            }
            else {
                vm.FormMessage = "";
                cadastroservice
                    .getPessoaPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.pessoas = result.data;
                   })
                   .catch(function (ex) {
                       notification.showError(ex.data.Message);
                   });
            }
        }

        function selecionar(item) {
            $modalInstance.close(item);
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

    }
})();