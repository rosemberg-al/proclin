(function () {
    'use strict';

    angular
		.module('app.core')
        .controller('alertController', alertController);

    alertController.$inject = ['$modalInstance', 'alert'];

    function alertController($modalInstance, alert) {

        var vm = this;
        vm.ok = ok;
        vm.cancel = cancel;

        vm.alertOptions = {
            Title: 'Atenção',
            Text: 'Confirma esta ação?',
            Action: 'Ok'
        }

        angular.extend(vm.alertOptions, alert);

        function ok() {
            $modalInstance.close(true);
        }

        function cancel() {
            $modalInstance.close(false);
        }
    }
})();
