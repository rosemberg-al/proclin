(function () {
    'use strict';

    angular
        .module('app.cid')
        .controller('ListaCid', ListaCid);

    ListaCid.$inject = ['$scope', '$http', '$q', 'blockUI', 'common', '$modal', 'exception', 'ds.cadastros', '$stateParams'];

    function ListaCid($scope, $http, $q, blockUI, common, $modal, exception, dsCadastro, $stateParams) {

        common.setBreadcrumb('CID');
        var vm = this;

        common.setBreadcrumb('pagina-inicial .cadastro .cid');

        //Funções
        vm.init = init;
        vm.addCid = addCid;
        vm.buscar = buscar;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListaCids');
            blocker.start();

            dsCadastro
                .getAllCid()
                .then(function (result) {
                    vm.cids = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaCids');
                blocker.start();
                dsCadastro
                   .getCidPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.cids = result.data;
                   })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function addCid(codigo) {
            var modalInstance = $modal.open({
                templateUrl: 'app/cid/crud.cid.html',
                controller: 'CidCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    codigo: function () {
                        return codigo;
                    },
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }
    }
})();