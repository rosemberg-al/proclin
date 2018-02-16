(function () {
    'use strict';

    angular
        .module('app')
        .controller('CadastroUsuario', CadastroUsuario);

    CadastroUsuario.$inject = ['$scope', 'common', '$modalInstance', 'blockUI', 'notification', 'exception', '$state', 'ds.beneficiario', 'ds.admin', 'authService', 'beneficiario'];

    function CadastroUsuario($scope, common, $modalInstance, blockUI, notification, exception, $state, dsBeneficiario, dsAdmin, authService, beneficiario) {

        common.setBreadcrumb('Cadastro Usuário');

        var vm = this;
        vm.cancel = cancel;
        vm.saveUsuario = saveUsuario;
        $scope.forms = {};
        vm.beneficiario = beneficiario;
        vm.message = "";
        vm.formValid = true;

        //Feature Start
        init();

        function init() {

        }
    
        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function saveUsuario() {

            vm.formValid = common.validateForm($scope.forms.editForm);
            if (vm.formValid) {

                if (vm.beneficiario.Senha != vm.beneficiario.ConfirmarSenha) {
                    vm.message = "'Senha' e 'Confirma Senha' não conferem.";
                    return;
                }

                if (vm.beneficiario.Senha.length < 6 || vm.beneficiario.Senha.length > 18) {
                    vm.message = "Sua senha deve conter no mínimo 6 e no máximo 18 caracteres. ";
                    return;
                }

                var blocker = blockUI.instances.get('blockModal');
                blocker.start();

                var model = {
                    User: {
                        UserName: vm.beneficiario.Carteira,
                        Password: vm.beneficiario.Senha,
                        Name: vm.beneficiario.Nome,
                        Mobile: vm.beneficiario.Telefone1,
                        Email: vm.beneficiario.Email,
                        DataNascimento: vm.beneficiario.DataNascimento,
                    },
                    Modulo: 'PORTAL_BENEFICIARIO'
                };

                dsAdmin
                    .adicionarUsuario(model)
                    .then(function (resultado) {

                        if (resultado) {

                            var user = {
                                UserName: model.User.UserName,
                                Password: model.User.Password
                            }
                            
                            notification.showSuccess("Sua conta foi associada com sucesso.");

                            authService.login(user).then(function (response) {
                                //salvar no solus
                                dsBeneficiario
                                    .atualizaContatoSolus(vm.beneficiario)
                                    .then(function (resultado) {
                                        if (resultado == null) {
                                            vm.message = "Não foi possível atualizar seu e-mail no sistema Legado. Favor entrar em contato com o Suporte TI.";
                                        }
                                    });

                                $state.go('pagina-inicial');
                            }).catch(function (ex) {
                                exception.throwEx(ex);
                            })['finally'](function () {
                                $modalInstance.dismiss('cancel');
                                blocker.stop();
                            });
                        }

                    }).catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });

            } else {
                vm.message = "Os campos marcados em vermelho devem ser preenchidos";
            }
        }
    }
})();


angular.module('app')  
  .directive('passwordStrength', [
    function () {
        return {
            require: 'ngModel',
            restrict: 'E',
            scope: {
                password: '=ngModel'
            },

            link: function (scope, elem, attrs, ctrl) {
                scope.$watch('password', function (newVal) {

                    scope.strength = isSatisfied(newVal && newVal.length >= 8) +
                      isSatisfied(newVal && /[A-z]/.test(newVal)) +
                      isSatisfied(newVal && /(?=.*\W)/.test(newVal)) +
                      isSatisfied(newVal && /\d/.test(newVal));

                    function isSatisfied(criteria) {
                        return criteria ? 1 : 0;
                    }
                }, true);
            },
            template: '<div class="progress">' +
              '<div class="progress-bar progress-bar-danger" style="width: {{strength >= 1 ? 25 : 0}}%"></div>' +
              '<div class="progress-bar progress-bar-warning" style="width: {{strength >= 2 ? 25 : 0}}%"></div>' +
              '<div class="progress-bar progress-bar-warning" style="width: {{strength >= 3 ? 25 : 0}}%"></div>' +
              '<div class="progress-bar progress-bar-success" style="width: {{strength >= 4 ? 25 : 0}}%"></div>' +
              '</div>'
        }
    }
  ])
  .directive('patternValidator', [
    function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elem, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {

                    var patt = new RegExp(attrs.patternValidator);

                    var isValid = patt.test(viewValue);

                    ctrl.$setValidity('passwordPattern', isValid);

                    // angular does this with all validators -> return isValid ? viewValue : undefined;
                    // But it means that the ng-model will have a value of undefined
                    // So just return viewValue!
                    return viewValue;

                });
            }
        };
    }
  ]);
