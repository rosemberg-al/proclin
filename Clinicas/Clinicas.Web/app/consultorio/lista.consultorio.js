(function () {
    'use strict';

    angular
        .module('app.consultorio')
        .controller('ListaConsultorios', ListaConsultorios);

    ListaConsultorios.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'DTInstances', 'DTOptionsBuilder', 'common', 'notification', 'cadastroservice'];

    function ListaConsultorios($scope, $http, $q, $modal, blockUI, DTInstances, DTOptionsBuilder, common, notification, cadastroservice) {

        var vm = this;
        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('Sistema .Consultório');

        //Funções
        vm.init = init;
        vm.addConsultorio = addConsultorio;
        vm.excluirConsultorio = excluirConsultorio;


        init();

        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('block');
            blocker.start();

              cadastroservice
                .listarConsultorios()
                .then(function (result) {
                    vm.consultorios = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function addConsultorio(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/consultorio/crud.consultorio.html',
                controller: 'CrudConsultorio as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function excluirConsultorio(id) {
            vm.askOptions = { Title: 'Excluir Consultório', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('block');
                    blocker.start();
                    cadastroservice.excluirConsultorio(id).then(function (result) {
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
    }
})();