(function () {
    'use strict';

    angular.module('app.core')
           .factory('server', server);

    server.$inject = ['$q', '$http', 'exception'];

    function server($q, $http, exception) {

        /*
        * Service
        */
        var service = {
            post: post,
            get: get
        };

        return service;

        /*
        * Chamadas GET e POST padrão
        */

        function post(url, data) {
            return callServer({
                url: url,
                method: 'POST',
                data: data
            });
        }

        function get(url, data) {
            return callServer({
                url: url,
                method: 'GET',
                data: data
            });
        }

        function callServer(options) {
            var url = options.url;
            var deferred = $q.defer();
            var data = options.data || null;

            if (options.method.toUpperCase() == 'GET') {
                var promise = $http.get(url, { params: data });
            }
            else {
                var promise = $http.post(url, data);
            }

            promise
                .then(function (response) {
                    deferred.resolve(response.data);
                }).catch(function (error) {
                    exception.throwEx(error);
                    return deferred.reject(error);
                })

            return deferred.promise;
        }
    }
})();
