(function () {
    'use strict';

    angular
        .module('app')
        .controller('Dashboard', Dashboard)

    Dashboard.$inject = ['$scope', '$state', '$injector', 'blockUI', '$q', '$modal', '$http', 'notification', 'localStorageService', 'servicebase', 'ds.session', 'segurancaservice', 'dashboardservice'];

    function Dashboard($scope, $state, $injector, blockUI, $q, $modal, $http, notification, localStorageService, servicebase, dsSession, segurancaservice, dashboardservice) {

        var vm = this;

        //Funções
        vm.init = init;
    
        init();
        //Implementations
        function init() {
            //vm.TotalPagar = dashboardservice.getTotalPagar();

            //verifica se o usuário é primeiro acesso
            /*segurancaservice
                .obterUsuarioPorLogin(localStorage.getItem('userName'))
                .then(function (result) {
                    dsSession.setUsuario(result.data);
                    if (result.data.PrimeiroAcesso) {
                        var modalInstance = $modal.open({
                            templateUrl: 'app/primeiroacesso/primeiroacesso.html',
                            controller: 'PrimeiroAcesso as vm',
                            size: 'lg',
                            backdrop: 'static',
                            keyboard: false,
                            resolve: {
                                usuario: function () {
                                    return result.data;
                                },
                            }
                        });

                        modalInstance.result.then(function () {
                            init();
                        });
                    }

                })
                .catch(function (ex) {
                    //notification.showError(ex.data.Message);
                })['finally'](function () {
                });*/

        }
    }
})();