(function () {
    'use strict';

    angular
        .module('app.convenio')
        .controller('UnidadeController', UnidadeController);

    UnidadeController.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'cadastroservice'];

    function UnidadeController($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Sistema .Unidades de Atendimento');

        //Funções
        vm.init = init;
        vm.add = add;
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
            var blocker = blockUI.instances.get('blockModalListaUnidades');
            blocker.start();

            cadastroservice
                .listarUnidadesAtendimento()
                .then(function (result) {
                    vm.unidades = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function excluir(id) {
            vm.askOptions = { Title: 'Excluir Unidade Atendimento', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaUnidades');
                    blocker.start();
                    cadastroservice.excluirUnidade(id).then(function (result) {
                        notification.showSuccessBar("Exclusão realizada com sucesso!");
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

        function add(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/clinica/crud.unidade.atendimento.html',
                controller: 'CrudUnidadeController as vm',
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
       
    }
})();