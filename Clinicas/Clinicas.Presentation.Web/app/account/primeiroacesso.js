(function () {
    'use strict';

    angular
        .module('app.agendamento')
        .controller('PrimeiroAcesso', PrimeiroAcesso);

    PrimeiroAcesso.$inject = ['$scope', 'common', '$modal', '$modalInstance', 'blockUI', 'notification', 'exception', '$state', 'ds.beneficiario', 'authService', 'carteira'];

    function PrimeiroAcesso($scope, common, $modal, $modalInstance, blockUI, notification, exception, $state, dsBeneficiario, authService, carteira) {

        common.setBreadcrumb('Confirmar Cancelamento');

        var vm = this;
        vm.cancel = cancel;
        vm.obterDadosPrimeiroacesso = obterDadosPrimeiroacesso;
        $scope.forms = {};
        vm.formValid = true;
        vm.message = '';
        vm.primeiroacesso = false;

        //Model
        vm.beneficiario = {
            DataDeNascimento: undefined,
            NumeroDaCarteira: carteira
        }

        //Feature Start
        init();

        function init() {
            if (angular.isUndefined(carteira))
                vm.primeiroacesso = true;
        }

        function obterDadosPrimeiroacesso() {

            vm.formValid = common.validateForm($scope.forms.dados);

            if (vm.formValid) {
               
                var blocker = blockUI.instances.get('blockModal');
                blocker.start();

                authService
                    .checkUser(vm.beneficiario.NumeroDaCarteira)
                    .then(function (checkResult) {

                        if (!checkResult.data) {

                            dsBeneficiario
                                .obterdadosprimeiroacesso(vm.beneficiario)
                                .then(function (resultado) {

                                    if (resultado != null) {
                                        $modalInstance.dismiss('cancel');
                                        // cadastro usuário
                                        var modalInstance = $modal.open({
                                            templateUrl: 'app/account/cadastroUsuario.html',
                                            controller: 'CadastroUsuario as vm',
                                            backdrop: true,
                                            size: 'lg',
                                            resolve: {
                                                beneficiario: function () {
                                                    return resultado;
                                                }
                                            }
                                        });

                                    } else {
                                        vm.message = "Seu cadastro não foi encontrado. Confira os dados digitados ou procure o setor de benefícios de sua empresa. ";
                                    }
                                });
                        }
                        else {
                            notification.showWarning("Este usuário já está cadastrado. Por favor, realize o Login utilizando sua Carteira e Senha");
                            $modalInstance.dismiss('cancel');
                        }

                    }).catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });
            } else {
                vm.message = "Por favor, preencha todos os campos abaixo";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

    }
})();
