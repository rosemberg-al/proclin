(function () {
    'use strict';

    var app = angular.module('app', [

        
         'ui.router',                    // Routing
        'oc.lazyLoad',                  // ocLazyLoad
        'ui.bootstrap',                 // Ui Bootstrap
        'pascalprecht.translate',       // Angular Translate
        'ngIdle',                       // Idle timer
        'ngSanitize',
        'app.cliente',
        'app.conta',
        'app.plano',
         'app.funcionario',
        'ngTable',
        'cp.ngConfirm',
        'LocalStorageModule',
        'blockUI',
        'brasil.filters',
        'ui.utils.masks',
        //'ui.tree',


        /*
         * App Core
         */
         'app.config',
         'app.core',
         'app.dataServices',
        /*
         * Shared Modules
         */

        /*
         * Modules
         */
        //'app.dashboard',
        'app.users',
        'app.financeiro',
        'app.anamnese',
        'app.receituario',
        'app.atendimento',
        'app.paciente',
        'app.vacinas',
        'app.atestado',
        'app.prontuario',
        'app.agenda',
        'app.historia',
        'app.hospital',
        'app.medidasantropometricas',
        'app.home',
        'app.especialidade',
        'app.procedimento',
        'app.convenio',
        'app.cid',
        'app.ocupacao',
        'app.funcionario',
        'app.fornecedor',
        'app.guias',
        'app.relatorio'
    ]);

    /*
    * Routing
    */
    app.config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {

        $stateProvider.
            state('login', {
                url: '/login',
                templateUrl: 'app/account/login.html',
                controller: 'loginController as vm',
                data: { requireAuth: false }
            }).
            state('activateAccount', {
                url: '/activate/:userId/:hashcode',
                templateUrl: 'app/account/activate.html',
                controller: 'activateAccountController as vm',
                data: { requireAuth: false }
            }).
             state('resetPassword', {
                 url: '/reset/:userId/:hashcode/:reset',
                 templateUrl: 'app/account/activate.html',
                 controller: 'activateAccountController as vm',
                 data: { requireAuth: false }
                 
             }).
            state('resendActivation', {
                url: '/resend',
                templateUrl: 'app/account/resendActivation.html',
                controller: 'resendActivationController as vm',
                data: { requireAuth: false }
            }).
            state('createAccount', {
                url: '/signup',
                templateUrl: 'app/account/signup.html',
                controller: 'signUpController as vm',
                data: { requireAuth: false }
            }).
            state('lostPassword', {
                url: '/lost',
                templateUrl: 'app/account/lostPassword.html',
                controller: 'lostPasswordController as vm',
                data: { requireAuth: false }
            }).            
            state('app', {
                url: '',
                abstract: true,
                templateUrl: 'layout.html',
                controller: 'appController'
            });

        $urlRouterProvider.otherwise(function ($injector, $location) {
            var $state = $injector.get("$state");
            $state.go("login");
        });
    }]);

    //Previne Cache (TODO:remover apos publicar)
    function configureTemplateFactory($provide) {
        // Set a suffix outside the decorator function 
        var cacheBust = Date.now().toString();

        function templateFactoryDecorator($delegate) {
            var fromUrl = angular.bind($delegate, $delegate.fromUrl);
            $delegate.fromUrl = function (url, params) {
                if (url !== null && angular.isDefined(url) && angular.isString(url)) {
                    url += (url.indexOf("?") === -1 ? "?" : "&");
                    url += "v=" + cacheBust;
                }

                return fromUrl(url, params);
            };
            return $delegate;
        }

        $provide.decorator('$templateFactory', ['$delegate', templateFactoryDecorator]);
    }

    app.config(['$provide', configureTemplateFactory]);


    /*
    * Authentication
    */
    app.run(['authService', function (authService) {
        authService.fillAuthData();
    }]);

    app.config(function ($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        $httpProvider.interceptors.push('authInterceptorService');
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
    });







    /*
    * ui-select2
    */
    app.config(function (uiSelectConfig) {
        uiSelectConfig.theme = 'bootstrap';
        uiSelectConfig.resetSearchInput = true;
    });







    /*
    * RootScope
    */
    app.constant('_', window._);
    app.run(['$rootScope', '$timeout', '$state', 'authService', function ($rootScope, $timeout, $state, authService) {

        authService.fillAuthData();

        //Evento de mudança de estado em rotas
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState) {

            var shouldLogin = toState.data !== undefined
                           && toState.data.requireAuth
                           && !authService.authentication.isAuth;

            if (shouldLogin) {
                $state.go('login');
                event.preventDefault();
                return;
            }

        });

        //Evento apos carregamento do DOM em rotas
        $rootScope.$on('$viewContentLoaded', function (event) {
            $timeout(function () {
                loadPageScripts();
            }, 0);
        });

        //Lodash
        $rootScope._ = window._;   // use in views, ng-repeat="x in _.range(3)"
    }]);







    /*
    * BlockUI
    */
    app.config(function (blockUIConfig) {

        // Change the default overlay message
        blockUIConfig.autoBlock = false;
        blockUIConfig.template = '<div class="block-ui-overlay"></div><div class="block-ui-message-container" aria-live="assertive" aria-atomic="true"><div class="block-ui-message ng-binding" ng-class="$_blockUiMessageClass"><div id="followingBallsG"><div id="followingBallsG_1" class="followingBallsG"></div><div id="followingBallsG_2" class="followingBallsG"></div><div id="followingBallsG_3" class="followingBallsG"></div><div id="followingBallsG_4" class="followingBallsG"></div></div></div></div>';
    });

    /*
    * Overrides TPLS ui-bootstrap
    */

    //Tabs
    angular.module("template/tabs/tabset.html", []).run(["$templateCache", function ($templateCache) {
        $templateCache.put("template/tabs/tabset.html",
          "<div>\n" +
          "  <ul class=\"nav nav-{{type || 'tabs'}} nav-tabs-simple\" ng-class=\"{'nav-stacked': vertical, 'nav-justified': justified}\" ng-transclude></ul>\n" +
          "  <div class=\"tab-content\">\n" +
          "    <div class=\"tab-pane\" \n" +
          "         ng-repeat=\"tab in tabs\" \n" +
          "         ng-class=\"{active: tab.active}\"\n" +
          "         tab-content-transclude=\"tab\">\n" +
          "    </div>\n" +
          "  </div>\n" +
          "</div>\n" +
          "");
    }]);

    //Accordion
    angular.module("template/accordion/accordion-group.html", []).run(["$templateCache", function ($templateCache) {
        $templateCache.put("template/accordion/accordion-group.html",
          "<div class=\"panel panel-default\">\n" +
          "  <div class=\"panel-heading {{hclass}}\">\n" +
          "    <h4 class=\"panel-title\">\n" +
          "      <a href ng-class=\"{'collapsed': !isOpen}\" class=\"accordion-toggle\" ng-click=\"toggleOpen()\" accordion-transclude=\"heading\"><span ng-class=\"{'text-muted': isDisabled}\">{{heading}}</span></a>\n" +
          "    </h4>\n" +
          "  </div>\n" +
          "  <div class=\"panel-collapse\" collapse=\"!isOpen\">\n" +
          "	  <div class=\"panel-body\" ng-transclude></div>\n" +
          "  </div>\n" +
          "</div>\n" +
          "");
    }]);

    //Modal
    angular.module("template/modal/backdrop.html", []).run(["$templateCache", function ($templateCache) {
        $templateCache.put("template/modal/backdrop.html",
          "<div class=\"modal-backdrop fade {{ backdropClass }}\"\n" +
          "     ng-class=\"{in: animate}\"\n" +
          "     ng-style=\"{'z-index': 11000 + (index && 1 || 0) + index*10}\"\n" +
          "></div>\n" +
          "");
    }]);

    angular.module("template/modal/window.html", []).run(["$templateCache", function ($templateCache) {
        $templateCache.put("template/modal/window.html",
          "<div tabindex=\"-1\" role=\"dialog\" class=\"modal fade slide-up disable-scroll\" ng-class=\"{in: animate}\" ng-style=\"{'z-index': 11010 + index*10, display: 'block'}\" ng-click=\"close($event)\">\n" +
          "    <div class=\"modal-dialog\" ng-class=\"{'modal-sm': size == 'sm', 'modal-lg': size == 'lg', 'modal-xl': size == 'xl'}\"><div class=\"modal-content-wrapper\"><div class=\"modal-content\" modal-transclude></div></div></div>\n" +
          "</div>");
    }]);




    /*
    * Theme Global Scripts
    */
    function loadPageScripts() {

        var sidebar = $('[data-pages="sidebar"]').sidebar();

        //$('[data-pages-progress="circle"]').circularProgress(); // initialize circular progress
        var parallax = $('[data-pages="parallax"]').parallax(); // initialize parallax
        $('[data-pages="portlet"]').portlet(); // initialize portlet
        //$('[data-pages="quickview"]').quickview(); // initialize quickview


        //TODO SEARCH ao digitar
        //$('[data-pages="search"]').search({
        //    searchField: '#overlay-search',
        //    closeButton: '.overlay-close',
        //    suggestions: '#overlay-suggestions',
        //    brand: '.brand',
        //    onSearchSubmit: function (searchString) {
        //        console.log("Search for: " + searchString);
        //    },
        //    onKeyEnter: function (searchString) {
        //        console.log("Live search for: " + searchString);
        //        var searchField = $('#overlay-search');
        //        var searchResults = $('.search-results');
        //        clearTimeout($.data(this, 'timer'));
        //        searchResults.fadeOut("fast");
        //        var wait = setTimeout(function () {
        //            searchResults.find('.result-name').each(function () {
        //                if (searchField.val().length != 0) {
        //                    $(this).html(searchField.val());
        //                    searchResults.fadeIn("fast");
        //                }
        //            });
        //        }, 500);
        //        $(this).data('timer', wait);
        //    }
        //});
    }


})();