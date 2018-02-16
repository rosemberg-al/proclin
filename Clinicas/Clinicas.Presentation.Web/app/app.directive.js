(function () {

    'use strict';
    angular
      .module('app')

    /*
 *
 * Directives
 * ============================
 */

    // Diretiva para 'Loading'
    // Usage: Incluir na div desejada: <loading></loading>
    app.directive('loading', function () {
        return {
            restrict: 'E',
            replace: true,
            template: '<div class="row"><div class="col-md-12"><div style="text-align: center"><img src="/content/img/loading.gif" /></div></div></div>',
            link: function (scope, element, attr) {
                scope.$watch('loading', function (val) {
                    if (val)
                        $(element).show();
                    else
                        $(element).hide();
                });
            }
        };
    });



    //Spin (Up/Down em inputs)
    app.directive('spin', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {

                var min = 0;
                var max = 999;
                var step = 1;

                element = angular.element(element);

                if (angular.isDefined(attrs)) {
                    min = typeof attrs.min !== 'undefined' ? attrs.min : 0;
                    max = typeof attrs.max !== 'undefined' ? attrs.max : 999;
                }

                element.TouchSpin({
                    verticalbuttons: true
                });

                if (!scope.$$phase) {
                    scope.$apply();
                }
            }
        };
    });


    // Directive for generic chart, pass in chart options
    app.directive('hcChart', function () {
        return {
            restrict: 'E',
            template: '<div></div>',
            scope: {
                options: '='
            },
            link: function (scope, element) {
                Highcharts.chart(element[0], scope.options);
            }
        };
    });
    // Directive for pie charts, pass in title and data only    
    app.directive('hcPieChart', function () {
        return {
            restrict: 'E',
            template: '<div></div>',
            scope: {
                title: '@',
                data: '='
            },
            link: function (scope, element) {
                Highcharts.chart(element[0], {
                    chart: {
                        type: 'pie'
                    },
                    title: {
                        text: scope.title
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            }
                        }
                    },
                    series: [{
                        data: scope.data
                    }]
                });
            }
        };
    });

    app.directive('homeChart', function () {
        console.log("config.series ewrwetregfdg");
        return {
            restrict: 'E',
            replace: true,
            template: '<div></div>',
            scope: {
                config: '='
            },
            link: function (scope, element, attrs) {
                console.log("config.series 87987897897");
                var chart;
                var process = function () {
                    var defaultOptions = {
                        chart: { renderTo: element[0] },
                    };
                    var config = angular.extend(defaultOptions, scope.config);
                    chart = new Highcharts.Chart(config);
                };
                process();
                scope.$watch("config.series", function (loading) {
                    console.log("config.series");
                    process();
                });
                scope.$watch("config.loading", function (loading) {
                    console.log("config.series 2222");
                    if (!chart) {
                        return;
                    }
                    if (loading) {
                        chart.showLoading();
                    } else {
                        chart.hideLoading();
                    }
                });
            }
        };
    });


    app.directive('hcChart', function () {
        return {
            restrict: 'A',
            template: '<div></div>',
            scope: {
                options: '='
            },
            link: function (scope, element, attribute) {
                Highcharts.chart('chart', {
                    chartOptions: {
                        type: 'line'
                    },

                    title: {
                        text: 'Temperature data'
                    },
                    series: [{
                        data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]
                    }]
                });
            }
        }

    });


    app.directive('uiCalendar', ['uiCalendarConfig',
        function (uiCalendarConfig) {

            return {
                restrict : 'AE',
                scope : {
                    eventSources : '=ngModel',
                    calendarWatchEvent : '&'
                },
                controller : 'uiCalendarCtrl',
                link : function (scope, elm, attrs, controller) {
                    var sources = scope.eventSources;
                    var sourcesChanged = false;
                    var calendar;
                    var eventSourcesWatcher = controller.changeWatcher(sources, controller.sourceFingerprint);
                    var eventsWatcher = controller.changeWatcher(controller.allEvents, controller.eventFingerprint);
                    var options = null;

                    function getOptions () {
                        var calendarSettings = attrs.uiCalendar ? scope.$parent.$eval(attrs.uiCalendar) : {};
                        var fullCalendarConfig = controller.getFullCalendarConfig(calendarSettings, uiCalendarConfig);
                        var localeFullCalendarConfig = controller.getLocaleConfig(fullCalendarConfig);
                        angular.extend(localeFullCalendarConfig, fullCalendarConfig);
                        options = {
                            eventSources : sources
                        };
                        angular.extend(options, localeFullCalendarConfig);
                        //remove calendars from options
                        options.calendars = null;

                        var options2 = {};
                        for (var o in options) {
                            if (o !== 'eventSources') {
                                options2[o] = options[o];
                            }
                        }
                        return JSON.stringify(options2);
                    }

                    scope.destroyCalendar = function () {
                        if (calendar && calendar.fullCalendar) {
                            calendar.fullCalendar('destroy');
                        }
                        if (attrs.calendar) {
                            calendar = uiCalendarConfig.calendars[attrs.calendar] = angular.element(elm).html('');
                        } else {
                            calendar = angular.element(elm).html('');
                        }
                    };

                    scope.initCalendar = function () {
                        if (!calendar) {
                            calendar = $(elm).html('');
                        }
                        calendar.fullCalendar(options);
                        if (attrs.calendar) {
                            uiCalendarConfig.calendars[attrs.calendar] = calendar;
                        }
                    };

                    scope.$on('$destroy', function () {
                        scope.destroyCalendar();
                    });

                    eventSourcesWatcher.onAdded = function (source) {
                        if (calendar && calendar.fullCalendar) {
                            calendar.fullCalendar(options);
                            if (attrs.calendar) {
                                uiCalendarConfig.calendars[attrs.calendar] = calendar;
                            }
                            calendar.fullCalendar('addEventSource', source);
                            sourcesChanged = true;
                        }
                    };

                    eventSourcesWatcher.onRemoved = function (source) {
                        if (calendar && calendar.fullCalendar) {
                            calendar.fullCalendar('removeEventSource', source);
                            sourcesChanged = true;
                        }
                    };

                    eventSourcesWatcher.onChanged = function () {
                        if (calendar && calendar.fullCalendar) {
                            calendar.fullCalendar('refetchEvents');
                            sourcesChanged = true;
                        }
                    };

                    eventsWatcher.onAdded = function (event) {
                        if (calendar && calendar.fullCalendar) {
                            calendar.fullCalendar('renderEvent', event, !!event.stick);
                        }
                    };

                    eventsWatcher.onRemoved = function (event) {
                        if (calendar && calendar.fullCalendar) {
                            calendar.fullCalendar('removeEvents', event._id);
                        }
                    };

                    eventsWatcher.onChanged = function (event) {
                        if (calendar && calendar.fullCalendar) {
                            var clientEvents = calendar.fullCalendar('clientEvents', event._id);
                            for (var i = 0; i < clientEvents.length; i++) {
                                var clientEvent = clientEvents[i];
                                clientEvent = angular.extend(clientEvent, event);
                                calendar.fullCalendar('updateEvent', clientEvent);
                            }
                        }
                    };

                    eventSourcesWatcher.subscribe(scope);
                    eventsWatcher.subscribe(scope, function () {
                        if (sourcesChanged === true) {
                            sourcesChanged = false;
                            // return false to prevent onAdded/Removed/Changed handlers from firing in this case
                            return false;
                        }
                    });

                    scope.$watch(getOptions, function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            scope.destroyCalendar();
                            scope.initCalendar();
                        } else if ((newValue && angular.isUndefined(calendar))) {
                            scope.initCalendar();
                        }
                    });
                }
            };
        }]);

    app.directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

    //DatePicker
    app.directive('bsdatepicker', function () {
        return {
            restrict: 'A',
            link: function (scope, element) {
                element.datepicker({ format: 'dd/mm/yyyy', language: 'pt-BR' });
            }
        };
    });

    angular.module("textAngularTest", ['textAngular'])
        .controller('wysiwygeditor', ['$scope', 'textAngularManager', function wysiwygeditor($scope, textAngularManager) {
            $scope.version = textAngularManager.getVersion();
            $scope.versionNumber = $scope.version.substring(1);
            $scope.orightml = '<h2>Try me!</h2><p>textAngular is a super cool WYSIWYG Text Editor directive for AngularJS</p><p><img class="ta-insert-video" ta-insert-video="http://www.youtube.com/embed/2maA1-mvicY" src="" allowfullscreen="true" width="300" frameborder="0" height="250"/></p><p><b>Features:</b></p><ol><li>Automatic Seamless Two-Way-Binding</li><li>Super Easy <b>Theming</b> Options</li><li style="color: green;">Simple Editor Instance Creation</li><li>Safely Parses Html for Custom Toolbar Icons</li><li class="text-danger">Doesn&apos;t Use an iFrame</li><li>Works with Firefox, Chrome, and IE9+</li></ol><p><b>Code at GitHub:</b> <a href="https://github.com/fraywing/textAngular">Here</a> </p><h4>Supports non-latin Characters</h4><p>昮朐 魡 燚璒瘭 譾躒鑅, 皾籈譧 紵脭脧 逯郹酟 煃 瑐瑍, 踆跾踄 趡趛踠 顣飁 廞 熥獘 豥 蔰蝯蝺 廦廥彋 蕍蕧螛 溹溦 幨懅憴 妎岓岕 緁, 滍 蘹蠮 蟷蠉蟼 鱐鱍鱕, 阰刲 鞮鞢騉 烳牼翐 魡 骱 銇韎餀 媓幁惁 嵉愊惵 蛶觢, 犝獫 嶵嶯幯 縓罃蔾 魵 踄 罃蔾 獿譿躐 峷敊浭, 媓幁 黐曮禷 椵楘溍 輗 漀 摲摓 墐墆墏 捃挸栚 蛣袹跜, 岓岕 溿 斶檎檦 匢奾灱 逜郰傃</p>';
            $scope.htmlcontent = $scope.orightml;
            $scope.disabled = false;
        }]);


    angular.module('app').directive('onlyDigits', function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attr, ctrl) {
                function inputValue(val) {
                    if (val) {
                        var digits = val.replace(/[^0-9]/g, '');

                        if (digits !== val) {
                            ctrl.$setViewValue(digits);
                            ctrl.$render();
                        }
                        return parseInt(digits, 10);
                    }
                    return undefined;
                }
                ctrl.$parsers.push(inputValue);
            }
        };
    });

    angular.module('app').directive('onlyAlphabets', function () {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                ngModelCtrl.$parsers.push(function (val) {
                    if (angular.isUndefined(val)) {
                        var val = '';
                    }
                    var clean = val.replace(/[^a-zA-Z]/g, '');

                    if (val !== clean) {
                        ngModelCtrl.$setViewValue(clean);
                        ngModelCtrl.$render();
                    }
                    return clean;
                });

                element.bind('keypress', function (event) {
                    if (event.keyCode === 32) {
                        event.preventDefault();
                    }
                });
            }
        };
    });

    angular.module('app').directive('validDate', function () {
        return {
            restrict: 'EA',
            require: '?ngModel',
            scope: {
                mindate: '=',
                maxdate: '='
            },
            link: function (scope, elem, attr, ngModel) {
                function validate(value) {
                    if (value.length > 0) {
                        //Formato dd/mm/yyyy
                        var from = value.split("/");
                        var d = Date.parse(new Date(from[2], from[1] - 1, from[0]).toString());
                        if (isNaN(d)) {
                            ngModel.$setValidity('valid', false);
                        } else {
                            var valid = true;
                            if (scope.mindate != undefined) {
                                valid = d >= Date.parse(scope.mindate);
                            }
                            if (scope.maxdate != undefined) {
                                valid = d <= Date.parse(scope.maxdate);
                            }
                            ngModel.$setValidity('valid', valid);
                        }
                    }
                };

                scope.$watch(function () {
                    return ngModel.$viewValue;
                }, validate);

            }
        };
    });




})();