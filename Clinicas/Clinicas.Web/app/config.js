
function config($httpProvider, $stateProvider, $urlRouterProvider, $locationProvider, $ocLazyLoadProvider) {

    // Configure Idle settings
    // IdleProvider.idle(5); // in seconds
    // IdleProvider.timeout(120); // in seconds


    $httpProvider.defaults.useXDomain = true;
    $httpProvider.interceptors.push('authInterceptorService');
    delete $httpProvider.defaults.headers.common['X-Requested-With'];

    $ocLazyLoadProvider.config({
        // Set to true if you want to see what and when is dynamically loaded
        debug: false
    });

  

    $stateProvider.state('principal', {
        url: "/principal",
        templateUrl: "views/principal.html"

    }).state('app', {
        url: '',
        abstract: true,
        templateUrl: 'layout.html'
    });



    $urlRouterProvider.otherwise("login");


}
angular
    .module('app')
    .config(config)
    .run(function ($rootScope,$location, $state, $timeout) {

        $rootScope.$state = $state;
        


        NProgress.configure({
            minimum: 0.2,
            trickleRate: 0.1,
            trickleSpeed: 200
        });

        // Carregando.....

        $rootScope.$on('$viewContentLoaded', function (event) {
            $timeout(function () {
                // NProgress Start
                $('body').addClass('cui-page-loading-state');
                NProgress.start();

                // Set to default (show) state left and top menu, remove single page classes
                $('body').removeClass('single-page single-page-inverse');
                $rootScope.hideLeftMenu = false;
                $rootScope.hideTopMenu = false;
                $rootScope.showFooter = true;

                // Firefox issue: scroll top when page load
                $('html, body').scrollTop(0);

                //// Set active state menu after success change route
                //$('.left-menu-list-active').removeClass('left-menu-list-active');
                //$('nav.left-menu .left-menu-list-root .left-menu-link').each(function () {
                //    if ($(this).attr('href') == '#' + $location.path()) {
                //        $(this).closest('.left-menu-list-root > li').addClass('left-menu-list-active');
                //    }
                //});

                // NProgress End
                setTimeout(function () {
                    NProgress.done();
                }, 1000);
                $('body').removeClass('cui-page-loading-state');






                //$('#owl1').owlCarousel({
                //    loop: true,
                //    margin: 10,
                //    responsive: {
                //        0: {
                //            items: 1
                //        },
                //        600: {
                //            items: 2
                //        },
                //        1000: {
                //            items: 4
                //        }
                //    }
                //});





            }, 0);
        });
       
    });



(function () {
    'use strict';

    angular.module('app')
           .factory('authInterceptorService', authInterceptorService);

    authInterceptorService.$inject = ['$q', '$location','$injector', 'localStorageService'];

    function authInterceptorService($q, $location,$injector, localStorageService) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorage.getItem('apptoken');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData;
            }
            return config; 
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                var stateService = $injector.get('$state');
                stateService.go('login');
                //$location.path('/#/login');

            }
            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }
})();


(function () {
    'use strict';

    angular
        .module('app')
        .controller('Main', Main)

    Main.$inject = ['$scope','$state', 'localStorageService','segurancaservice'];

    function Main($scope, $state, localStorageService, segurancaservice) {

        var vm = this;
        vm.user = {};

        //Funções
        vm.init = init;
        vm.sair = sair;
        vm.buscar = buscar;

        init();
      
        //Implementations
        function init() {

            segurancaservice
                .obterUsuarioPorLogin(localStorage.getItem('userName'))
                .then(function (result) {
                    vm.user.Nome = result.data.Nome;
                    vm.user.Clinica = result.data.NmClinica;
                    vm.user.NmUnidadeAtendimento = result.data.NmUnidadeAtendimento;
                });

            segurancaservice
                .getMenu(1)
                .then(function (result) {
                    vm.menu = result.data;
                });
        }

        function buscar(){
           $state.go("busca", {"param": vm.busca }); 
        }

        function sair() {
            localStorageService.clearAll();
            localStorage.removeItem('userName');
            localStorage.removeItem('apptoken');
            $state.go('login');
        }
    }
})();
