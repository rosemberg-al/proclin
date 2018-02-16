(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('CrudFinanceiro', CrudFinanceiro);

    CrudFinanceiro.$inject = ['$scope', '$state', '$http', '$q', '$modal','$modalInstance', 'blockUI', 'common', 'notification', 'financeiroservice', 'tipo'];

    function CrudFinanceiro($scope, $state, $http, $q, $modal, $modalInstance, blockUI, common, notification, financeiroservice, tipo) {
        var vm = this;

        vm.titulo = "Contas a pagar";


        vm.tipopagamentoSelecionado = undefined;
        vm.parcelasAdicionadas = [];
        vm.pessoaSelecionada = undefined;
        vm.idpessoaSelecionada = 0;
        $scope.forms = {};
        vm.formValid = true;

        vm.init = init;
        vm.salvar = salvar;
        vm.geraParcelas = geraParcelas;
        vm.cancel = cancel;


        vm.getpessoa = getpessoa;
        vm.validatotal = validatotal;

        init();

        function init() {

            if (tipo == "P") {
                vm.titulo = "Contas a pagar";
                vm.tipoContas = "D";
                vm.tituloPessoa = "Pagar para";
                common.setBreadcrumb('Financeiro .Contas a pagar');
            }
            else {
                vm.titulo = "Contas a receber";
                vm.tipoContas = "R";
                vm.tituloPessoa = "Receber de";
                common.setBreadcrumb('Financeiro .Contas a receber');
            }

            vm.tipos = [{ tp: 'A', desc: "A Vista" }, { tp: 'P', desc: "A Prazo" }];

            var pContas = financeiroservice.listarContas();
            pContas.then(function (result) {
                vm.contas = result.data;
            });

            var pTipos = financeiroservice.listarPlanodeContas();
            pTipos.then(function (result) {
                vm.planos = result.data;
            });
        }

        function getpessoa() {

            var modalInstance = $modal.open({
                templateUrl: 'app/pessoa/busca.pessoa.html',
                controller: 'BuscaPessoa as vm',
                size: 'lg',
                backdrop: 'static'
            });

            modalInstance.result.then(function (item) {
                vm.pessoaSelecionada = item;
                vm.idpessoaSelecionada = item.IdPessoa;
                vm.nomepessoaSelecionada = item.Nome;
                if (item != null)
                    $scope.forms.dadosfinanceiro.pessoa.$invalid = false;
            });
        }


        function salvar() {

            $scope.showErrorsCheckValidity = true;

            if ($scope.forms.dadosfinanceiro.$valid && vm.idpessoaSelecionada > 0) {

                if (vm.planoSelecionado != undefined)
                    vm.financeiro.IdPlanoConta = vm.planoSelecionado;

                if (vm.contaSelecionada != null)
                    vm.financeiro.IdConta = vm.contaSelecionada;

                if (vm.tipopagamentoSelecionado != null)
                    vm.financeiro.FormaPagamento = vm.tipopagamentoSelecionado == 'A' ? 'A VISTA' : 'A PRAZO';


                if (vm.parcelasAdicionadas.length > 0) {
                    vm.financeiro.Parcelas = [];
                    _.forEach(vm.parcelasAdicionadas, function (item) {
                        var parcela = { NumeroParcela: item.NumeroParcela, Valor: item.Valor, DataVencimento: item.DataVencimento };
                        vm.financeiro.Parcelas.push(parcela);
                    });
                }

                vm.financeiro.IdPessoa = vm.idpessoaSelecionada;
                vm.financeiro.Tipo = tipo;


                var blocker = blockUI.instances.get('blockModalCFinanc');
                blocker.start();

                financeiroservice
                    .salvarFinanceiro(vm.financeiro)
                    .then(function (result) {
                        vm.financeiro = result.data;
                        if (tipo == "R") {
                            notification.showSuccessBar("Conta a receber cadastrada com sucesso");
                              $modalInstance.close();
                        }
                        else {
                            notification.showSuccessBar("Conta a pagar cadastrada com sucesso");
                             $modalInstance.close();
                        }
                    })
                    .catch(function (ex) {
                    })['finally'](function () {
                        blocker.stop();
                    });

            } else {
                vm.msgalert = 'Existem campos obrigatórios sem o devido preenchimento';

                if (vm.idpessoaSelecionada == 0)
                { $scope.forms.dadosfinanceiro.pessoa.$invalid = true; }
                else { $scope.forms.dadosfinanceiro.pessoa.$invalid = false; }

                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        }

       /*  function cancelar() {
            if (tipo == "R")
                $state.go("receber", { tipo: 'R' });
            else
                $state.go("pagar", { tipo: 'P' });

        } */

        function cancel() {
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

        function geraParcelas() {
            if (vm.financeiro.QuantidadeParcelas == undefined || vm.financeiro.Total == undefined || vm.financeiro.DataVencimento == undefined) {
                notification.showErrorTop("Para gerar as parcelas preencha os campos em vermelho");
            }
            else {
                var parcela = { QuantidadeParcelas: vm.financeiro.QuantidadeParcelas, ValorAdesao: 0, DataVencimentoAdesao: null, Valor: vm.financeiro.Total, DataVencimentoPrimeiraParcela: vm.financeiro.DataVencimento };

                financeiroservice
                      .gerarparcelas(parcela)
                      .then(function (result) {
                          vm.parcelasAdicionadas = result.data;
                      })
                      .catch(function (ex) {
                      })['finally'](function () {
                      });
            }

        }


        $scope.$watch('vm.tipopagamentoSelecionado', function (newValue, oldValue) {
            if (angular.isDefined(newValue)) {
                if (vm.tipopagamentoSelecionado == 'A') {
                    vm.parcelasAdicionadas = [];
                    vm.financeiro.QuantidadeParcelas = undefined;
                }
            }
        });

        //$scope.$watchCollection('vm.parcelasAdicionadas', function (newValue, oldValue) {
        //    if (angular.isDefined(newValue)) {
        //    }
        //});


    }
})();