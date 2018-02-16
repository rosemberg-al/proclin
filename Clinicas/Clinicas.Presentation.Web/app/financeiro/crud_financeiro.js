(function () {
    'use strict';

    angular
    .module('app.financeiro')
    .controller('CrudFinanceiro', CrudFinanceiro);

    CrudFinanceiro.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'exception', 'DTInstances', 'ds.atestado', 'ds.funcionario', 'paciente', '$stateParams'];

    function CrudFinanceiro($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, exception, DTInstances, dsAtestado, dsFuncionario, paciente, $stateParams) {
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
        vm.cancelar = cancelar;
        vm.getpessoa = getpessoa;
        vm.validatotal = validatotal;

        init();

        function init() {

            if ($stateParams.tipo == "P") {
                vm.titulo = "Contas a pagar";
                vm.tipoContas = "D";
                vm.tituloPessoa = "Pagar para";
            }
            else {
                vm.titulo = "Contas a receber";
                vm.tipoContas = "R";
                vm.tituloPessoa = "Receber de";
            }

            //var pContas = contaservice.listar().success(function (result, status) {
            //    vm.contas = result;
            //});

            //var pTipos = contratoservice.listarplanodecontas(vm.tipoContas).success(function (result, status) {
            //    vm.planos = result;
            //});
            vm.tipos = [{ tp: 'A', desc: "à vista" }, { tp: 'P', desc: "parcelado" }];

            var pContas = dsFinanceiro.listarContas();
            pContas.then(function (result) {
                vm.contas = result.data;
            });

            var pTipos = dsFinanceiro.listarplanodecontas(vm.tipoContas);
            pTipos.then(function (result) {
                vm.planos = result.data;
            });
            var blocker = blockUI.instances.get('blockModalCFinanc');
            blocker.start();

            $q.all([pContas, pTipos]).then(function () {

               

                if ($stateParams.id > 0) {
                    dsFinanceiro
                       .getFinanceiroById($stateParams.id)
                       .then(function (result) {
                           vm.financeiro = result.data;

                           var conta = _.find(vm.contas, { id: vm.financeiro.IdConta });
                           if (conta != null)
                               vm.contaSelecionada = conta.IdConta;

                           var plano = _.find(vm.planos, { id: vm.financeiro.IdPlanoConta });
                           if (plano != null)
                               vm.planoSelecionado = plano.IdPlanoConta;

                           vm.idpessoaSelecionada = vm.financeiro.IdPessoa;
                           vm.nomepessoaSelecionada = vm.financeiro.NomePessoa;
                           vm.parcelasAdicionadas = vm.financeiro.Parcelas;
                       })
                       .catch(function (ex) {
                           exception.throwEx(ex);
                       })['finally'](function () {
                       });

                } else {
                    vm.TipoPessoaSelecionado = "PF";
                }

            })['finally'](function () {
                blockUI.stop();
            }).catch(function (ex) {
            });

        }

        function getpessoa() {

            var modalInstance = $modal.open({
                templateUrl: 'app/funcionario/crud.funcionario.html',
                controller: 'FuncionarioCrud as vm',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return id;
                    },
                }
            });

            modalInstance.result.then(function (obj) {
                vm.pessoaSelecionada = obj;
                vm.idpessoaSelecionada = obj.IdPessoa;
                vm.nomepessoaSelecionada = obj.Nome;
            });
        }


        function salvar() {

            var formFuncionadio = common.validateForm($scope.forms.dadosfuncionario);

            if (formFuncionadio) {

                if (vm.planoSelecionado != undefined)
                    vm.financeiro.IdPlanoConta = vm.planoSelecionado;

                if (vm.contaSelecionada != null)
                    vm.financeiro.IdConta = vm.contaSelecionada;

                if (vm.tipopagamentoSelecionado != null)
                    vm.financeiro.FormaPagamento = vm.tipopagamentoSelecionado == 'A' ? 'A VISTA' : 'A PRAZO';


                if (vm.parcelasAdicionadas.length > 0) {
                    vm.financeiro.Parcelas = [];
                    _.forEach(vm.parcelasAdicionadas, function (item) {
                        var parcela = { NumeroParcela: item.NumeroParcela, Valor: item.Valor, DataVencimento: moment(item.DataVencimento).format("DD/MM/YYYY") };
                        vm.financeiro.Parcelas.push(parcela);
                    });
                }

                vm.financeiro.IdPessoa = vm.idpessoaSelecionada;
                vm.financeiro.Tipo = vm.tipoContas;


                var blocker = blockUI.instances.get('blockModalCFinanc');
                blocker.start();

                dsFuncionario
                    .saveFinanceiro(vm.financeiro)
                    .then(function (result) {
                        vm.financeiro = result.data;
                        if ($stateParams.tipo == "R") {
                            notification.showSuccessBar("Conta a receber cadastrada com sucesso");
                            $state.go("receber", { tipo: 'R' });
                        }
                        else {
                            notification.showSuccessBar("Conta a pagar cadastrada com sucesso");
                            $state.go("pagar", { tipo: 'P' });
                        }
                    })
                    .catch(function (ex) {
                        exception.throwEx(ex);
                    })['finally'](function () {
                        blocker.stop();
                    });

            } else {
                vm.msgalert = 'Existem campos obrigatórios sem o devido preenchimento';
                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        }

        function cancelar() {
            if ($stateParams.tipo == "R")
                $state.go("receber", { tipo: 'R' });
            else
                $state.go("pagar", { tipo: 'P' });

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

                dsFinanceiro
                      .gerarparcelas(parcela)
                      .then(function (result) {
                          vm.parcelasAdicionadas = result.data;
                      })
                      .catch(function (ex) {
                          exception.throwEx(ex);
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