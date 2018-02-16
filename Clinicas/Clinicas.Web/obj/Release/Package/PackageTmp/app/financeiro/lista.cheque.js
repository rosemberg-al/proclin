(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('Cheques', Cheques);

    Cheques.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'financeiroservice', '$stateParams'];
    function Cheques($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, financeiroservice, $stateParams) {
        var vm = this;
        //vm.pesq = [];

        common.setBreadcrumb('Financeiro .Cheque');

        vm.init = init;
        vm.abrir = abrir;
        vm.excluir = excluir;
        vm.buscar = buscar;

        init();

        function init() {

            vm.dtOptions = DTOptionsBuilder
                   .newOptions()
                   .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListaCheques');
            blocker.start();

            financeiroservice
                .listarcheques()
                .then(function (result) {
                    vm.cheques = result.data;
                })
                .catch(function (ex) {
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function buscar() {
            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockListaCheques');
                blocker.start();

                if (vm.pesq.Emitente == undefined) {
                    vm.pesq.Emitente = "";
                }

                if (vm.pesq.Banco == undefined) {
                    vm.pesq.Banco = "";
                }
                financeiroservice
                    .pesquisarCheques(vm.pesq.Emitente, vm.pesq.Banco)
                    .then(function (result) {
                        vm.cheques = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function excluir(id) {

            vm.askOptions = { Title: 'Excluir', Text: 'Tem certeza que deseja excluir o registro selecionado?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    financeiroservice
                       .excluircheque(id)
                       .then(function (result) {
                           notification.showSuccessBar("Cheque excluido com sucesso!");
                           init();
                       })
                       .catch(function (ex) {
                       })['finally'](function () {
                       });
                }
            });
        }

        function abrir(id) {
            console.log(id);
            var modalInstance = $modal.open({
                templateUrl: 'app/financeiro/crud.cheque.html',
                controller: "ChequeCrud as vm",
                backdrop: 'static',
                size: 'lg',
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

    }
})();