(function () {
    'use strict';

    angular
        .module('app')
        .controller('SolicitarAcesso', SolicitarAcesso)

    SolicitarAcesso.$inject = ['$scope', '$state', '$injector', 'blockUI', '$q', '$timeout', '$http', 'notification', 'localStorageService', 'servicebase', 'ds.session', 'comumservice', 'segurancaservice'];

    function SolicitarAcesso($scope, $state, $injector, blockUI, $q, $timeout, $http, notification, localStorageService, servicebase, dsSession, comumservice, segurancaservice) {

        var vm = this;
        vm.solicitar = {};
        $scope.forms = {};

        //Funções
        vm.init = init;
        vm.solicitarAcesso = solicitarAcesso;


        init();
        //Implementations
        function init() {
            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

        var pEspecialidades = segurancaservice.getAllEspecialidades();
            pEspecialidades.then(function (result) {
                vm.especialidades = result.data;
            });
        }

        $scope.$watch('vm.solicitar.IdEstado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue);
            }
        });

        function buscarCidadesByEstado(idEstado) {
            comumservice
                .getCidadesByEstado(idEstado)
                .then(function (result) {
                    vm.cidades = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });
        }

        function solicitarAcesso() {

            $scope.showErrorsCheckValidity = true;
            var form = $scope.forms.acesso.$valid;

            if (form) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockAcesso');
                blocker.start();
                
                segurancaservice
                    .solicitaracesso(vm.solicitar)
                    .then(function (result) {
                        notification.showSuccessBar("Seu acesso foi criado com sucesso, seus dados foram enviados para seu e-mail.");
                        var stateService = $injector.get('$state');
                        stateService.go('login');
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.data.Message;


                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Todos os campos são obrigatórios.";
            }
        }
    }
})();