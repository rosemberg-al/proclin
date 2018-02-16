(function () {
    'use strict';

    angular
        .module('app.usuario')
        .controller('UsuarioCrud', UsuarioCrud);

    UsuarioCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'segurancaservice', 'id'];

    function UsuarioCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, comumservice,segurancaservice, id) {

        var vm = this;
        vm.State = "Incluir Usuário";
        vm.FormMessage = "";
  

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            var blocker = blockUI.instances.get('blockModalUsuario');
            blocker.start();

            cadastroservice
             .getDadosClinica()
                .then(function (result) {
                vm.unidades = result.data.Unidades;
            })
            .catch(function (ex) {
                notification.showError(ex.data.Message);
                })['finally'](function () {
                blocker.stop();
            });

            segurancaservice
             .listarGrupoUsuario()
                .then(function (result) {
                vm.grupos = result.data;
            })
            .catch(function (ex) {
                notification.showError(ex.data.Message);
                })['finally'](function () {
                blocker.stop();
            });

            if(id>0){
                vm.State = "Editar Usuário";
                
                var blocker = blockUI.instances.get('blockModalUsuario');
                blocker.start();
                segurancaservice
                     .obterUsuarioPorId(id)
                        .then(function (result) {
                         vm.usuario = result.data;
                         vm.grupoUsuarioSelecionado = vm.usuario.IdGrupoUsuario;
                         vm.unidadeSelecionada = vm.usuario.IdUnidadeAtendimento;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                        })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dadosusuario.$valid) {

                var blocker = blockUI.instances.get('blockModalUsuario');
                blocker.start();

                vm.usuario.IdGrupoUsuario = vm.grupoUsuarioSelecionado;
                vm.usuario.IdUnidadeAtendimento = vm.unidadeSelecionada;

                segurancaservice
                    .salvarUsuario(vm.usuario)
                    .then(function (result) {
    
                        vm.usuario = result.data;

                        if(vm.usuario.IdUsuario>0){
                            notification.showSuccess("Alteração realizada com sucesso");
                        }else{
                            notification.showSuccess("Cadastro realizado com sucesso");
                        }      
                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        vm.FormMessage= ex.data.Message;
                        //notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });

            }else {
                vm.FormMessage = "Existem campos obrigatórios sem devido preenchimento.";
            }

        }

    }
})();