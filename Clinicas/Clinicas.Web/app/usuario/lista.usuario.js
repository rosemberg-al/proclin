(function () {
    'use strict';

    angular
        .module('app.funcionario')
        .controller('ListarUsuario', ListarUsuario);

    ListarUsuario.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice', 'segurancaservice'];

    function ListarUsuario($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice,segurancaservice) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Sistema .Usuário');

        vm.tipoBusca = 'Nome';

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addUsuario = addUsuario;
        vm.desativar = desativar;
        vm.resetar = resetar;
        vm.ativar = ativar;
        vm.excluir = excluir;


        //Feature Start
        init();

        //Implementations
        function init() {
            vm.pesq = {};

            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListaFor');
            blocker.start();

            segurancaservice
                .listarUsuarios()
                .then(function (result) {
                    vm.usuarios = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addUsuario(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/usuario/crud.usuario.html',
                controller: 'UsuarioCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }


        function desativar(id) {
            
            vm.askOptions = { Title: 'Desativar Usuário ', Text: 'Tem certeza que deseja desativar o usuário selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                
                if (confirm) {
                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    segurancaservice.desativarUsuario(id).then(function (result) {
                        notification.showSuccessBar("Alteração realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }

        function resetar(login) {
            vm.askOptions = { Title: 'Resetar Senha Usuário ', Text: 'Tem certeza que deseja resetar a senha do usuário selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                
                if (confirm) {
                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    segurancaservice.resetarSenha(login).then(function (result) {
                        notification.showSuccessBar("Alteração realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }

        function ativar(id) {
            
            vm.askOptions = { Title: 'Ativar Usuário ', Text: 'Tem certeza que deseja ativar o usuário selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                
                if (confirm) {
                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    segurancaservice.ativarUsuario(id).then(function (result) {
                        notification.showSuccessBar("Alteração realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }


        function excluir(id) {
            vm.askOptions = { Title: 'Excluir ', Text: 'Tem Certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    segurancaservice.excluirUsuario(id).then(function (result) {
                        notification.showSuccessBar("Exclusão realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }

        function buscar() {

            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaFor');
                blocker.start();

                if (vm.pesq.Nome == undefined) {
                    vm.pesq.Nome = "";
                }

                cadastroservice
                   .pesquisarFuncionarios(vm.pesq.Nome, vm.pesq.Codigo)
                   .then(function (result) {
                       vm.funcionarioes = result.data;
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