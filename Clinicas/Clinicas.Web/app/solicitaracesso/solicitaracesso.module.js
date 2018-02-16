(function () {
	'use strict';

	var appSolicitarAcesso = angular.module('app.solicitaracesso', []); // 'angular-loading-bar', 'app.config'

	appSolicitarAcesso.config(["$stateProvider", function ($stateProvider) { // appConfig // "appConfig"

		$stateProvider
		  .state("solicitaracesso", {
		  //	parent: 'app',
		  	url: "/solicitaracesso",
		  	templateUrl: "solicitaracesso.html",
		  	controller: 'SolicitarAcesso as vm'
		  });
	}]);

})();