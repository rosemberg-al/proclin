(function () {
    'use strict';

    angular
        .module('app.cid')
        .controller('ListaCid', ListaCid);

    ListaCid.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'common', 'notification', 'cadastroservice', '$stateParams'];

    function ListaCid($scope, $http, $q, $modal, blockUI, common, notification, cadastroservice, $stateParams) {

        var vm = this;

        common.setBreadcrumb('Pesquisa .CID');

        //Funções
        vm.init = init;
        vm.addCid = addCid;
        vm.buscar = buscar;

        //Feature Start
        init();

        //Implementations
        function init() {
            
            var blocker = blockUI.instances.get('blockListaCid');
            blocker.start();

            vm.FormMessage = "";

            cadastroservice
                .getAllCid()
                .then(function (result) {
                    vm.cids = result.data;
                })
                .catch(function (ex) {
                    
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
                cadastroservice
                   .getCidPorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
                           vm.cids = result.data;
                   })
                    .catch(function (ex) {
                    })['finally'](function () {
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