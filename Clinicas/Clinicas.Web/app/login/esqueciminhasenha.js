(function () {
    'use strict';

    angular
        .module('app.login')
        .controller('RecuperarSenha', RecuperarSenha)

    RecuperarSenha.$inject = ['$scope', '$state', 'blockUI', '$modal', '$modalInstance', '$q', '$timeout', '$http', 'notification', 'segurancaservice'];

    function RecuperarSenha($scope, $state, blockUI, $modal, $modalInstance, $q, $timeout, $http, notification, segurancaservice) {

        var vm = this;
        vm.login = "";
        $scope.forms = {};

        //Funções
        vm.init = init;
        vm.enviar = enviar;
        vm.cancel = cancel;

        init();
        //Implementations
        function init() {

        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }


        function enviar() {
            $scope.showErrorsCheckValidity = true;
            if (!$scope.forms.esqueci.$valid) {
                vm.FormMessage = "Informe seu email ";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModal');
                blocker.start();
                segurancaservice
                    .validalogin(vm.login)
                    .then(function (result) {
                        if (result.data) {
                            vm.FormMessage = "";
                            segurancaservice
                                .resetarSenha(vm.login)
                                .then(function (result) {
                                    notification.showSuccessBar("Sua nova senha foi enviada para " + vm.login + " com sucesso ");
                                    $modalInstance.dismiss('cancel');
                                })
                                .catch(function (ex) {
                                    notification.showError(ex.data.Message);
                                })['finally'](function () {
                                    blocker.stop();
                                });
                        }
                        else {
                            vm.FormMessage = "Este login não existe.";
                        }
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }
    }
})();