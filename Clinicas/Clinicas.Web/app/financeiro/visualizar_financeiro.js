(function () {
    'use strict';

    angular
        .module('app.financeiro')
        .controller('VisualizarFinanceiro', VisualizarFinanceiro);

    VisualizarFinanceiro.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'financeiroservice', 'idfinanceiro','idparcela'];

    function VisualizarFinanceiro($scope, $http, $modal, $modalInstance, blockUI, common, notification, financeiroservice, idfinanceiro,idparcela) {

        var vm = this;
        vm.FormMessage = "";

        $scope.forms = {};
        vm.formValid = true;


        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.idparcela = idparcela;
      //   vm.editarparcela = editarparcela;

        //Feature Start
        init();

        //Implementations
        function init() {
           financeiroservice.getFinanceiroPorId(idfinanceiro).success(function (response) {
                vm.financeiro = response;
                
            });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }


        /* function editarparcela(tipo,idparcela,idfinanceiro) {
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/edit_parcela.html',
                controller: "EditarParcela as vm",
                //windowClass: "animated flipInY",
                backdrop: 'static',
                size: 'lg',
                resolve: {
                    tipo:function(){
                        return tipo;
                    },
                    idparcela: function () {
                        return idparcela;
                    },
                    idfinanceiro: function () {
                        return idfinanceiro;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        } */

    }
})();