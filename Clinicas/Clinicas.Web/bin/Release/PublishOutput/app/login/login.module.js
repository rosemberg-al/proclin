(function () {
	'use strict';

	var appLogin = angular.module('app.login', []); // 'angular-loading-bar', 'app.config'

	appLogin.config(["$stateProvider", function ($stateProvider) { // appConfig // "appConfig"

		$stateProvider
		  .state("login", {
		  //	parent: 'app',
		  	url: "/login",
		  	templateUrl: "login.html",
		  	controller: 'Login as vm'
		  });
	}]);

})();