﻿(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('ListaPacienteProntuario', ListaPacienteProntuario);

    ListaPacienteProntuario.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice'];

    function ListaPacienteProntuario($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        //Breadcrumb
        common.setBreadcrumb('Prontuário Eletrônico');

        vm.tipoBusca = 'Nome';

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addpaciente = addpaciente;
        vm.excluir = excluir;


        //Feature Start
        init();

        //Implementations
        function init() {

            vm.pesq = {};
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);
            vm.FormMessage = "";
            var blocker = blockUI.instances.get('blockModalListaFor');
            blocker.start();

            pacienteservice
                .listarPacientes()
                .then(function (result) {
                    vm.pacientees = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addpaciente(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/crud.paciente.html',
                controller: 'pacienteCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function excluir(id) {
            vm.askOptions = { Title: 'Excluir Paciente', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockModalListaFor');
                    blocker.start();
                    pacienteservice.excluirPacienteById(id).then(function (result) {
                        notification.showSuccessBar("Exclusão realizada com sucesso");
                        init();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
                    blocker.stop();
                }
            });
        }

        function buscar() {
            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaFor');
                blocker.start();
                if (vm.pesq.Nome == undefined) {
                    vm.pesq.Nome = "";
                }
                pacienteservice
                   .pesquisarPacientes(vm.pesq.Nome, vm.pesq.Codigo)
                   .then(function (result) {
                       vm.pacientees = result.data;
                   })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }
    }
})();