(function () {
    'use strict';

    angular
        .module('app.financeiro')
        .controller('ChequeCrud', ChequeCrud);

    ChequeCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'cadastroservice', 'financeiroservice', 'id'];

    function ChequeCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, cadastroservice, financeiroservice, id) {

        var vm = this;
        vm.State = "Incluir Cheque";
        vm.FormMessage = "";
        vm.especialidades = {};

        $scope.forms = {};
        vm.formValid = true;
        vm.cheque = {};
        vm.idpessoaSelecionada = 0;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.pessoa = pessoa;
        vm.financeiro = financeiro;

        //Feature Start
        init();

        //Implementations
        function init() {
            vm.FormMessage = "";
            var pBancos = financeiroservice.listarbancos();
            pBancos.then(function (result) {
                vm.bancos = result.data;
            });

            vm.situacoes = [
                { Key: "Depositado", Value: "Depositado" },
                { Key: "Emitido", Value: "Emitido" },
                { Key: "Devolvido", Value: "Devolvido" }
            ];

            $q.all([pBancos]).then(function () {
                if (id > 0) {
                    vm.State = "Editar Cheque";
                    var blocker = blockUI.instances.get('blockModalCrudCheque');
                    blocker.start();
                    financeiroservice
                        .getChequeById(id)
                        .then(function (result) {
                            vm.cheque = result.data;

                            if (result.data.Banco != null) {
                                var banco = _.find(vm.bancos, { NomeBanco: result.data.Banco });
                                if (banco != null)
                                    vm.cheque.Banco = banco.NomeBanco;
                            }

                            vm.cheque.IdPessoa = result.data.IdPessoa;
                            vm.pessoaselecionada = result.data.NomePessoa;
                            vm.idpessoaSelecionada = result.data.IdPessoa;

                        })
                        .catch(function (ex) {
                            notification.showError(ex.data.Message);
                        })['finally'](function () {
                            blocker.stop();
                        });
                }
                else {
                    vm.SituacaoA = "A";
                }
            })['finally'](function () {
            }).catch(function (ex) {
            });
        }

        function pessoa() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/busca.paciente.html',
                controller: 'BuscaPaciente as vm',
                size: 'lg',
                backdrop: 'static'
            });

            modalInstance.result.then(function (item) {
                vm.cheque.IdPessoa = item.Pessoa.IdPessoa;
                vm.pessoaselecionada = item.Pessoa.Nome;
                vm.idpessoaSelecionada = item.Pessoa.IdPessoa;
                if (item != null)
                    $scope.forms.cheques.pessoa.$invalid = false;
            });
        }

        function financeiro() {
            //var modalInstance = $modal.open({
            //    templateUrl: 'app/paciente/busca.paciente.html',
            //    controller: 'BuscaPaciente as vm',
            //    size: 'lg',
            //    backdrop: 'static'
            //});

            //modalInstance.result.then(function (item) {
            //    //vm.cheque.IdFinanceiro = item.IdFinanceiro;
            //    //vm.financeiroselecionado = item.Tipo;
            //});
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function save() {
            $scope.showErrorsCheckValidity = true;
            if ($scope.forms.cheques.$valid && vm.idpessoaSelecionada > 0) {
                vm.FormMessage = "";

                var blocker = blockUI.instances.get('blockModalCrudCheque');
                blocker.start();

                financeiroservice
                    .salvarCheque(vm.cheque)
                    .then(function (result) {
                        vm.cheque = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        notification.showError(ex.data.Message);
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {

                if (vm.idpessoaSelecionada == 0)
                { $scope.forms.cheques.pessoa.$invalid = true; }
                else { $scope.forms.cheques.pessoa.$invalid = false; }

                vm.FormMessage = "Preencha os campos em vermelho.";
            }
        }

    }
})();