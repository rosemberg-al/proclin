(function () {
	'use strict';

	var appDashboard = angular.module('app.dashboard', []); // 'angular-loading-bar', 'app.config'

	appDashboard.config(["$stateProvider", function ($stateProvider) { // appConfig // "appConfig"

		$stateProvider
		  .state("dashboard", {
		      parent: 'app',
		      url: "/dashboard",
		      templateUrl: "app/dashboard/dashboard.html",
		      controller: 'Dashboard as vm'
		  });
	}]);

})();