(function () {
    'use strict';

    angular
        .module('app.especialidade')
        .controller('ProcedimentoEspecialidade', ProcedimentoEspecialidade);

    ProcedimentoEspecialidade.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'procedimento'];

    function ProcedimentoEspecialidade($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice, procedimento) {

        var vm = this;
        vm.State = "Procedimento";
        vm.FormMessage = "";
        vm.especialidades = {};

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        //vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();
        vm.procedimento = procedimento;

        //Implementations
        function init() {
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }
    }
})();