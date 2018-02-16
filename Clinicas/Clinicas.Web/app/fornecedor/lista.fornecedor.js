(function () {
    'use strict';

    angular
        .module('app.fornecedor')
        .controller('ListarFornecedor', ListarFornecedor);

    ListarFornecedor.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function ListarFornecedor($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Cadastro .Fornecedor');

        vm.tipoBusca = 'Nome';

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addFornecedor = addFornecedor;
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
                .listarFornecedores()
                .then(function (result) {
                    vm.fornecedores = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addFornecedor(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/fornecedor/crud.fornecedor.html',
                controller: 'FornecedorCrud as vm',
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
            vm.askOptions = { Title: 'Excluir Fornecedor', Text: 'Tem Certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    cadastroservice.excluirFornecedorById(id).then(function (result) {
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

               /*  var codigo = 0;
                var nome = "";
                if (vm.tipoBusca == "Codigo") {
                    codigo = vm.pesq;
                } else if (vm.tipoBusca == "Nome") {
                    nome = vm.pesq;
                }
                 */

                if (vm.pesq.Nome == undefined) {
                    vm.pesq.Nome = "";
                }


                cadastroservice
                   .pesquisarFornecedores(vm.pesq.Nome, vm.pesq.Codigo)
                   .then(function (result) {
                           vm.fornecedores = result.data;
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