/**
*
*  Controller Login
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .controller('signUpController', signUpController);

    signUpController.$inject = ['$scope', '$state', '$modal', 'blockUI','common', 'notification', 'authService','ds.admin'];

    function signUpController($scope, $state, $modal, blockUI, common, notification, authService,  dsAdmin) {
        var vm = this;
        vm.message = '';
        vm.success = '';
        vm.formValid = false;
        vm.showForm = true;

        init();

        function init() {
            vm.message = "";
        }

        vm.signUp = function () {


            vm.formValid = common.validateForm($scope.signupForm);
            if (vm.formValid) {

             

                if (vm.beneficiario.Senha != vm.beneficiario.ConfirmarSenha) {
                    vm.message = " Atenção: Senhas não conferem.";
                    return;
                }

                if (vm.beneficiario.Senha.length < 6) {
                    vm.message = "Sua senha deve conter no minimo 6 caracteres. ";
                    return;
                }

                var blocker = blockUI.instances.get('blockModal');
                blocker.start();


                var model = {
                    UserName: vm.beneficiario.Carteira,
                    Password: vm.beneficiario.Senha,
                    Name: vm.beneficiario.Nome,
                    Mobile: vm.beneficiario.Telefone1,
                    Email: vm.beneficiario.Email,
                    UsarCriptografia: true,
                    UsarSenhaPadrao: false,
                    LevelValue: 5,
                    StatusValue: 1
                };

                var obj = {
                    User: model,
                    Modulo: 105
                };

                dsAdmin
                    .adicionarUsuario(obj)
                    .then(function (resultado) {
                        if (resultado) {
                            notification.showSuccess("Usuário cadastrado com sucesso. Realize seu primeiro acesso");

                            $state.go('login');
                        }
                    }, function errorCallback(response) {
                        notification.showError(response.data);
                    }).finally(function (res) {
                        blocker.stop();
                    }).catch(function (res) {
                        notification.showError(res.statusText);
                    });
            } else {
                vm.message = "Existem campos obrigatórios sem o devido preenchimento";
            }
        };
    }
})();
