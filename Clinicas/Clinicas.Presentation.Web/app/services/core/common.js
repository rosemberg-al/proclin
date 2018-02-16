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

    common.$inject = ['$rootScope', '$document', 'appConfig'];

    //Retornar string
    Array.prototype.toStringJoin = function (prop, separator) {
        return this.map(function (elem) {
            return elem[prop];
        }).join(separator);
    }

    //Somar campo de um array
    Array.prototype.sum = function (prop) {
        var total = 0;
        for (var i = 0, _len = this.length; i < _len; i++) {
            total += this[i][prop]
        }
        return total
    }

    //Retornar objetos
    Array.prototype.uniqueObjectByProperty = function (prop) {
        var uniqueObj = [];
        var resultObj = [];
        for (var i = 0; i < this.length; i++) {
            if (uniqueObj.indexOf(this[i][prop]) === -1) {
                uniqueObj.push(this[i][prop]);
                resultObj.push(this[i]);
            }
        }
        return resultObj.sort();
    };

    //Retirar itens duplicados
    Array.prototype.uniqueByProperty = function (prop) {
        var uniqueNames = [];
        for (var i = 0; i < this.length; i++) {
            if (uniqueNames.indexOf(this[i][prop]) === -1) {
                uniqueNames.push(this[i][prop]);
            }
        }
        return uniqueNames.sort();
    };

    Array.prototype.unique = function () {
        var uniqueNames = [];
        for (var i = 0; i < this.length; i++) {
            if (uniqueNames.indexOf(this[i]) === -1) {
                uniqueNames.push(this[i]);
            }
        }
        return uniqueNames.sort();
    };


    function common($rootScope, $document, appConfig) {

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
            return appConfig.apiPrefix + '/' + route;
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
