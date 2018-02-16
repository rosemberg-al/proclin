 (function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('CrudAtestado', CrudAtestado);

    CrudAtestado.$inject = ['$scope', '$http', '$q',  '$modal','$modalInstance',  'DTOptionsBuilder','textAngularManager', 'blockUI', 'common', 'notification','cadastroservice', 'pacienteservice','prontuarioservice','idpaciente'];

    function CrudAtestado($scope, $http, $q,  $modal, $modalInstance,  DTOptionsBuilder,textAngularManager, blockUI, common, notification,cadastroservice, pacienteservice,prontuarioservice,idpaciente) {

        var vm = this;
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.selecionarModelo = selecionarModelo;

        vm.atestado = { };

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;

        $scope.version = textAngularManager.getVersion();
        $scope.versionNumber = $scope.version.substring(1);
        $scope.orightml = "";
        $scope.htmlcontent = $scope.orightml;
        $scope.disabled = false;


        init();
        
        function init() 
        {
             var blocker = blockUI.instances.get('blockModalHP');
            
             blocker.start();
             pacienteservice
                .getPacienteById(idpaciente)
                .then(function (result) {
                    vm.paciente = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 


            blocker.start();
            prontuarioservice
                    .listarModeloProntuarioPorTipo('Atestado')
                    .then(function (result) {
                        vm.modelos = result.data;
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            
            blocker.start();
            cadastroservice
                .listarProfissionaisSaude()
                .then(function (result) {
                    vm.profissionais = result.data;                    
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                }); 
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {
                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalHP');
                blocker.start();

                vm.atestado.IdPaciente = vm.paciente.IdPaciente;
                vm.atestado.IdFuncionario = vm.profissionalSelecionado;

                prontuarioservice
                    .saveAtestado(vm.atestado)
                    .then(function (result) {
                        vm.historia = result.data;
                        notification.showSuccess("Atestado cadastrado com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento.";
            }
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


      function selecionarModelo() {

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
                    var modelo = _.find(vm.modelos, { IdModeloProntuario: vm.modeloSelecionado });
                    var texto = _.replace(modelo.Descricao, "@nomepaciente", vm.paciente.Nome);
                    texto = _.replace(texto, "@medico", $("#profissionalSelecionado option:selected").text());
                    texto = _.replace(texto, "@comparecimento ", newData);
                    texto = _.replace(texto, "@periodo", periodo);
                    texto = _.replace(texto, "@data", montadata);
                    vm.atestado.Descricao = texto;
           }
        }
    }
})();