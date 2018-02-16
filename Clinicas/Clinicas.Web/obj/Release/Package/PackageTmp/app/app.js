
(function () {
    var app = angular.module('app', [
        'ui.router',
        'ui.bootstrap',
        'oc.lazyLoad',
        'datatables',
        'LocalStorageModule',
        'blockUI',
        'app.core',
        'ui.utils.masks',
        'ngMask',
        'webcam',
        'textAngular',
        'ui.calendar',
        'ui.select',

        'app.login',
        'app.dashboard',
        'app.paciente',
        'app.convenio',
        'app.especialidade',
        'app.cid',
        'app.fornecedor',
        'app.funcionario',
        'app.hospital',
        'app.ocupacao',
        'app.procedimento',
        'app.vacinas',
        'app.guias',
        'app.financeiro',
        //'app.atendimento',
        'app.agenda',
        'app.relatorio',
        'app.medicamento',
        'app.modeloprontuario',
        'app.clinica',
        'app.usuario',
        'app.tabelapreco',
        'app.busca',
        'app.solicitaracesso',
        'app.consultorio',
        'app.bloqueio',
        'app.estoque'
    ]);

    /*
   * BlockUI
   */
    app.config(function (blockUIConfig) {

        // Change the default overlay message
        //blockUIConfig.message = 'Aguarde...';
        blockUIConfig.autoBlock = false;
        blockUIConfig.template = '<div class="block-ui-overlay"></div><div class="block-ui-message-container" aria-live="assertive" aria-atomic="true"><div class="block-ui-message ng-binding" ng-class="$_blockUiMessageClass"><div id="followingBallsG"><div id="followingBallsG_1" class="followingBallsG"></div><div id="followingBallsG_2" class="followingBallsG"></div><div id="followingBallsG_3" class="followingBallsG"></div><div id="followingBallsG_4" class="followingBallsG"></div></div></div></div>';
    });

    /*
     * ui-select2
     */
    app.config(function (uiSelectConfig) {
        uiSelectConfig.theme = 'bootstrap';
        uiSelectConfig.resetSearchInput = true;
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



 app.directive('appCpfCnpj', function cpfCnpj(){
    return {
      link : link,
      require: 'ngModel'
    };
    function link(scope, element, attrs, ngModelController){
      attrs.$set('maxlength',18);
      scope.$watch(attrs['ngModel'], applyMask);
      function applyMask(event){
        var value=element.val().replace(/\D/g,"");
        if (value.length <= 11) {
          value=value.replace(/(\d{3})(\d)/,"$1.$2");
          value=value.replace(/(\d{3})(\d)/,"$1.$2");
          value=value.replace(/(\d{3})(\d{1,2})$/,"$1-$2");
        } else {
          value=value.replace(/^(\d{2})(\d)/,"$1.$2")
          value=value.replace(/^(\d{2})\.(\d{3})(\d)/,"$1.$2.$3");
          value=value.replace(/\.(\d{3})(\d)/,".$1/$2");
          value=value.replace(/(\d{4})(\d)/,"$1-$2");
        }
        element.val(value);
        if('asNumber' in attrs){
          ngModelController.$setViewValue(
            isNaN(parseInt(value.replace(/\D/g,""), 10))
            ? undefined
            : parseInt(value.replace(/\D/g,""), 10));
        } else {
          ngModelController.$setViewValue(value);
        }
      }
    };//link
 });//cpfCnpj


    /* jshint undef:false */
    //app.directive('webcam', [function () {
    //    'use strict';

    //    var ua = navigator.userAgent,
    //        phantomjs = /phantom/i.test(ua);

    //    if (phantomjs) {
    //        console.log('PhantomJS detected; video.play will be mocked');
    //    }

    //    var element,
    //        mediaSpy,
    //        onStreamSpy,
    //        onErrorSpy,
    //        onSuccessSpy,
    //        rootScope;

    //    //beforeEach(module('webcam'));

    //    //beforeEach(function createSpy() {
    //    //    navigator.getMedia = mediaSpy = jasmine.createSpy('getMediaSpy');
    //    //    onStreamSpy = jasmine.createSpy('onStreamSpy');
    //    //    onErrorSpy = jasmine.createSpy('onErrorSpy');
    //    //    onSuccessSpy = jasmine.createSpy('onSuccessSpy');
    //    //});

    //    //beforeEach(inject(function ($rootScope, $compile) {
    //    //    $rootScope.opts = {
    //    //        // video: null
    //    //    };
    //    //    $rootScope.onStream = onStreamSpy;
    //    //    $rootScope.onError = onErrorSpy;
    //    //    $rootScope.onSuccess = onSuccessSpy;
    //    //    rootScope = $rootScope;
    //    //    element = angular.element(
    //    //        '<webcam ' +
    //    //        'on-stream="onStream(stream)" ' +
    //    //        'on-error="onError(err)" ' +
    //    //        'on-streaming="onSuccess()" ' +
    //    //        'channel="opts"' +
    //    //        'placeholder="\'http://www.example.com/\'">' +
    //    //        '</webcam>');
    //    //    element = $compile(element)($rootScope);
    //    //    expect(element).not.toBe(null);
    //    //}));

    //    if('should get only video media from the navigator',
    //        function () {
    //            expect(mediaSpy).toHaveBeenCalled();
    //            var args = mediaSpy.mostRecentCall.args;
    //            expect(args[0]).not.toBeNull();
    //            expect(args[0].video).toBeTruthy();
    //            expect(args[0].audio).toBeFalsy(); // Needs to be changed if audio support is added
    //            expect(typeof args[1]).toBe('function');
    //            expect(typeof args[2]).toBe('function');
    //        }
    //    );

    //    it('should create a video element', function () {
    //        var video = element.find('video');
    //        expect(video.length).toBe(1);
    //        expect(video[0].getAttribute('class')).toBe('webcam-live');
    //        expect(video[0].getAttribute('autoplay')).toBe('');
    //    });

    //    it('should create an image for the placeholder',
    //        function () {
    //            var image = element.find('img');
    //            expect(image.length).toBe(1);
    //            expect(image[0].getAttribute('src')).toBe('http://www.example.com/');
    //            expect(image[0].getAttribute('class')).toBe('webcam-loader');
    //        }
    //    );

    //    describe('on success', function () {
    //        var video,
    //            streamSpy;

    //        beforeEach(function () {
    //            video = element.find('video')[0];
    //            if (phantomjs) { // phantomjs doesn't support video
    //                video.play = jasmine.createSpy('play');
    //            } else {
    //                spyOn(video, 'play');
    //            }

    //            streamSpy = jasmine.createSpyObj('stream', ['stop']);

    //            // createObjectURL throws a Type Error if passed a spy
    //            var vendorURL = window.URL || window.webkitURL;
    //            spyOn(vendorURL, 'createObjectURL').andReturn('abc');
    //        });

    //        beforeEach(function () {
    //            var args = mediaSpy.mostRecentCall.args;
    //            args[1](streamSpy); // call success function
    //        });

    //        it('should play the video element', function () {
    //            expect(video.play).toHaveBeenCalled();
    //        });

    //        it('should make the video element available for the parent scope',
    //            function () {
    //                expect(rootScope.opts.video).not.toBeUndefined();
    //                expect(rootScope.opts.video).not.toBeNull();
    //                expect(rootScope.opts.video).toBe(video);
    //            });

    //        it('should call the stream callback', function () {
    //            expect(onStreamSpy).toHaveBeenCalledWith(streamSpy);
    //        });

    //        describe('scope destruction', function () {
    //            beforeEach(function () {
    //                expect(video.src).toBeTruthy(); // non-empty string
    //                element.scope().$destroy();
    //            });

    //            it('should stop the video stream', function () {
    //                runs(function () { expect(streamSpy.stop).toHaveBeenCalled(); });
    //            });

    //            it('should clear the video element src', function () {
    //                runs(function () { expect(video.src).toBeFalsy(); }); // empty or null
    //            });
    //        });
    //    });

    //    describe('on failure', function () {
    //        var video;

    //        beforeEach(function () {
    //            video = element.find('video')[0];
    //            if (phantomjs) { // phantomjs doesn't support video
    //                video.play = jasmine.createSpy('play');
    //            } else {
    //                spyOn(video, 'play').andCallThrough();
    //            }
    //        });

    //        beforeEach(function () {
    //            var args = mediaSpy.mostRecentCall.args;
    //            args[2]('Fake Error'); // call failure function
    //        });

    //        it('should not play the video element', function () {
    //            expect(video.play).not.toHaveBeenCalled();
    //        });

    //        it('should remove the placeholder', function () {
    //            var image = element.find('img');
    //            expect(image.length).toBe(0);
    //        });

    //        it('should call the error callback', function () {
    //            expect(onErrorSpy).toHaveBeenCalled();
    //            expect(onErrorSpy.mostRecentCall.args[0]).toBe('Fake Error');
    //        });
    //    });

    //    describe('without user media support', function () {

    //        beforeEach(function () {
    //            navigator.getMedia = false;
    //        });

    //        beforeEach(inject(function ($rootScope, $compile) {
    //            $rootScope.onStream = onStreamSpy;
    //            $rootScope.onError = onErrorSpy;
    //            $rootScope.onSuccess = onSuccessSpy;
    //            element = angular.element(
    //                '<webcam ' +
    //                'on-stream="onStream(stream,video)" ' +
    //                'on-error="onError(err)" ' +
    //                'on-streaming="onSuccess(video)" ' +
    //                'placeholder="\'http://www.example.com/\'">' +
    //                '</webcam>');
    //            element = $compile(element)($rootScope);
    //            expect(element).not.toBe(null);
    //        }));

    //        it('should call the failure callback',
    //            function () {
    //                expect(onErrorSpy).toHaveBeenCalled();
    //                expect(onErrorSpy.mostRecentCall.args[0]).not.toBeNull();
    //                expect(onErrorSpy.mostRecentCall.args[0].code).toBe(-1);
    //            }
    //        );
    //    });

    //}]);

    /* ============================================================
     * Directive: status-imc
     * AngularJS directive para o status (label) do calculo de IMC
     * ============================================================ */
    app.directive('statusImc', [function () {
        return {
            restrict: 'AE',
            template: '<span class="label label-rounded"></span>',
            replace: true,
            scope: { imc: '@', sexo: '@' },
            link: function ($scope, element, attrs) {

                $scope.$watch('statusImc', function () {

                    if ($scope.sexo == 'Feminino') {
                         if ($scope.imc < 18.5) {
                            element.text($scope.imc+" Abaixo do Peso ");
                            element.addClass('label-default');
                        }
                        else if ($scope.imc >= 20.7 && $scope.imc <= 26.4) {
                            element.text($scope.imc+" - Peso Normal ");
                            element.addClass('label-success');
                        }
                        else if ($scope.imc >= 26.4 && $scope.imc < 27.8) {
                            element.text($scope.imc+" - Sobrepeso ");
                            element.addClass('label-system');
                        }
                        else if ($scope.imc >= 30.0 && $scope.imc < 34.9) {
                            element.text($scope.imc+" - Obesidade grau I ");
                            element.addClass('label-danger');
                        }
                        else if ($scope.imc >= 35.0 && $scope.imc < 39.9) {
                            element.text($scope.imc+" - Obesidade grau II ");
                            element.addClass('label-danger');
                        }
                        else if ($scope.imc >= 40.0) {
                            element.text($scope.imc);
                            element.addClass('label-danger'+" - Obesidade grau III ");
                        }
                        else {
                            if (angular.isDefined($scope.imc))
                                console.warn('Nenhum estilo CSS definido para status "' + $scope.imc + '" na diretiva status-imc. Usando os valores padrões.');
                        }
                    }
                    else {
                        if ($scope.imc < 18.5) {
                            element.text($scope.imc+" Abaixo do Peso ");
                            element.addClass('label-default');
                        }
                        else if ($scope.imc >= 20.7 && $scope.imc <= 26.4) {
                            element.text($scope.imc+" - Peso Normal ");
                            element.addClass('label-success');
                        }
                        else if ($scope.imc >= 26.4 && $scope.imc < 27.8) {
                            element.text($scope.imc+" - Sobrepeso ");
                            element.addClass('label-system');
                        }
                        else if ($scope.imc >= 30.0 && $scope.imc < 34.9) {
                            element.text($scope.imc+" - Obesidade grau I ");
                            element.addClass('label-danger');
                        }
                        else if ($scope.imc >= 35.0 && $scope.imc < 39.9) {
                            element.text($scope.imc+" - Obesidade grau II ");
                            element.addClass('label-danger');
                        }
                        else if ($scope.imc >= 40.0) {
                            element.text($scope.imc+" - Obesidade grau III ");
                            element.addClass('label-danger');
                        }
                        else {
                            if (angular.isDefined($scope.imc))
                                console.warn('Nenhum estilo CSS definido para status "' + $scope.imc + '" na diretiva status-imc. Usando os valores padrões.');
                        }
                    }
                });
            }
        };
    }]);

    app.directive('myEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.myEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

    app.directive('showErrors', function showErrors() {
        return {
            restrict: 'A',
            require: '^form',
            link: function (scope, el, attrs, formCtrl) {
                // find the text box element, which has the 'name' attribute
                var inputEl = el[0].querySelector("[name]");
                // convert the native text box element to an angular element
                var inputNgEl = angular.element(inputEl);
                // get the name on the text box
                var inputName = inputNgEl.attr('name');

                // only apply the has-error class after the user leaves the text box
                inputNgEl.bind('blur', function () {
                    el.toggleClass('has-danger', formCtrl[inputName].$invalid);
                });

                scope.$watch(function () {
                    return scope.showErrorsCheckValidity;
                }, function (newVal, oldVal) {
                    if (!newVal) { return; }
                    if (formCtrl[inputName] != undefined)
                        el.toggleClass('has-danger', formCtrl[inputName].$invalid);
                });
            }
        }
    });


    angular.module('app').filter('tel', function () {
        return function (input) {
            var str = input + '';
            str = str.replace(/\D/g, '');
            if (str.length === 11) {
                str = str.replace(/^(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
            } else {
                str = str.replace(/^(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
            }
            return str;
        };
    });

    angular.module('app').filter('cep', function () {
        return function (input) {
            var str = input + '';
            str = str.replace(/\D/g, '');
            str = str.replace(/^(\d{2})(\d{3})(\d)/, "$1.$2-$3");
            return str;
        };
    });


    app.directive('iCheck', ['$timeout', '$parse', function ($timeout, $parse) {
        return {
            require: 'ngModel',
            link: function ($scope, element, $attrs, ngModel) {

                var $element = $(element);
                var value;
                value = $attrs.value;

                $element.iCheck({
                    checkboxClass: 'icheckbox_square-green',
                    radioClass: 'iradio_square-green',
                });


                $scope.$watch($attrs['ngDisabled'], function (newValue) {
                    $(element).iCheck(newValue ? 'disable' : 'enable');
                    $(element).iCheck('update');
                })

                $element.on('ifChecked', function (event) {
                    if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                        $scope.$apply(function () {
                            return ngModel.$setViewValue(event.target.checked);
                        });
                    }
                    if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                        return $scope.$apply(function () {
                            return ngModel.$setViewValue(value);
                        });
                    }
                });

                $element.on('ifUnchecked', function (event) {
                    ngModel.$setViewValue(false);
                });

                $scope.$watch($attrs.ngHide, function (newValue) {
                    if (newValue) {
                        $element.parent().hide();
                    } else {
                        $element.parent().show();
                    }
                });


                $scope.$watch($attrs.ngModel, function (newValue) {
                    if (newValue) {
                        $element.iCheck('check');
                        $element.iCheck('update');
                    } else {
                        $element.iCheck('uncheck');
                        $element.iCheck('update');
                    }
                });

                $scope.$watch($attrs.ngDisabled, function (newValue) {
                    if (newValue) {
                        $element.iCheck('disable');
                    } else {
                        $element.iCheck('enable');
                    }
                });
            }
        }
    }]);


    //app.directive('leftMenu', function () {
    //    return {
    //        restrict: 'A',
    //        link: function (scope, element, attrs) {
    //            element.on('click', '.left-menu-link', function () {

    //                if (!$(this).closest('.left-menu-list-submenu').length) {
    //                    $('.left-menu-list-opened > a + ul').slideUp(200, function () {
    //                        $('.left-menu-list-opened').removeClass('left-menu-list-opened');
    //                    });
    //                }

    //            });
    //        }
    //    };
    //})

})();
