(function () {

    'use strict';
    angular
        .module('app')
        .controller('appController', appController);

    appController.$inject = ['$rootScope', '$scope', '$state', '$timeout', 'authService', 'ds.session'];

    function appController($rootScope, $scope, $state, $timeout, authService, dsSession) {

        $scope.$state = $state;

        //Roles/Profile
        authService.fillAuthData();
        var roles = authService.authentication.Roles;

        //Armazenamento de variáveis de escopo
        $rootScope.operador = dsSession.getOperador();
        $rootScope.usuario = dsSession.getUsuario();

    }


})();