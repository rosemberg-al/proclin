(function () {
    'use strict';

    angular
        .module('app.modeloprontuario')
        .controller('ModeloProntuario', ModeloProntuario);

    ModeloProntuario.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', 'notification', '$modal', 'prontuarioservice', '$stateParams'];

    function ModeloProntuario($scope, $http, $q, blockUI, common, notification, $modal, prontuarioservice, $stateParams) {

        common.setBreadcrumb('Cadastro .Modelo de Prontuário');
        var vm = this;

        //Funções
        vm.init = init;
        vm.addModelo = addModelo;
        vm.excluirModelo = excluirModelo;
        vm.buscar = buscar;

        //Feature Start
        init();

        vm.pesq = {};

        //Implementations
        function init() {

            vm.pesq = {};

            var blocker = blockUI.instances.get('blockModalListaModelo');
            blocker.start();

            prontuarioservice
                    .getModelos()
                    .then(function (result) {
                        vm.modelos = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
        }

        function addModelo(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/modeloprontuario/crud.modelo.prontuario.html',
                controller: 'ControllerModeloCrud as vm',
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

        function excluirModelo(id) {
            vm.askOptions = { Title: 'Excluir Modelo', Text: 'Tem Certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    prontuarioservice.excluirModelo(id).then(function (result) {
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

                prontuarioservice
                   .pesquisarModelos(vm.pesq.Nome)
                   .then(function (result) {
                       vm.modelos = result.data;
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