/**
*
*  Serviços úteis/comuns
*  ==============================================================
*  1. RoutePrefix/ApiPrefix configs
*  2. Logging configs
*  3. Breadcrumb
*  4. MakeUrl
*  
* 
**/

(function() {
    'use strict';

    angular.module('app.core')
           .factory('common', common);

    common.$inject = ['$rootScope', '$document', 'servicebase'];

    function common($rootScope, $document, servicebase) {

        /*
        * Service
        */
        var service = {
            setBreadcrumb: setBreadcrumb,
            getBreadcrumbs: getBreadcrumbs,
            makeUrl: makeUrl,
            makeApiRoute: makeApiRoute,
            validateForm: validateForm
        };

        return service;

        /*
        *
        * Implementations
        * =========================================
        */

        /*
       * Breadcrumb
       */
        function setBreadcrumb(tree) {
            $rootScope.currentCrumbs = [];
            var crumbs = [];
            var segments;
            if (tree.indexOf('.') === -1)
                crumbs.push({ name: tree });
            else {
                segments = tree.split('.');
                segments.forEach(function (item) {
                    crumbs.push({ name: item });
                });
            }
            $rootScope.currentCrumbs = crumbs;
        }

        function getBreadcrumbs() {
            return $rootScope.currentCrumbs;
        }

        /*
        * Montagem de urls
        */
        function makeUrl(params) {
            var url = '';

            if (_.isArray(params)) {
                _.forEach(params, function(item, i) {
                    if (i + 1 == params.length)
                        url += item;
                    else
                        url += item + '/';
                });
                return url;
            } else {
                return undefined;
            }
        }

        function makeApiRoute(route) {
            return servicebase.urlApi() + '/' + route;
        }

        /*
        * Montagem de urls
        */
        function validateForm(form) {
            var isValid = false;
            var formEls = getFormElements(form.$name);
            _.forEach(formEls, function (item, i) {
                    var el = angular.element(item);
                    el.parent().removeClass('has-error');
                    el.closest('.form-group').removeClass('has-error');
            });

            $rootScope.$broadcast(form.$name + ':show-errors-check-validity');
            isValid = form.$valid;

            _.forEach(form.$error.required, function (item, i) {
                if (item.$name) {
                    var elem = angular.element("[name=" + item.$name + "]");
                    if (elem.hasClass('ui-select-container')) {
                        elem.parent().addClass('has-error');
                    }
                }
            });

            return isValid;
        }

        function getFormElements (formName) {
            var els = Array.prototype.slice.call($document[0].forms[formName].elements)
                .filter(hasModel);              
            return els;

            function hasModel(el) {
                return el.hasAttribute('ng-model');
            }
        }
    }
})();
