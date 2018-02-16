/**
*
*  Controller Login
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .controller('lostPasswordController', lostPasswordController);

    lostPasswordController.$inject = ['$scope', '$state', '$modal', 'common', 'notification', 'exception', 'authService', 'blockUI', 'appConfig'];

    function lostPasswordController($scope, $state, $modal, common, notification, exception, authService, blockUI, appConfig) {

        var vm = this;
        vm.message = '';
        vm.formValid = false;
        vm.beneficiario = {
            numeroDaCarteira: '',
            dataDeNascimento: '',
            cpf: ''
        }

        vm.esqueceusenha = function () {

            vm.message = '';
            vm.formValid = common.validateForm($scope.lostPasswordForm);

            if (vm.formValid) {

                var beneficiario = {
                    Email: vm.beneficiario.email,
                    Usuario: vm.beneficiario.NumeroDaCarteira,
                    Modulo: appConfig.idModulo,
                    UrlAtivacao:"1"
                }

                var blocker = blockUI.instances.get('blockModal');
                blocker.start();

                authService.forgotPassword(beneficiario)
                    .then(function (response) {
                            notification.showSuccess(response);
                    }).catch(function (ex) {
                        //exception.throwEx(ex);

                        //if (response.status == "430") {
                        //    var modalInstance = $modal.open({
                        //        templateUrl: 'app/account/primeiroacesso.html',
                        //        controller: 'PrimeiroAcesso as vm',
                        //        backdrop: true,
                        //        size: 'lg'
                        //    });
                        //}
                    }).finally(function () {
                         blocker.stop();
                });

            } else {
                vm.message = 'Favor preencher todos os campos abaixo.';
            }
        }
    }
})();