(function () {
    'use strict';

    angular.module('app.core')
           .factory('notification', notification);

    notification.$inject = ['$modal'];

    function notification($modal) {

        var service = {
            showError: showError,
            showErrorTop: showErrorTop,
            showErrorBar: showErrorBar,
            showSuccess: showSuccess,
            showSuccessBar: showSuccessBar,
            showWarning: showWarning,
            showInfo: showInfo,
            ask: ask,
            alert: alert,
        };

        return service;


        function showError(message) {
            noty({
                text: '<i class="fa fa-warning"></i> ' + message,
                layout: 'topRight',
                theme: 'relax',
                type: 'error',
                timeout: 6000
                //animation: {
                //    open: 'animated fadeIn',
                //    close: 'animated fadeOutRight',
                //    easing: 'swing',
                //    speed: 500
                //}
            });
        }

        function showErrorTop(message) {
            noty({
                text: '<i class="fa fa-warning"></i> ' + message,
                layout: 'top',
                theme: 'relax',
                type: 'error',
                timeout: 6000,
                //animation: {
                //    open: 'animated fadeInDown',
                //    close: 'animated fadeOutUp',
                //    easing: 'swing',
                //    speed: 500
                //}
            });
        }

        function showErrorBar(message) {
            $('body').pgNotification({
                style: 'bar',
                message: '<strong>' + message + '</strong>',
                position: 'top',
                type: 'danger'
            }).show();
        }

        function showSuccess(message) {
            noty({
                text: '<i class="fa fa-check"></i> ' + message,
                layout: 'topRight',
                theme: 'relax',
                type: 'success',
                timeout: 6000
                //animation: {
                //    open: 'animated fadeIn',
                //    close: 'animated fadeOutRight', 
                //    easing: 'swing',
                //    speed: 500
                //}
            });
        }

        function showSuccessBar(message) {
            $('body').pgNotification({
                style: 'bar',
                message: '<i class="fa fa-check"></i>&nbsp;<strong>' + message + '</strong>',
                position: 'top',
                type: 'success'
            }).show();
        }

        function showWarning(message) {
            noty({
                text: '<i class="fa fa-check"></i> ' + message,
                layout: 'topRight',
                theme: 'relax',
                type: 'warning',
                timeout: 6000
                //animation: {
                //    open: 'animated fadeIn',
                //    close: 'animated fadeOutRight', 
                //    easing: 'swing',
                //    speed: 500
                //}
            });
        }

        function showInfo(message) {
        }

        function ask(askObject, confirmCallback) {

            var modalInstance = $modal.open({
                template: '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                          '     <i class="pg-close fs-14"></i>' +
                          '</button>' +
                          '<div class="container-xs-height full-height">' +
                          '    <div class="row-xs-height">' +
                          '        <div class="modal-body col-xs-height col-middle">' +
                          '            <h4 class="text-primary"><i class="fa fa-question-circle" style="font-size:1.6em"></i> <span class="semi-bold">{{ vm.askOptions.Title }}</span><h4> ' +
                          '            <h5>{{ vm.askOptions.Text }}</h5>' +
                          '            <br>' +
                          '            <button type="button" class="btn btn-primary btn-block" ng-click="vm.confirm()">{{ vm.askOptions.Yes }}</button>' +
                          '            <button type="button" class="btn btn-default btn-block" ng-click="vm.cancel()">{{ vm.askOptions.No }}</button>' +
                          '        </div>' +
                          '    </div>' +
                          '</div>',
                controller: 'askController as vm',
                backdrop: 'static',
                windowClass: 'slide-right',
                keyboard: false,
                size: 'sm',
                resolve: {
                    ask: function () {
                        return askObject;
                    }
                }
            });

            modalInstance.result.then(confirmCallback);
        }

        function alert(alertObject, okCallback) {

            var modalInstance = $modal.open({
                template: '<button type="button" class="close" data-dismiss="modal" aria-hidden="true" ng-click="vm.cancel()">' +
                          '     <i class="pg-close fs-14"></i>' +
                          '</button>' +
                          '<div class="container-xs-height full-height">' +
                          '    <div class="row-xs-height">' +
                          '        <div class="modal-body col-xs-height col-middle">' +
                          '            <h4 class="text-primary"><i class="fa fa-exclamation-triangle" style="font-size:1.6em"></i> <span class="semi-bold">{{ vm.alertOptions.Title }}</span><h4> ' +
                          '            <h5>{{ vm.alertOptions.Text }}</h5>' +
                          '            <br>' +
                          '            <button type="button" class="btn btn-primary btn-block" ng-click="vm.ok()">{{ vm.alertOptions.Action }}</button>' +
                          '        </div>' +
                          '    </div>' +
                          '</div>',
                controller: 'alertController as vm',
                backdrop: true,
                windowClass: 'slide-right',
                size: 'sm',
                resolve: {
                    alert: function () {
                        return alertObject;
                    }
                }
            });

            modalInstance.result.then(okCallback);
        }
    }
})();
