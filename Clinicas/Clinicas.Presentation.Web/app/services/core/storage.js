(function () {
    'use strict';

    angular.module('app.core')
           .factory('storage', storage);

    storage.$inject = ['appConfig', 'logger', 'notification', 'localStorageService'];

    function storage(appConfig, logger, notification, localStorageService) {

        var service = {
            set: set,
            get: get,
            remove: remove,
            clearAll: clearAll
        };

        return service;

        function set(key, value) {            
            localStorageService.set(key, value);
        }

        function get(key) {
            return localStorageService.get(key);
        }

        function remove(key) {
            localStorageService.remove(key);
        }

        function clearAll() {
            localStorageService.clearAll();
        }
    }
})();
