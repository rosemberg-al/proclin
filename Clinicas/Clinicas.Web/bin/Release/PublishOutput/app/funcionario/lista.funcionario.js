(function () {
    'use strict';

    angular
        .module('app.funcionario')
        .controller('Listarfuncionario', Listarfuncionario);

    Listarfuncionario.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function Listarfuncionario($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Cadastro .Funcionário');

        vm.tipoBusca = 'Nome';

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addfuncionario = addfuncionario;
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

            cadastroservice
                .listarFuncionarios()
                .then(function (result) {
                    vm.funcionarioes = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addfuncionario(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/funcionario/crud.funcionario.html',
                controller: 'funcionarioCrud as vm',
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

        function excluir(id) {
            vm.askOptions = { Title: 'Excluir Funcionário', Text: 'Tem Certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    cadastroservice.excluirFuncionarioById(id).then(function (result) {
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