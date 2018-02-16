(function () {
    'use strict';

    angular
        .module('app.cid')
        .controller('CidCrud', CidCrud);

    CidCrud.$inject = ['$scope', '$http', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros', 'codigo'];

    function CidCrud($scope, $http, $modal, $modalInstance, blockUI, common, notification, exception, dsCadastro, codigo) {

        var vm = this;
        vm.State = "Incluir CID";
        vm.FormMessage = "";
        vm.cid = {};

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            combos();

            if (codigo != "") {
                vm.State = "Editar CID";
                var blocker = blockUI.instances.get('blockModalCidCrud');
                blocker.start();
                dsCadastro
                    .getCidByCodigo(codigo)
                    .then(function (result) {
                        vm.cid = result.data;

                        var sexo = _.find(vm.sexos, { Key: vm.cid.SexoOcorrencia });
                        if (sexo != null)
                            vm.sexoSelecionado = sexo.Key;

                        var agravo = _.find(vm.tpagravo, { Key: vm.cid.Agravo });
                        if (agravo != null)
                            vm.agravoSelecionado = agravo.Key;

                        var estadio = _.find(vm.tpestadio, { Key: vm.cid.Estadio });
                        if (estadio != null)
                            vm.estadioSelecionado = estadio.Key;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function combos() {
            vm.tpagravo = [{ Key: "0", Value: "0" }, { Key: "1", Value: "1" },  {Key: "2", Value: "2"}];
            vm.sexos = [{Key: "I", Value: "Indiferente"},{Key: "F", Value: "Feminino"},{Key: "M", Value: "Masculino"}];
            vm.tpestadio = [{ Key: "S", Value: "Sim" }, { Key: "N", Value: "Não" }];
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            vm.formValid = common.validateForm($scope.forms.crudcid);

            if (vm.formValid) {
                vm.FormMessage = "";

                if (vm.agravoSelecionado != undefined)
                    vm.cid.Agravo = vm.agravoSelecionado;

                if (vm.sexoSelecionado != undefined)
                    vm.cid.SexoOcorrencia = vm.sexoSelecionado;

                if (vm.estadioSelecionado != undefined)
                    vm.cid.Estadio = vm.estadioSelecionado;

                var blocker = blockUI.instances.get('blockModalCidCrud');
                blocker.start();

                dsCadastro
                    .saveCid(vm.cid)
                    .then(function (result) {
                        vm.cid = result.data;
                        if (codigo == "")
                            notification.showSuccessBar("CID cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("CID alterado com sucesso!");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }

    }
})();