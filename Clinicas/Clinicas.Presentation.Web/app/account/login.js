(function () {
    'use strict';

    angular.module('app')
           .controller('loginController', loginController);

    loginController.$inject = ['$scope', '$state', '$modal', 'common', 'notification', 'exception', 'authService'];

    function loginController($scope, $state, $modal, common, notification, exception, authService) {
        var vm = this;
        vm.message = '';
        vm.formValid = false;
        vm.login = login;
        vm.User = {
            UserName: "",
            Password: ""
        }

        authService.logOut();

        function login() {

            vm.message = '';
            vm.formValid = common.validateForm($scope.loginForm);

            if (vm.formValid) {

                authService.login(vm.User).then(function (response) {

                    $state.go('prontuario');

                },
                function (err) {
                    if (angular.isDefined(err.error_description)){
                            notification.showError(err.error_description);
                    }
                    else {
                        exception.throwEx(err);
                    }
                });
            } else {
                vm.message = 'Favor preencher os campos abaixo:';
            }
        };

    }
})();