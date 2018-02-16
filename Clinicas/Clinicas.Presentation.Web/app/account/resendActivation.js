/**
*
*  Controller Login
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .controller('resendActivationController', resendActivationController);

    resendActivationController.$inject = ['$scope', '$state', 'common', 'exception', 'authService'];

    function resendActivationController($scope, $state, common, exception, authService) {

        var vm = this;
        vm.message = '';
        vm.formValid = false;
        vm.User = {
            Cnpj: ""
        }

        authService.logOut();

        vm.resendActivation = function () {

            vm.message = '';
            vm.formValid = common.validateForm($scope.resendActivationForm);

            if (vm.formValid) {
                authService.resendActivation(vm.User).then(function (response) {

                    vm.success = 'Chave de ativação reenviada com sucesso.';

                },
                    function (err) {
                        exception.throwEx(err);
                    });
            } else {
                if ($scope.resendActivationForm.Cnpj.$error.cnpj) {
                    vm.message = 'O CNPJ digitado não é válido';
                }
                else {
                    vm.message = 'Favor preencher os campos abaixo:';
                }
            }

        };

    }
})();