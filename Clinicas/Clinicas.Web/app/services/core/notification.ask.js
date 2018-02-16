(function () {
	'use strict';

	angular
		.module('app.core')
        .controller('askController', askController);

	askController.$inject = ['$modalInstance', 'ask'];

	function askController($modalInstance, ask) {

		var vm = this;
		vm.confirm = confirm;
		vm.cancel = cancel;

		vm.askOptions = {
			Title: 'Confirmar',
			Text: 'Confirma esta ação?',
			Yes: 'Sim',
			No: 'Não'
		}

		angular.extend(vm.askOptions, ask);

		function confirm() {
			$modalInstance.close(true);
		}

		function cancel() {
            $modalInstance.close(false);
		}
	}
})();
