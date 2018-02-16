(function () {
    'use strict';

    angular
        .module('app.usuario')
        .controller('AlterarSenha', AlterarSenha);

    AlterarSenha.$inject = ['$scope', '$http', '$q', 'FileUploader', 'blockUI', 'servicebase', 'common', 'notification', '$modal', 'segurancaservice', 'comumservice'];

    function AlterarSenha($scope, $http, $q, FileUploader, blockUI, servicebase, common, notification, $modal, segurancaservice, comumservice) {

        var vm = this;

        common.setBreadcrumb('Sistema .Alterar Senha');
        $scope.forms = {};

        //Funções
        vm.init = init;
        vm.save = save;

        init();

        function init() { }
        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dadosusuario.$valid) {

                var blocker = blockUI.instances.get('blockModalUsuario');
                blocker.start();

                segurancaservice
                    .alterarSenha(vm.usuario.NovaSenha,vm.usuario.Confirmar)
                    .then(function (result) {
                      notification.showSuccessBar("Alteração realizada com sucesso");
                      vm.usuario.NovaSenha = "";
                      vm.usuario.Confirmar = "";
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.data.Message;
                    })['finally'](function () {
                        blocker.stop();
                    });

            }else {
                vm.FormMessage = "Existem campos obrigatórios sem devido preenchimento.";
            }
        }
    }
})();