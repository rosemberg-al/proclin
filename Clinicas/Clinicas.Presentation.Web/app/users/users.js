(function () {
    'use strict';

    angular
        .module('app.users')
        .controller('Users', Users);

    Users.$inject = ['$scope', '$http', '$modal', 'blockUI', 'common', 'notification', 'exception', 'DTInstances', 'ds.users'];

    function Users($scope, $http, $modal, blockUI, common, notification, exception, DTInstances, DsUsers) {

        common.setBreadcrumb('users');
        var vm = this;

        //Funções
        vm.init = init;
        vm.create = create;
        vm.edit = edit;

        //Feature Start
        init();

        //Implementations
        function init() {

            var blocker = blockUI.instances.get('blockGrid');
            blocker.start();

            DsUsers
                .getAll()
                .then(function (result) {
                    vm.Users = result.data;
                    console.log(vm.Users);
                    rerender();
                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function rerender() {
            if (angular.isUndefined(vm.dtInstance)) {
                DTInstances.getLast().then(function (dtInstance) {
                    vm.dtInstance = dtInstance;
                });
            } else {
                vm.dtInstance.rerender();
            }
        }

        function create() {

            var modalInstance = $modal.open({
                templateUrl: 'app/users/edit.html',
                controller: 'EditUser as vm',
                windowClass: 'md-modal-window',
                resolve: {
                    user: null
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }

        function edit(id) {

            blockUI.start();

            DsUsers
                .getById(id)
                .then(function (result) {
                    var modalInstance = $modal.open({
                        templateUrl: 'app/users/edit.html',
                        controller: 'EditUser as vm',
                        windowClass: 'xl-modal-window',
                        resolve: {
                            user: function () {
                                return result.data;
                            }
                        }
                    });

                    modalInstance.result.then(function () {
                        init();
                    });

                })
                .catch(function (ex) {
                    exception.throwEx(ex);
                })['finally'](function () {
                    blockUI.stop();
                });
        }
    }
})();