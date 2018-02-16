(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('EditarParcela', EditarParcela);

    EditarParcela.$inject = ['$scope', '$state', '$http', '$q', 'blockUI', 'common', '$modal', '$modalInstance', 'notification', 'financeiroservice','tipo', 'idparcela','idfinanceiro'];
    function EditarParcela($scope, $state, $http, $q, blockUI, common, $modal, $modalInstance, notification, financeiroservice,tipo, idparcela,idfinanceiro) {
        var vm = this;
        vm.titulo = "Editar parcela";
        vm.tipopagamentoSelecionado = undefined;
        vm.parcela = {
            ValorAcerto: "",
            ValorAcrescimo: 0,
            ValorDesconto: 0
        };

        $scope.forms = {};
        vm.formValid = true;

        vm.init = init;
        vm.salvar = salvar;
        vm.cancelar = cancelar;
        vm.validatotal = validatotal;
        vm.calcularDesconto = calcularDesconto;
        vm.calcularAcrescimo = calcularAcrescimo;

        init();

        function init() {

            if (tipo == "P") {
                vm.titulo = "Contas a pagar";
                vm.tipoContas = "D";
                vm.tituloPessoa = "Pagar para";
                common.setBreadcrumb('Financeiro .Parcela Contas a pagar');
            }
            else {
                vm.titulo = "Contas a receber";
                vm.tipoContas = "R";
                vm.tituloPessoa = "Receber de";
                common.setBreadcrumb('Financeiro .Parcela Contas a receber');
            }

            var pContas = financeiroservice.listarContas();
            pContas.then(function (result) {
                vm.contas = result.data;
            });

            var pMeios = financeiroservice.listarmeiospagamento();
            pMeios.then(function (result) {
                vm.meiospagamentos = result.data;
            });

            var pTipos = financeiroservice.listarPlanodeContas();
            pTipos.then(function (result) {
                vm.planos = result.data;
            });

            vm.situacoes = [{ Key: 'Aberto', Value: 'Aberto' }, { Key: 'Baixado', Value: 'Baixado' }];

            var blocker = blockUI.instances.get('blockModalEditParcela');
            blocker.start();

            $q.all([pContas, pTipos, pMeios]).then(function () {

                if (idparcela > 0) {
                    financeiroservice
                       .getParcelaById(idparcela)
                       .then(function (result) {
                           vm.parcela = result.data;

                           var conta = _.find(vm.contas, { IdConta: vm.parcela.IdConta });
                           if (conta != null)
                               vm.contaSelecionada = conta.IdConta;

                           var plano = _.find(vm.planos, { IdPlanoConta: vm.parcela.IdPlanoConta });
                           if (plano != null)
                               vm.planoSelecionado = plano.IdPlanoConta;

                           var situacao = _.find(vm.situacoes, { Value: vm.parcela.Situacao });
                           if (situacao != null)
                               vm.situacaoSelecionada = situacao.Value;

                           var meio = _.find(vm.meiospagamentos, { IdMeioPagamento: vm.parcela.IdMeioPagamento });
                           if (meio != null)
                               vm.tipopagamentoSelecionado = meio.IdMeioPagamento;

                           vm.parcela.ValorAcerto = vm.parcela.Valor;


                           if (vm.parcela.ValorAcerto != undefined && vm.parcela.ValorDesconto != undefined)
                               vm.parcela.TotalAcerto = parseFloat(vm.parcela.TotalAcerto) - parseFloat(vm.parcela.ValorDesconto);

                           if (vm.parcela.ValorAcerto != undefined && vm.parcela.ValorAcrescimo != undefined)
                               vm.parcela.TotalAcerto = parseFloat(vm.parcela.TotalAcerto) + parseFloat(vm.parcela.ValorAcrescimo);

                           if (vm.parcela.ValorDesconto == null)
                               vm.parcela.ValorDesconto = 0;

                           if (vm.parcela.ValorAcrescimo == null)
                               vm.parcela.ValorAcrescimo = 0;
                       })
                       .catch(function (ex) {
                           
                       })['finally'](function () {
                       });

                } else {
                    vm.TipoPessoaSelecionado = "PF";
                }

            })['finally'](function () {
                blocker.stop();
            }).catch(function (ex) {
            });
        }

        function calcularDesconto() {
            if (vm.parcela.ValorAcerto != undefined && vm.parcela.ValorDesconto != undefined) {
                if (vm.parcela.TotalAcerto == undefined)
                    vm.parcela.TotalAcerto = vm.parcela.ValorAcerto;

                vm.parcela.TotalAcerto = parseFloat(vm.parcela.ValorAcerto) - parseFloat(vm.parcela.ValorDesconto) + parseFloat(vm.parcela.ValorAcrescimo);

            }
        }

        function calcularAcrescimo() {
            if (vm.parcela.ValorAcerto != undefined && vm.parcela.ValorAcrescimo != undefined) {
                if (vm.parcela.TotalAcerto == undefined)
                    vm.parcela.TotalAcerto = vm.parcela.ValorAcerto;

                vm.parcela.TotalAcerto = parseFloat(vm.parcela.ValorAcerto) + parseFloat(vm.parcela.ValorAcrescimo) - parseFloat(vm.parcela.ValorDesconto);
            }
        }

        function salvar() {

             vm.situacaoSelecionada = "Baixado"

            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.editparcela.$valid) {
                vm.msgalert = "";
                //valida baixa

               
                if (vm.situacaoSelecionada != undefined) {
                    vm.parcela.Situacao = vm.situacaoSelecionada;

                    

                    if (vm.situacaoSelecionada == "Baixado") {
                        if (vm.parcela.DataAcerto == undefined || vm.parcela.DataAcerto.length == 0) {
                            notification.showErrorTop("A data do acerto é obrigatória para fazer a baixa.");
                            $scope.forms.editparcela.datavencAcerto.$invalid = true;
                            return;
                        }
                    }
                }

                $scope.forms.editparcela.datavencAcerto.$invalid = false;


                if (vm.planoSelecionado != undefined)
                    vm.parcela.IdPlanoConta = vm.planoSelecionado;

                if (vm.contaSelecionada != undefined)
                    vm.parcela.IdConta = vm.contaSelecionada;

                if (vm.tipopagamentoSelecionado != undefined)
                    vm.parcela.IdMeioPagamento = vm.tipopagamentoSelecionado;


                vm.parcela.Tipo = vm.tipoContas;

                var blocker = blockUI.instances.get('blockModalEditParcela');
                blocker.start();


                financeiroservice
                    .alterarParcela(vm.parcela)
                    .then(function (result) {
                        vm.parcela = result.data;

                        if (tipo == "R") {
                            notification.showSuccessBar("Parcela alterada com sucesso");
                            $modalInstance.close();
                        }
                        else {
                            notification.showSuccessBar("Parcela alterada com sucesso");
                            $modalInstance.close();
                        }
                    })
                    .catch(function (ex) {
                        
                        vm.msgalert = ex.data.Message;
                    })['finally'](function () {
                        blocker.stop();
                    });


            } else {
                vm.msgalert = 'Existem campos obrigatórios sem o devido preenchimento';
                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        }

        function cancelar() {
            /*
            if ($stateParams.tipo == "R")
                $state.go("receber", { tipo: 'R' });
            else
                $state.go("pagar", { tipo: 'P' });*/
            $modalInstance.dismiss('cancel');
        }


        function validatotal() {
            var total = 0;
            _.forEach(vm.parcelasAdicionadas, function (item) {
                total = parseFloat(total) + parseFloat(item.Valor);
            });

            if (total != vm.financeiro.Total)
                notification.showErrorTop("A soma das parcelas não correspondem ao total.");
        }


        $scope.$watch('vm.tipopagamentoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (vm.tipopagamentoSelecionado == 'A') {
                    vm.parcelasAdicionadas = [];
                    vm.financeiro.QuantidadeParcelas = undefined;
                }
            }
        });

        $scope.$watch('vm.parcela.ValorAcerto', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                vm.parcela.TotalAcerto = vm.parcela.ValorAcerto;
                calcularDesconto();
                calcularAcrescimo();
            }
        });


    }
})();