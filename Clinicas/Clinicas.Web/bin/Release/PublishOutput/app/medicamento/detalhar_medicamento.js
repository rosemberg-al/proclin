(function () {
    'use strict';

    angular
        .module('app.medicamento')
        .controller('DetalharMedicamento', DetalharMedicamento);

    DetalharMedicamento.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'item'];

    function DetalharMedicamento($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, comumservice, item) {

        var vm = this;
        vm.State = "Detalhar Medicamento";
        vm.FormMessage = "";
        vm.funcionario = {};

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.cancel = cancel;

        //Feature Start
        init();


        //Implementations
        function init() {
            vm.medicamento = item;
        }


        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        

    }
})();