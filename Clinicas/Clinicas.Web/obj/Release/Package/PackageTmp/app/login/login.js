(function () {
    'use strict';

    angular
        .module('app')
        .controller('Login', Login)

     Login.$inject = ['$scope', '$state', 'blockUI', '$modal', '$q',  '$timeout','$http','notification','localStorageService','servicebase', 'ds.session'];

    function Login($scope, $state, blockUI, $modal, $q, $timeout, $http,notification, localStorageService, servicebase, dsSession) {

        var vm = this;

        //Funções
        vm.init = init;
        vm.logar = logar;
        vm.esqueceuSenha = esqueceuSenha;

        init();
        //Implementations
        function init() {

            vm.user = {
                Login: "renatouai@gmail.com",
                Senha: "150709"
            };

            localStorageService.clearAll();
            localStorage.removeItem('userName');
            localStorage.removeItem('apptoken');
            localStorage.removeItem('dadosusuario');
        }

        function logar() {


            //$timeout(function () {
            //    blocker.stop();
            //}, 4000);

            if (vm.user == undefined) {
                vm.FormMessage = "Atenção: Existem campos obrigatórios sem o devido preenchimento";
                blocker.stop();
                return;
            }
            if ((vm.user.Login == "") || (vm.user.Senha == "")) {
                vm.FormMessage = "Atenção: Existem campos obrigatórios sem o devido preenchimento";
                blocker.stop();
                return;
            } else {
                var blocker = blockUI.instances.get('blockLogin');
                blocker.start();
                // servicebase.urlApi() + 
                var data = "grant_type=password&username=" + vm.user.Login + "&password=" + vm.user.Senha;

                var config = {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                    }
                }

                $http.post(servicebase.urlApi() + "/token", data, config).success(function (response) {
                    blocker.stop();
                    localStorage.setItem('apptoken', response.access_token);
                    localStorage.setItem('userName', vm.user.Login);
                   // console.log(response);

                    dsSession.setUsuario(vm.user.Login);
                    if (response.access_token != '') {
                        $state.go('dashboard');
                    }
                }).error(function (err, status) {
                    vm.FormMessage = "Usuário ou senha inválidos";
                    blocker.stop();
                });
            }
        }


        function esqueceuSenha() {
            var modalInstance = $modal.open({
                templateUrl: 'app/login/esqueciminhasenha.html',
                controller: 'RecuperarSenha as vm',
                size: 'xs',
                backdrop: 'static'
            });
            modalInstance.result.then(function () {
            });
        }

    }
})();