 (function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('CrudReceituario', CrudReceituario);

    CrudReceituario.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'DTOptionsBuilder', 'textAngularManager', 'blockUI', 'common', 'notification', 'cadastroservice', 'pacienteservice', 'prontuarioservice', 'medicamentoservice','idpaciente'];

    function CrudReceituario($scope, $http, $q, $modal, $modalInstance, DTOptionsBuilder, textAngularManager, blockUI, common, notification, cadastroservice, pacienteservice, prontuarioservice, medicamentoservice, idpaciente) {

        var vm = this;
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.selecionarModelo = selecionarModelo;
        vm.add = add;
        vm.getprescricao = getprescricao;
        vm.deletar = deletar;
        vm.selecionado = selecionado;
        vm.addModelo = addModelo;

        vm.Receituario = { };

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        vm.prescricaoSelecionada = undefined;
        vm.unidades = 1;
        vm.adicionados = [];

        $scope.version = textAngularManager.getVersion();
        $scope.versionNumber = $scope.version.substring(1);
        $scope.orightml = "";
        $scope.htmlcontent = $scope.orightml;
        $scope.disabled = false;
        vm.itensadicionados = "";


        init();
        
        function init() 
        {
             var blocker = blockUI.instances.get('blockModalHP');
             blocker.start();

             var pPaciente = pacienteservice.getPacienteById(idpaciente);
             pPaciente.then(function (result) {
                 vm.paciente = result.data;         
             });

             var pModelo = prontuarioservice.listarModeloProntuarioPorTipo('Receituario')
             pModelo.then(function (result) {
                 vm.modelos = result.data;
             });

             var pProfissionais = cadastroservice.listarProfissionaisSaude();
             pProfissionais.then(function (result) {
                 vm.profissionais = result.data; 
             });


             $q.all([pPaciente, pModelo, pProfissionais]).then(function () {

             })['finally'](function () {
                 blocker.stop();
             }).catch(function (ex) {

             });
        }

        function selecionado(item) {
            vm.prescricao = item.Posologia;
        }


        function deletar(item) {
            _.remove(vm.adicionados, item);
        }

        function addModelo() {
            var modalInstance = $modal.open({
                templateUrl: 'app/prontuarionovo/add.modelo.receituario.html',
                controller: 'AddModeloController as vm',
                size: 'xl',
                backdrop: 'static',
                resolve: {
                    descricao: function () {
                        return vm.itensadicionados;
                    },
                }
            });

            modalInstance.result.then(function () {
                init();
            });
        }

        function getprescricao(nome) {

            if (nome.length < 3)
                return;

            medicamentoservice
                .pesquisarMedicamentos(nome)
                .then(function (result) {
                    vm.medicamentos = result.data;
                })
                .catch(function (ex) {
                    vm.FormMessage = ex.Message;
                })['finally'](function () {
                    blocker.stop();
                });
        }

        $scope.$watch('vm.prescricaoSelecionada', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.prescricao = vm.prescricaoSelecionada.Posologia;
            }
        });

        function add() {
            $scope.showErrorsCheckValidity = true;
            var formdados = $scope.forms.medicamentos.$valid;

            if (formdados) {
                $scope.forms.medicamentos.prescricaoSelecionada.$invalid = false;
                vm.FormMessage = "";
                /*var proc = _.find(vm.adicionados, { IdMedicamento: vm.prescricaoSelecionada.IdMedicamento });
                if (proc != null) {
                    notification.showError("Medicamento já foi adicionado!");
                }*/
                //else {
                    var model = {
                        Medicamento: vm.prescricaoSelecionada,
                        Quantidade: vm.unidades,
                        IdMedicamento: vm.prescricaoSelecionada.IdMedicamento
                    };
                    vm.itensadicionados = vm.itensadicionados + "<div style='width:85%; float:left'>" + vm.prescricaoSelecionada.Nome + "</div >  <div style='width:15%; float:left'>" + vm.unidades + " unidade(s)</div > <div > " + vm.prescricaoSelecionada.Posologia + " </div>" ;
                    vm.adicionados.push(model);
                    vm.prescricaoSelecionada = undefined;
                    vm.unidades = 1;
                    vm.prescricao = "";
                   
                //}
            }
            else {
                $scope.forms.medicamentos.prescricaoSelecionada.$invalid = true;
                vm.FormMessage = "";
                vm.FormMessage = "Você deve selecionar um medicamento para adicionar!";
            }
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.dados.$valid) {

                if (vm.adicionados.length == 0) {
                    vm.FormMessage = "Você tem que adicionar medicamentos ao receituário!";
                    return;
                }

                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalHP');
                blocker.start();

                vm.Receituario.IdPaciente = vm.paciente.IdPaciente;
                vm.Receituario.IdFuncionario = vm.profissionalSelecionado;

                var medicamentos = [];

                //adiciono os medicamentos ao receituario para salvar
                _.forEach(vm.adicionados, function (item) {
                    var novo = {
                        IdReceituario: 0,
                        IdMedicamento: item.IdMedicamento,
                        Nome: item.Medicamento.Nome,
                        Posologia: item.Medicamento.Posologia,
                        Quantidade: item.Quantidade
                    };
                    medicamentos.push(novo);
                });

                vm.Receituario.Medicamentos = medicamentos;

                vm.Receituario.Descricao = vm.itensadicionados;

                prontuarioservice
                    .saveReceituario(vm.Receituario)
                    .then(function (result) {
                        vm.historia = result.data;
                        notification.showSuccess("Receituario cadastrado com sucesso");

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


      function selecionarModelo(item) {
          var modelo = _.find(vm.modelos, { IdModeloProntuario: item });
          if (modelo != null) {
              vm.itensadicionados = modelo.Descricao;
          }
        }
    }
})();