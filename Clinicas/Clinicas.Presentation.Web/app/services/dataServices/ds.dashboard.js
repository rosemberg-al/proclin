(function () {
    'use strict';

    angular.module('app.dataServices')
           .factory('ds.dashboard', DsDashboard);

    DsDashboard.$inject = ['server', 'common', 'appConfig'];

    function DsDashboard(server, common, appConfig) {

        var apiRoute = common.makeApiRoute('dashboard');
        var service = {
           
        };

        return service;

        
    }
})();
