(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('PacienteCrud', PacienteCrud);

    PacienteCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'ds.prontuario', 'ds.paciente', 'ds.cadastros', 'ds.estadoscidades', 'paciente'];

    function PacienteCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, dsProntuario, dsPaciente, dsCadastros, dsEstadoscidades, paciente) {

        var vm = this;
        vm.State = "Dados do Paciente";
        vm.paciente = {
            DadosNascimento: undefined
        };

        $scope.forms = {};
        vm.formValid = true;
        vm.FormCarteiras = [];
        vm.conveniopaciente = {
            NumeroCarteira: "",
            ValidadeCarteira: "",
            Plano: ""
        };

        vm.carteirasAdicionadas = [];

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.excluircarteira = excluircarteira;
        vm.addCarteira = addCarteira;

        //Feature Start
        init();

        //Implementations
        function init() {

            //vm.paciente = paciente;

            initDadosCombos();

            vm.FormMessage = "";

            var blocker = blockUI.instances.get('blockModalPaciente');
            blocker.start();

            var pHospitais = dsProntuario.getHospitais();
            pHospitais.then(function (result) {
                vm.hospitais = result.data;
            });

            var pConvenios = dsCadastros.getAllConvenios();
            pConvenios.then(function (result) {
                vm.convenios = result.data;
            });

            var pEstados = dsPaciente.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            var pPaciente = dsPaciente.getById(paciente.IdPaciente);
            pPaciente.then(function (result) {
                vm.paciente = result.data;
            });

            $q.all([pHospitais, pPaciente, pEstados, pConvenios]).then(function () {

                if (paciente.IdPaciente > 0) {
                    vm.dadosnascimento = vm.paciente.DadosNascimento;

                    if (vm.paciente.Estado != null || vm.paciente.Estado != "") {

                        var estado = _.find(vm.estados, { Uf: vm.paciente.Estado });
                        if (estado != null) {
                            vm.estadoSelecionado = estado.Id;
                            buscarCidadesByEstado(estado.Id, vm.paciente.Cidade);
                        }
                    }

                    if (vm.paciente.Situacao == "ATIVO")
                        vm.paciente.Situacao = "A";
                    else
                        vm.paciente.Situacao = "I";

                    if (vm.paciente.Sexo != undefined) {
                        var sexo = _.find(vm.sexo, { Key: vm.paciente.Sexo });
                        vm.sexoSelecionado = sexo.Key;
                    }

                    if (vm.paciente.Carteiras.length > 0)
                        vm.carteirasAdicionadas = vm.paciente.Carteiras;

                    if (vm.paciente.DadosNascimento != null) {

                        vm.paciente.DadosNascimento.HoraNascimento = moment(vm.paciente.DadosNascimento.HoraNascimento).format("HH:mm");


                        var hospital = _.find(vm.hospitais, { IdHospital: vm.paciente.DadosNascimento.IdHospital });
                        if (hospital != undefined)
                            vm.hospitalSelecionado = hospital.IdHospital;

                        var tpParto = _.find(vm.tipoparto, { Key: vm.paciente.DadosNascimento.TipoParto });
                        if (tpParto != undefined)
                            vm.tipoPartoSelecionado = tpParto.Key;

                        var aleitamento = _.find(vm.aleitamento, { Key: vm.paciente.DadosNascimento.AleitamentoMaternoPrimeiraHoraVida });
                        if (aleitamento != undefined)
                            vm.aleitamentoSelecionado = aleitamento.Key;

                        var assistiu = _.find(vm.assistiu, { Key: vm.paciente.DadosNascimento.AssistiuRecemNascidoRN });
                        if (assistiu != undefined)
                            vm.AssistiuSelecionado = assistiu.Key;

                        var tpsangue = _.find(vm.tiposanguineo, { Key: vm.paciente.DadosNascimento.TipoSanguineoRN });
                        if (tpsangue != undefined)
                            vm.tipoSanguineoSelecionado = tpsangue.Key;

                        var tpsanguemae = _.find(vm.tiposanguineo, { Key: vm.paciente.DadosNascimento.TipoSanguineoMae });
                        if (tpsanguemae != undefined)
                            vm.tipoSanguineoMaeSelecionado = tpsanguemae.Key;

                        var pezinho = _.find(vm.testepezinho, { Key: vm.paciente.DadosNascimento.TestePezinho });
                        if (pezinho != undefined)
                            vm.testepeSelecionado = pezinho.Key;

                        var reflexo = _.find(vm.testereflexo, { Key: vm.paciente.DadosNascimento.TesteReflexoVermelho });
                        if (reflexo != undefined)
                            vm.testereflexoSelecionado = reflexo.Key;
                    }
                }
                else {
                    vm.paciente.Situacao = "A";
                    vm.paciente.DtNascimento = "";
                    vm.dadosnascimento = {};
                }

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
                notification.showError(ex.data.Message);
            });
            //vm.State = "Editar Anamnese";


            //dsAnamnese
            //    .getById(id)
            //    .then(function (result) {
            //        vm.anamnese = result.data;
            //        vm.anamnese.Paciente = paciente.NmPaciente;
            //        console.log(result.data);
            //        if (result.data.Situacao == 'Ativo')
            //            vm.anamnese.Situacao = "A";
            //        else
            //            vm.anamnese.Situacao = "I";
            //    })
            //    .catch(function (ex) {
            //        exception.throwEx(ex);
            //    })['finally'](function () {
            //        blocker.stop();
            //    });

        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function buscarCidadesByEstado(idEstado, cidadeSelecionada) {
            dsPaciente
                .getCidadesByEstado(idEstado)
                .then(function (result) {
                    vm.cidades = result.data;
                    if (cidadeSelecionada != "") {
                        var cidade = _.find(vm.cidades, { Nome: cidadeSelecionada });
                        if (cidade != undefined)
                            vm.cidadeSelecionada = cidade.Id;
                    }
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });

        }

        function addCarteira() {
            vm.FormCarteiras = [];
            if (vm.convenioSelecionado == undefined)
                vm.FormCarteiras.push('Selecione o convênio!');

            if (vm.conveniopaciente.NumeroCarteira == "")
                vm.FormCarteiras.push('Preencha o campo Número da carteira!');

            if (vm.conveniopaciente.ValidadeCarteira == "")
                vm.FormCarteiras.push('Preencha o campo validade da carteira!');

            if (vm.conveniopaciente.Plano == "")
                vm.FormCarteiras.push('Preencha o campo plano!');

            if (vm.FormCarteiras.length == 0) {
                var convenio = _.find(vm.convenios, { IdConvenio: vm.convenioSelecionado });
                var cart = { ValidadeCarteira: vm.conveniopaciente.ValidadeCarteira, NumeroCarteira: vm.conveniopaciente.NumeroCarteira, Plano: vm.conveniopaciente.Plano, IdConvenio: vm.convenioSelecionado, Convenio: convenio.NomeConvenio };
                vm.carteirasAdicionadas.push(cart);
                resetCarteira();
            }
        }

        function resetCarteira() {
            vm.FormCarteiras = [];
            vm.convenioSelecionado = undefined;
            vm.conveniopaciente = {
                NumeroCarteira: "",
                ValidadeCarteira: "",
                Plano: ""
            };
        }

        function excluircarteira(item) {
            _.remove(vm.carteirasAdicionadas, item);
        }

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
        });


        function initDadosCombos() {
            vm.tipoparto = [
               { Key: 'Cesárea', Value: 'Cesárea' },
               { Key: 'Fórceps', Value: 'Fórceps' },
               { Key: 'Natural', Value: 'Natural' },
               { Key: 'Normal', Value: 'Normal' }
            ];

            vm.sexo = [
               { Key: 'Feminino', Value: 'Feminino' },
               { Key: 'Masculino', Value: 'Masculino' }
            ];

            vm.tiposanguineo = [
               { Key: 'A+', Value: 'A+' },
               { Key: 'A-', Value: 'A-' },
               { Key: 'B+', Value: 'B+' },
               { Key: 'B-', Value: 'B-' },
               { Key: 'AB+', Value: 'AB+' },
               { Key: 'AB-', Value: 'AB-' },
               { Key: 'O+', Value: 'O+' },
               { Key: 'O-', Value: 'O-' }
            ];

            vm.testepezinho = [
                { Key: 'S', Value: 'Sim' },
                { Key: 'N', Value: 'Não' }
            ];

            vm.assistiu = [
                { Key: 'Pediatra', Value: 'Pediatra' },
                { Key: 'Enfermeira', Value: 'Enfermeira' },
                { Key: 'Parteira', Value: 'Parteira' },
                { Key: 'Outro', Value: 'Outro' }
            ];

            vm.aleitamento = [
                { Key: 'S', Value: 'Sim' },
                { Key: 'N', Value: 'Não' }
            ];

            vm.testereflexo = [
                { Key: 'S', Value: 'Sim' },
                { Key: 'N', Value: 'Não' }
            ];
        }

        function validaAbas(a, b, c) {
            var aba1 = angular.element(document.querySelector('#tabdados'));
            a == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#liPaciente'));
            a == "A" ? li1.addClass("active") : li1.removeClass("active");

            var aba2 = angular.element(document.querySelector('#tabendereco'));
            b == "A" ? aba2.addClass("active") : aba2.removeClass("active");
            var li2 = angular.element(document.querySelector('#liEndereco'));
            b == "A" ? li2.addClass("active") : li2.removeClass("active");

            var aba3 = angular.element(document.querySelector('#tabnascimento'));
            c == "A" ? aba3.addClass("active") : aba3.removeClass("active");
            var li3 = angular.element(document.querySelector('#liNascimento'));
            c == "A" ? li3.addClass("active") : li3.removeClass("active");
            //remove a class active da tab de carteiras
            var licarteiras = angular.element(document.querySelector('#liCarteiras'));
            licarteiras.removeClass("active");
            var tacCarteiras = angular.element(document.querySelector('#tabconvenios'));
            tacCarteiras.removeClass("active");
        }

        function save() {

            var formNascimento = common.validateForm($scope.forms.dadosnascimento);
            var formEndereco = common.validateForm($scope.forms.dadosendereco);
            var formPaciente = common.validateForm($scope.forms.dadospaciente);

            if (!formPaciente) {
                validaAbas("A", "I", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
            } else if (!formEndereco) {
                validaAbas("I", "A", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
            } else if (!formNascimento) {
                validaAbas("I", "I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Preencha os campos em vermelho.";
            }
            else {

                if (vm.tipoPartoSelecionado != undefined)
                    vm.dadosnascimento.TipoParto = vm.tipoPartoSelecionado;

                if (vm.tipoSanguineoSelecionado != undefined)
                    vm.dadosnascimento.TipoSanguineoRN = vm.tipoSanguineoSelecionado;

                if (vm.AssistiuSelecionado != undefined)
                    vm.dadosnascimento.AssistiuRecemNascidoRN = vm.AssistiuSelecionado;

                if (vm.aleitamentoSelecionado != undefined)
                    vm.dadosnascimento.AleitamentoMaternoPrimeiraHoraVida = vm.aleitamentoSelecionado;

                if (vm.tipoSanguineoMaeSelecionado != undefined)
                    vm.dadosnascimento.TipoSanguineoMae = vm.tipoSanguineoMaeSelecionado;

                if (vm.testepeSelecionado != undefined)
                    vm.dadosnascimento.TestePezinho = vm.testepeSelecionado;

                if (vm.testereflexoSelecionado != undefined)
                    vm.dadosnascimento.TesteReflexoVermelho = vm.testereflexoSelecionado;

                if (vm.hospitalSelecionado != undefined)
                    vm.dadosnascimento.IdHospital = vm.hospitalSelecionado;

                //salva dados estado e cidade

                var est = _.find(vm.estados, { Id: vm.estadoSelecionado });
                if (est != undefined)
                    vm.paciente.Estado = est.Uf;

                var cid = _.find(vm.cidades, { Id: vm.cidadeSelecionada });
                if (cid != undefined)
                    vm.paciente.Cidade = cid.Nome;


                if (vm.sexoSelecionado != undefined)
                    vm.paciente.Sexo = vm.sexoSelecionado;

                if (vm.carteirasAdicionadas.length > 0)
                    vm.paciente.Carteiras = vm.carteirasAdicionadas;

                vm.paciente.DadosNascimento = vm.dadosnascimento;


                vm.FormMessage = "";
                var blocker = blockUI.instances.get('blockModalPaciente');
                blocker.start();

                dsPaciente
                        .salvarPaciente(vm.paciente)
                        .then(function (result) {
                            vm.paciente = result.data;
                            if (paciente != undefined)
                                notification.showSuccessBar("Paciente cadastrado com sucesso!");
                            else
                                notification.showSuccessBar("Paciente alterado com sucesso!");

                            $modalInstance.close();
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