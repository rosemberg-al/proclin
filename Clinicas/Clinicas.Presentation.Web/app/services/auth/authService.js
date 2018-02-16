/**
*
*  Serviço de Autenticação
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .factory('authService', authService);

    authService.$inject = ['$rootScope', '$http', '$q', 'localStorageService', 'ds.session', 'common', 'server', '$location', 'appConfig'];

    function authService($rootScope, $http, $q, localStorageService, dsSession, common, server, $location, appConfig) {

        var serviceBase = appConfig.authUrl;
        var _authentication = {
            isAuth: false,
            UserName: "",
            Name: "",
            Roles: undefined,
        };

        var _confirmAccount = function (account) {

            return $http.post(serviceBase + '/seguranca/resetarsenha', account);
        }

        var _checkUser = function (userName) {
            return $http.get(serviceBase + '/seguranca/usuariovalido/', {
                params: { userName: userName }
            });
        }

        var _signUp = function (registration) {
            _logOut();
            return $http.post(serviceBase + '/seguranca/signup', registration);

        };

        var _login = function (user) {

            var data = "grant_type=password&username=" + user.UserName + "&password=" + user.Password + "&module=" + appConfig.idModulo;
            var deferred = $q.defer();

            $http.post(serviceBase + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                var rolesObj = undefined;
                if (response.roles)
                    rolesObj = angular.fromJson(response.roles);

                _authentication.Roles = rolesObj;
                localStorageService.set('authorizationData', {
                    token: response.access_token,
                    userName: user.UserName,
                    name: response.name,
                    roles: rolesObj
                });

                _authentication.isAuth = true;
                _authentication.UserName = user.UserName;
                _authentication.Name = response.name;

                dsSession.setUsuario(_authentication);
                dsSession.setOperador(response.id_user);

                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });

            return deferred.promise;

        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');
            $rootScope.userRoles = undefined;
            $rootScope.User = undefined;

            _authentication.isAuth = false;
            _authentication.UserName = "";
            _authentication.Name = "";

            dsSession.limparSessao();
        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.UserName = authData.userName;
                _authentication.Name = authData.name;
                _authentication.Roles = authData.roles;
                dsSession.setUsuario(_authentication);
            
            }
        }

        function _forgotPassword(user) {
            user.UrlAtivacao = $location.absUrl().replace($location.path(), '/reset');
            var url = serviceBase + '/seguranca/enviarnovasenha ';
            return server.post(url, user);


        }
        
        function _changePassword(senha) {
            var url = serviceBase + '/seguranca/alterarsenha';
            return server.post(url, senha);
        }

        var authServiceFactory = {
            signUp: _signUp,
            login: _login,
            logOut: _logOut,
            fillAuthData: _fillAuthData,
            authentication: _authentication,
            confirmAccount: _confirmAccount,
            forgotPassword: _forgotPassword,
            changePassword: _changePassword,
            checkUser: _checkUser
        };

        return authServiceFactory;

    }
})();