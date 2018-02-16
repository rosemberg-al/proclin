(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('VisualizarAgenda', VisualizarAgenda)

    VisualizarAgenda.$inject = ['$scope','notification', '$http','$modalInstance', '$q', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', '$modal', 'exception', 'ds.agenda', 'id'];

    function VisualizarAgenda($scope,notification, $http,$modalInstance, $q, DTInstances, DTOptionsBuilder, blockUI, common, $modal, exception, dsAgenda, id) {

        var vm = this;

        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.confirmaratendimento = confirmaratendimento;
        vm.cancelar = cancelar;

        //Feature Start
        init();
        vm.idusuario = 0;

        //Implementations
        function init() {
            dsAgenda
              .obteragendaporid(id)
              .then(function (result) {
                  vm.agenda = result.data;
              })
              .catch(function (ex) {
                  exception.throwEx(ex);
              })['finally'](function () {
                  blocker.stop();
              });
        }

        function confirmaratendimento() {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            dsAgenda
             .realizado(vm.agenda.CdAgenda, vm.idusuario)
             .then(function (result) {
                 notification.showSuccessBar("Atendimento realizado com sucesso");
                 $modalInstance.close();
             })
             .catch(function (ex) {
                 exception.throwEx(ex);
             })['finally'](function () {
                 blocker.stop();
             });
        }


        function cancelar() {
            var blocker = blockUI.instances.get('block');
            blocker.start();
            dsAgenda
             .cancelado(vm.agenda.CdAgenda, vm.idusuario)
             .then(function (result) {
                 notification.showSuccessBar("Cancelamento realizado com sucesso");
                 $modalInstance.close();
             })
             .catch(function (ex) {
                 exception.throwEx(ex);
             })['finally'](function () {
                 blocker.stop();
             });
        }

       

        function cancel() {
            $modalInstance.dismiss('cancel');
        }
        
    }
})();