(function () {
    'use strict';

    angular
        .module('app.especialidade')
        .controller('EspecialidadeProcedimento', EspecialidadeProcedimento);

    EspecialidadeProcedimento.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'especialidade'];

    function EspecialidadeProcedimento($scope, $http, $modal, $modalInstance, blockUI, common, notification, cadastroservice, especialidade) {

        var vm = this;
        vm.State = "Especialidade";
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
        vm.especialidade = especialidade;

        //Implementations
        function init() {
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }
    }
})();