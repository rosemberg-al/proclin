(function () {
    'use strict';

    angular
        .module('app.especialidade')
        .controller('ListaEspecialidades', ListaEspecialidades);

    ListaEspecialidades.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros'];

    function ListaEspecialidades($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, exception, dsCadastros) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('pagina-inicial .cadastro .especialidades');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addEspecialidade = addEspecialidade;
        vm.excluirEspecialidade = excluirEspecialidade;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockModalListaEspecialidades');
            blocker.start();

            dsCadastros
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

        function excluirEspecialidade(id) {
            vm.askOptions = { Title: 'Excluir Especialidade', Text: 'Tem Certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    dsCadastros
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
                vm.FormMessage = "Para realizar a busca você deve preencher o campo nome!";
            }
            else {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalListaEspecialidades');
                blocker.start();
                dsCadastros
                   .getEspecialidadePorNome(vm.nome)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else
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