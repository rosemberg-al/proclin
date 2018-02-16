(function () {
    'use strict';

    angular
        .module('app.agenda')
        .controller('LiberarAgenda', LiberarAgenda)

    LiberarAgenda.$inject = ['$scope', '$http', '$q','$modal', '$modalInstance', 'notification', 'DTInstances', 'DTOptionsBuilder', 'blockUI', 'common', 'exception', 'ds.agenda', 'ds.funcionario'];

    function LiberarAgenda($scope, $http, $q,$modal, $modalInstance, notification, DTInstances, DTOptionsBuilder, blockUI, common, exception, dsAgenda,dsFuncionario) {

        common.setBreadcrumb('atendimento .liberar .agendamento');
        var vm = this;

        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.save = save;

        $scope.forms = {};
        vm.formValid = true;


        //Feature Start
        init();

        /* vm.dtOptions = DTOptionsBuilder
                       .newOptions()
                       .withOption('order', [[0, 'desc']]);
        */


        //Implementations
        function init() {

            var pProfissionais = dsFuncionario.listarProfissionaisAtivos();
            pProfissionais.then(function (result) {
                vm.profissionais = result.data;
            });


            $q.all([pProfissionais]).then(function () {
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                exception.throwEx(ex);
            });
        }

        function save() {

            var frm = common.validateForm($scope.forms.liberaragenda);
            if (frm) {

                var blocker = blockUI.instances.get('blockModalLiberarAgenda');
                blocker.start();

                vm.agenda.IdProfissional = vm.profissionalSelecionado;
                vm.agenda.PossuiIntervalo = vm.PossuiIntervalo;

                dsAgenda
                     .liberaragenda(vm.agenda)
                     .then(function (result) {

                         console.log(result);
                         notification.showSuccessBar("Cadastro realizado com sucesso");
                         $modalInstance.close();

                    })
                     .catch(function (ex) {
                         console.log(ex);
                         notification.showError(ex.data.Message);
                         vm.FormMessage = ex.data.Message;
                     })['finally'](function () {
                         blocker.stop();
                     });
            } else {
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos Obrigatórios sem o devido preenchimento";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }
     
    }
})();