(function () {
    'use strict';

    angular
        .module('app.users')
        .controller('EditUser', EditUser);

    EditUser.$inject = ['$scope', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.users', 'user'];

    function EditUser($scope, $q, $modal, $modalInstance, blockUI, common, notification, exception, DsUsers, user) {

        //Variáveis de escopo
        var vm = this;
        $scope.forms = {};
        vm.FormValid = false;
        vm.FormMessage = '';
        vm.usuarioSemPerfil = true;
        vm.usuarioSemFuncionalidades = true;
        vm.usarCriptografia = true;
        vm.usarSenhaPadrao = false;
        vm.emailExistente = null;
        
        //Entity Model
        if (angular.isUndefined(user)) {
            vm.State = 'Novo Usuário';
            vm.User = {
                Id: 0,
                StatusValue: 1,
                LevelValue: 1,
                UserFeatures: [],
                Levels: [],
                Roles: [],
                UsarCriptografia: true,
                UsarSenhaPadrao: false
            };

        }
        else {
            vm.User = user;
            vm.User.UsarCriptografia = false;
            vm.User.UsarSenhaPadrao = false;

            vm.State = 'Editar Usuário';
            var level = _.find(user.Levels, { Key: vm.User.LevelValue });
            vm.LevelSelecionado = level;

        }

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Inicialização
        init();

        //Implementations
        function init() {

            vm.Levels = [
                { value: "Usuário de sistema", key: "1" },
                { value: "Usuário administrador", key: "4" }
            ];
        }

        function sortFeatures() {
            var array = _.sortBy(vm.Modulo.Features, 'Ordenacao');
        }


        function resetPassword() {
            DsUsers
                .resetPassword(vm.User)
                .then(function (result) {
                    $modalInstance.close(result.data);
                    notification.showSuccess("A senha do usuário foi resetada com sucesso.");
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blocker.stop();
                });
        }


        function save() {

            vm.FormMessage = '';
            var formValid = common.validateForm($scope.forms.editForm);

            if (formValid) {

                if (vm.User.Password != vm.User.PasswordConfirm) {
                    vm.FormMessage = "As senhas digitadas não conferem, tente novamente.";
                    return;
                }

                vm.User.LevelValue = vm.LevelSelecionado.Key;

                //Salva o modulo
                var blocker = blockUI.instances.get('blockModal');
                blocker.start();

                DsUsers
                    .save(vm.User)
                    .then(function (result) {
                        $modalInstance.close(result.data);
                        notification.showSuccess("Usuário salvo com sucesso");
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });
            } else
                vm.FormMessage = 'Por favor, preencha os campos em vermelho';
        };

        function cancel() {
            $modalInstance.dismiss('cancel');
        };

        ///////////////////////
        ///Password Generator
        var keylist = "abcdefghijkmnopqrstuvwxyz23456789";
        var tempPwd = '';
        var plength = 6;

        function generatePass() {
            tempPwd = '';
            for (var i = 0; i < plength; i++)
                tempPwd += keylist.charAt(Math.floor(Math.random() * keylist.length));
            vm.User.Password = tempPwd;
        }

        function verificaEmail()
        {
            //Verificar se o e-mail já existe
            DsUsers
                .checkUserEmail(vm.User.Email)
                .then(function (result) {
                    vm.emailExistente = result.data;
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    //blocker.stop();
                });
        }

        //function addFeature() {
        //    var modalInstance = $modal.open({
        //        templateUrl: 'app/modulos/edit.feature.html',
        //        controller: 'EditFeature as vm',
        //        windowClass: 'md-modal-window',
        //        resolve: {
        //            pais: function () {
        //                return vm.Modulo.Features;
        //            },
        //            feature: null
        //        }
        //    });

        //    modalInstance.result.then(function () {
        //        init();
        //    });

        //}
    }
})();
