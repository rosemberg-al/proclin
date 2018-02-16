(function () {
    'use strict';

    angular
        .module('app.atestado')
        .controller('AtestadoCrud', AtestadoCrud);

    AtestadoCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'textAngularManager', 'common', 'notification', 'DTInstances', 'prontuarioservice', 'funcionarioservice', 'paciente', 'id'];

    function AtestadoCrud($scope, $http, $q, $modal, $modalInstance, blockUI, textAngularManager, common, notification, DTInstances, prontuarioservice, funcionarioservice, paciente, id) {

        var vm = this;
        vm.State = "Incluir Atestado";
        vm.FormMessage = "";
        vm.profissionalSelecionado = undefined;
        vm.modeloSelecionado = undefined;
        vm.atestado = {
            Paciente: undefined
        };
        vm.exibeModelo = true;


        $scope.forms = {};
        vm.formValid = true;

        $scope.version = textAngularManager.getVersion();
        $scope.versionNumber = $scope.version.substring(1);
        $scope.orightml = "";
        $scope.htmlcontent = $scope.orightml;
        $scope.disabled = true; 


        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            vm.atestado.Paciente = paciente.NmPaciente;

            var pProfissionais = funcionarioservice.listarProfissionaisAtivos();
            pProfissionais.then(function (result) {
                vm.profissionais = result.data;
            });

            var pModelos = prontuarioservice.getModelosAtestados();
            pModelos.then(function (result) {
                vm.modelosatestados = result.data;
            });

            $q.all([pProfissionais, pModelos]).then(function () {
                if (id > 0) {
                    vm.exibeModelo = false;
                    vm.State = "Editar Atestado";
                    var blocker = blockUI.instances.get('blockModalAtestado');
                    blocker.start();
                    prontuarioservice
                        .getAtestadoById(id)
                        .then(function (result) {
                            vm.atestado = result.data;
                            console.log(result.data);
                            if (result.data.Situacao == 'Ativo')
                                vm.atestado.Situacao = "A";
                            else
                                vm.atestado.Situacao = "I";

                            //seta o profissional de saúde
                            var profissional = _.find(vm.profissionais, { IdFuncionario: vm.atestado.IdFuncionario });
                            vm.profissionalSelecionado = profissional.IdFuncionario;
                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
                else {
                    vm.atestado.Situacao = "A";
                }
            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
           
        }

        function retornaMes(index) {
            var arrayMes = new Array(12);
            arrayMes[0] = "Janeiro";
            arrayMes[1] = "Fevereiro";
            arrayMes[2] = "Março";
            arrayMes[3] = "Abril";
            arrayMes[4] = "Maio";
            arrayMes[5] = "Junho";
            arrayMes[6] = "Julho";
            arrayMes[7] = "Agosto";
            arrayMes[8] = "Setembro";
            arrayMes[9] = "Outubro";
            arrayMes[10] = "Novembro";
            arrayMes[11] = "Dezembro";

            return arrayMes[index];
        }

        //recupera o modelo de atestado selecionado
        $scope.$watch('vm.modeloSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (vm.modeloSelecionado != undefined) {
                    var data = new Date();
                    var newData = moment(data).format("DD/MM/YYYY");
                    var hora = moment(data).format("HH");
                    var periodo = "";

                    if (hora >= 12 && hora < 18)
                        periodo = "Tarde";
                    else if (hora < 12)
                        periodo = "Manhã";
                    else
                        periodo = "Noite";

                    var mes = retornaMes(data.getMonth());

                    var montadata = moment(data).format("DD") + " de " + mes + " de " + moment(data).format("YYYY");

                    $scope.disabled = false;
                    var modelo = _.find(vm.modelosatestados, { IdModeloProntuario: vm.modeloSelecionado });
                    var texto = _.replace(modelo.Descricao, "@nomepaciente", paciente.NmPaciente);
                    texto = _.replace(texto, "@medico", $("#profissional option:selected").text());
                    texto = _.replace(texto, "@comparecimento ", newData);
                    texto = _.replace(texto, "@periodo", periodo);
                    texto = _.replace(texto, "@data", montadata);
                    vm.atestado.Descricao = texto;
                }
            }
        });

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.atestado.$valid) {
                vm.FormMessage = "";
               
                var blocker = blockUI.instances.get('blockModalAtestado');
                blocker.start();

                //seta o código d paciente
                vm.atestado.IdPaciente = paciente.IdPaciente;

                if (vm.profissionalSelecionado != undefined)
                    vm.atestado.IdFuncionario = vm.profissionalSelecionado;

                prontuarioservice
                    .saveAtestado(vm.atestado)
                    .then(function (result) {
                        vm.anamnese = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Atestado cadastrado com sucesso!");
                        else
                            notification.showSuccessBar("Atestado alterado com sucesso!");

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