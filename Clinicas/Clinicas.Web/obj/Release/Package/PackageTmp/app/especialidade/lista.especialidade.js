(function () {
    'use strict';

    angular
        .module('app.especialidade')
        .controller('ListaEspecialidades', ListaEspecialidades);

    ListaEspecialidades.$inject = ['$scope', '$http', '$q', '$modal', 'blockUI', 'DTInstances', 'DTOptionsBuilder', 'common', 'notification', 'cadastroservice'];

    function ListaEspecialidades($scope, $http, $q, $modal, blockUI, DTInstances, DTOptionsBuilder, common, notification, cadastroservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;
       
        vm.TipoBusca = "Descricao";

        common.setBreadcrumb('Pesquisa .Especialidade');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addEspecialidade = addEspecialidade;
        vm.excluirEspecialidade = excluirEspecialidade;
        vm.especialidadeprocedimento = especialidadeprocedimento;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaEspecialidades');
            blocker.start();

            cadastroservice
                .getAllEspecialidades()
                .then(function (result) {
                    vm.especialidades = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function addEspecialidade(id) {
            var modalInstance = $modal.open({
                templateUrl: 'app/especialidade/crud.especialidade.html',
                controller: 'EspecialidadeCrud as vm',
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

        function especialidadeprocedimento(especialidade) {
            var modalInstance = $modal.open({
                templateUrl: 'app/especialidade/especialidadeprocedimento.html',
                controller: 'EspecialidadeProcedimento as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    especialidade: function () {
                        return especialidade;
                    },
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }



        function excluirEspecialidade(id) {
            vm.askOptions = { Title: 'Excluir Especialidade', Text: 'Tem Certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    cadastroservice
                        .excluirEspecialidade(id)
                        .then(function (result) {
                            notification.showSuccessBar("Exclusão realizada com sucesso ");
                            init();
                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
            });
        }
        function buscar() {
            if (vm.nome == undefined || vm.nome == "") {
                init();
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaEspecialidades');
                blocker.start();
                cadastroservice
                   .getEspecialidadePorNome(vm.nome)
                   .then(function (result) {
                      vm.especialidades = result.data;
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