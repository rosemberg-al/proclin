(function () {
    'use strict';

    angular.module('app.core')
           .factory('logger', logger);

    logger.$inject = ['$http', 'appConfig'];

    function logger($http, appConfig) {

        var loggerUrl = appConfig.loggerUrl;

        var service = {
            logEx: logEx
        };

        return service;

        function logEx(message, reason) {
            $http.post(loggerUrl, {
                params: {
                    message: message,
                    reason: reason
                }
            });
        }
    }
})();
