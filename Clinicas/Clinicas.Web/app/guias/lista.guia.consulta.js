(function () {
    'use strict';

    angular
        .module('app.guias')
        .controller('ListaGuiasConsultas', ListaGuiasConsultas);

    ListaGuiasConsultas.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'guiaservice'];

    function ListaGuiasConsultas($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, guiaservice) {

        var vm = this;

        $scope.forms = {};
        vm.formValid = true;

        common.setBreadcrumb('Guia .Consulta');

        //Funções
        vm.init = init;
        vm.buscar = buscar;
        vm.addGuiaConsulta = addGuiaConsulta;
        vm.cancelarGuia = cancelarGuia;
        vm.print = print;
        //Feature Start
        init();

        //Implementations
        function init() {
            vm.dtOptions = DTOptionsBuilder
                    .newOptions()
                    .withOption('order', [[0, 'desc']]);

            var blocker = blockUI.instances.get('blockListaGuiaConsulta');
            blocker.start();

            guiaservice
                .getguiasconsultas()
                .then(function (result) {
                    vm.guiasconsultas = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function cancelarGuia(id){

            vm.askOptions = { Title: 'Cancelar', Text: 'Tem certeza que deseja cancelar a guia selecionada?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {
                    guiaservice
                       .cancelar(id)
                       .then(function (result) {
                           notification.showSuccessBar("Guia cancelada com sucesso");
                           init();
                       })
                       .catch(function (ex) {
                       })['finally'](function () {
                       });
                }
            });


             guiaservice
                .getguiasconsultas()
                .then(function (result) {
                    vm.guiasconsultas = result.data;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });

        }

        function print(id){
            guiaservice.printconsulta(id);
        }

        function addGuiaConsulta(id) {

            var modalInstance = $modal.open({
                templateUrl: 'app/guias/guia.consulta.html',
                controller: 'GuiaConsultaCrud as vm',
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


       function buscar() {
            if (vm.pesq == undefined || vm.pesq == "") {
                init();
            }
            else {

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockListaGuiaConsulta');
                blocker.start();

                if (vm.pesq.Codigo == undefined) {
                    vm.pesq.Codigo = 0;
                }

                guiaservice
                    .pesquisar(vm.pesq.Codigo, vm.pesq.Nome)
                    .then(function (result) {
                          vm.guiasconsultas = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }


        /* function buscar() {
            if (vm.busca == undefined) {
                vm.FormMessage = "Para realizar a busca você deve preencher pelo menos um campo!";
            }
            else {
                vm.FormMessage = "";
               
                var model = { NomePaciente: vm.busca.NomePaciente, DataInicio: vm.busca.DataInicio, DataFim: vm.busca.DataFim, NumeroGuia: vm.busca.NumeroGuia, Profissional: vm.busca.Profissional };
                var blocker = blockUI.instances.get('blockListaGuiaConsulta');
                blocker.start();
                guiaservice
                   .getguiasbuscaavancada(model)
                   .then(function (result) {
                       if (result.data.length == 0)
                           vm.FormMessage = "Nenhum resultado encontrado para a busca realizada.";
                       else {
                           vm.FormMessage = "";
                           vm.guiasconsultas = result.data;
                       }
                       vm.busca = undefined;
                   })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        } */
    }
})();