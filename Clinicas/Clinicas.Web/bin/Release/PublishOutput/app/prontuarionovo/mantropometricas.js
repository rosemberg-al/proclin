(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('MAntropometricas', MAntropometricas);

    MAntropometricas.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','prontuarioservice','$stateParams'];

    function MAntropometricas($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice,prontuarioservice,$stateParams) {

        var vm = this;
        vm.init = init;
        vm.alterarfoto = alterarfoto;
        vm.crudMAntrometrica = crudMAntrometrica;
        vm.excluirmedidas = excluirmedidas;
        
        vm.arrAltura = [];
        vm.arrPeso = [];
        vm.arrCabeca = [];
        vm.dados = {};

        vm.IdPaciente = $stateParams.id;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
       
        init();

        vm.dtOptions = DTOptionsBuilder
                     .newOptions()
            .withOption('order', [[0, 'desc']]);  
        
        function init() {

            var blocker = blockUI.instances.get('blockProntuario');
            blocker.start();

             prontuarioservice
                .listarMedidas($stateParams.id)
                .then(function (result) {
                    vm.dados = result.data;   

                                vm.arrAltura = [];
                                vm.arrPeso = [];
                                vm.arrCabeca = [];

                                for (var i = 0; i < vm.dados.length; i++) {
                                    vm.arrAltura.push(vm.dados[i].Altura);
                                     vm.arrPeso.push(vm.dados[i].Peso);
                                     vm.arrCabeca.push(vm.dados[i].PerimetroCefalico);
                                }
                                vm.altura = vm.arrAltura;
                                vm.peso = vm.arrPeso;
                                vm.cabeca = vm.arrCabeca;


                                Highcharts.chart('cont_altura', {
                                    title: {
                                        text: 'Altura em cm'
                                    },
                                    colors: ['#562F1E'],
                                    legend: {
                                        layout: 'vertical',
                                        align: 'right',
                                        verticalAlign: 'middle'
                                    },

                                    plotOptions: {
                                        series: {
                                            pointStart: 0
                                        }
                                    },

                                    series: [{
                                        name: 'Altura em cm',
                                        data:vm.altura
                                    }]
                                });
                                Highcharts.chart('cont_peso', {

                                    title: {
                                        text: 'Peso em kg'
                                    },
                                    colors: ['#B22222'],
                                    legend: {
                                        layout: 'vertical',
                                        align: 'right',
                                        verticalAlign: 'middle'
                                    },

                                    plotOptions: {
                                        series: {
                                            pointStart: 0
                                        }
                                    },

                                    series: [{
                                        name: 'Peso em kg',
                                        data: vm.peso
                                    }]
                                });
                                Highcharts.chart('cont_cabeca', {

                                    title: {
                                        text: 'Cabeça em cm'
                                    },
                                    colors: ['#3CB371'],
                                    legend: {
                                        layout: 'vertical',
                                        align: 'right',
                                        verticalAlign: 'middle'
                                    },

                                    plotOptions: {
                                        series: {
                                            pointStart: 0
                                        }
                                    },

                                    series: [{
                                        name: 'Cabeça em cm',
                                        data: vm.cabeca
                                    }]
                                });
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
            
            pacienteservice
                .getPacienteById($stateParams.id)
                .then(function (result) {
                    vm.paciente = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function alterarfoto() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/alterar_foto.html',
                controller: 'AlterarFoto as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return vm.paciente.IdPaciente;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function crudMAntrometrica(idpaciente,idmedida) {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/crud.mantropometricas.html',
                controller: 'CrudMAntrometrica as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    idpaciente: function () {
                        return idpaciente;
                    },
                    idmedida: function () {
                        return idmedida;
                    }
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function excluirmedidas(id) {
           vm.askOptions = { Title: 'Excluir', Text: 'Tem certeza que deseja excluir o registro selecionado ?', Yes: 'Sim', No: 'Não' };
            notification.ask(vm.askOptions, function (confirm) {
                if (confirm) {

                    var blocker = blockUI.instances.get('blockProntuario');
                    blocker.start();
                    prontuarioservice.excluirMedidas(id).then(function (result) {
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



 
    }
})();