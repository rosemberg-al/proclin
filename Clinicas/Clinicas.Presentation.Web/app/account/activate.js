/**
*
*  Controller Ativação Cadastro
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .controller('activateAccountController', activateAccountController);

    activateAccountController.$inject = ['$scope', '$state', '$stateParams', 'common', 'notification', 'exception', 'authService'];

    function activateAccountController($scope, $state, $stateParams, common, notification, exception, authService) {
        var vm = this;
        vm.message = '';
        vm.formValid = false;
        vm.ResetPassword = false;

        vm.Account = {
            UserId: '',
            NewPassword: '',
            ConfirmPassword: '',
            Token: ''
        }

        authService.logOut();

        //Verifica as variáves GET
        if (angular.isUndefined($stateParams) || angular.isUndefined($stateParams.userId) || angular.isUndefined($stateParams.hashcode)) {
            $state.go('login');
        } else {
            vm.Account.UserId = $stateParams.userId;
            vm.Account.Token = $stateParams.hashcode;
            if (angular.isDefined($stateParams.reset) && $stateParams.reset.length > 0) {
                vm.ResetPassword = true;
            }
        }

        vm.confirmAccount = function () {

            vm.message = '';
            vm.formValid = common.validateForm($scope.confirmAccountForm);

            if (vm.formValid) {

                if (vm.Account.NewPassword.length < 6 || vm.Account.NewPassword.length > 20) {
                    vm.message = 'Sua senha deve conter entre 6 e 20 caracteres';
                    return;
                }

                if (vm.Account.NewPassword != vm.Account.ConfirmPassword) {
                    vm.message = 'Senha e confirma senha não conferem';
                    return;
                }

                authService.confirmAccount(vm.Account).then(function (response) {

                    //Usuário ativado, loga o mesmo
                    authService.login({
                        UserName: response.data,
                        Password: vm.Account.NewPassword
                    }).then(function () {
                        if (vm.ResetPassword = false) {
                            notification.showSuccess('Sua conta foi ativada com sucesso.');
                        }
                        else {
                            notification.showSuccess('Sua senha foi redefinida com sucesso.');
                        }
                        $state.go('pagina-inicial');

                    },
                    function (err) {
                        notification.showError(err);
                        exception.throwEx(err);
                    });

                },
                function (err) {
                    notification.showError(err.data);
                    exception.throwEx(err.data);
                });
                
            } else {
                vm.message = 'Favor preencher os campos abaixo:';
            }
        };

    }
})();