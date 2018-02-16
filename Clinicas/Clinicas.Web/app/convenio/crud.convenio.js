(function () {
    'use strict';

    angular
        .module('app.convenio')
        .controller('convenioCrud', convenioCrud);

    convenioCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'FileUploader', 'blockUI', 'common', 'notification', 'cadastroservice', 'comumservice', 'servicebase', 'id'];

    function convenioCrud($scope, $http, $q, $modal, $modalInstance, FileUploader, blockUI, common, notification, cadastroservice, comumservice, servicebase, id) {

        var vm = this;
        vm.State = "Incluir Convênio";
        vm.FormMessage = "";
        vm.convenio = {};
        vm.novo = false;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.alterartipo = alterartipo; // Alterar tipo pessoa 

        //Feature Start
        init();

        vm.uploader = new FileUploader({
            url: servicebase.urlApi() + '/cadastros/uploadthumbNail?idconveio=' + id,
            queueLimit: 1,
            method: "POST",
            //formData: { model: vm.item },
            //headers: { Authorization: dsSession.getAuthorization() },
            //withCredentials: true
        });


        vm.uploader.onCompleteItem = function (fileItem, response, status, headers) {
            if (status == 200) {
                notification.showSuccess("Logo adicionada com sucesso.");
                if (vm.novo)
                    $modalInstance.close();
                
                //vm.item.imagem = response;
                //save();
                //$uibModalInstance.close();
            }
            else { notification.showError("Erro ao adicionar a imagem."); }
        };

        vm.uploader.filters.push({
            name: 'customFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                var r = /\.([^./]+)$/.exec(item.name);
                var extensao = r && r[1] || '';

                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                if ('|png|jpg|jpeg|'.indexOf(type) !== -1) {
                    return true;
                }
                else {
                    notification.showError('Somente é permitido arquivos dos tipos (.png, .jpg e .jpeg).');
                    return false;
                }
            }
        });


        //Implementations
        function init() {
            vm.FormMessage = "";

            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            combos();

            if (id > 0) {
                vm.State = "Editar Convênio";
                var blocker = blockUI.instances.get('blockModalCconvenio');
                blocker.start();
                cadastroservice
                    .getConvenioById(id)
                    .then(function (result) {
                        vm.convenio = result.data;

                        var tipo = _.find(vm.tipos, { Key: result.data.Tipo });
                        vm.tipoSelecionado = tipo.Key;

                        if (result.data.Situacao == "Ativo")
                            vm.SituacaoA = "Ativo";
                        else
                            vm.SituacaoI = "Inativo";

                        vm.estadoSelecionado = vm.convenio.EstadoSelecionado
                        buscarCidadesByEstado(vm.estadoSelecionado, vm.convenio.CidadeSelecionada);

                        if (vm.convenio.Sexo != undefined) {
                            vm.sexoSelecionado = vm.convenio.Sexo;
                        }
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.Message;
                    })['finally'](function () {
                        blocker.stop();
                    });
            }
            else {
                vm.SituacaoA = "Ativo";
                vm.convenio.DataNascimento = "";
                vm.tipoSelecionado = "PJ";
            }
        }

        function buscarCidadesByEstado(idEstado, cidadeSelecionada) {
            comumservice
                .getCidadesByEstado(idEstado)
                .then(function (result) {
                    vm.cidades = result.data;
                    if (cidadeSelecionada != "") {
                        var cidade = _.find(vm.cidades, { Id: cidadeSelecionada });
                        if (cidade != undefined)
                            vm.cidadeSelecionada = cidade.Id;
                    }
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                });
        }

        function alterartipo() {
            if (vm.tipoSelecionado == "PF") {
                vm.convenio.CPF = "";
                vm.convenio.Nome = "";
                vm.convenio.DataNascimento = "";
                vm.convenio.Sexo = "";
            } else {
                if (vm.tipoSelecionado == "PJ") {
                    vm.convenio.CNPJ = "";
                    vm.convenio.Nome = "";
                    vm.convenio.RazaoSocial = "";
                    vm.convenio.InscricaoEstadual = "";
                    vm.convenio.NomeFantasia = "";
                }
            }
        }

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockModalCconvenio');
            blocker.start();
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
            blocker.stop();
        });

        function combos() {
            vm.sexos = [{ Key: "M", Value: "Masculino" }, { Key: "F", Value: "Feminino" }];
            vm.tipos = [{ Key: "PJ", Value: "Pessoa Juridica" }, { Key: "PF", Value: "Pessoa Fisica" }];
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        function validaAbas(a, b, c) {
            var aba1 = angular.element(document.querySelector('#tabdados'));
            a == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#lidados'));
            a == "A" ? li1.addClass("active") : li1.removeClass("active");

            var aba2 = angular.element(document.querySelector('#tabendereco'));
            b == "A" ? aba2.addClass("active") : aba2.removeClass("active");
            var li2 = angular.element(document.querySelector('#liEndereco'));
            b == "A" ? li2.addClass("active") : li2.removeClass("active");

            var aba3 = angular.element(document.querySelector('#tabcontato'));
            c == "A" ? aba3.addClass("active") : aba3.removeClass("active");
            var li3 = angular.element(document.querySelector('#liContato'));
            c == "A" ? li3.addClass("active") : li3.removeClass("active");
        }

        function save() {

            $scope.showErrorsCheckValidity = true;

            var formEndereco = $scope.forms.dadosendereco.$valid;
            var formdados = $scope.forms.dadosconvenioes.$valid;
            var formcontato = $scope.forms.dadoscontato.$valid;

            if (!formdados) {
                validaAbas("A", "I", "C");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formEndereco) {
                validaAbas("I", "A", "C");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formcontato) {
                validaAbas("C", "I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
            else {

                if (vm.sexoSelecionado != undefined)
                    vm.convenio.Sexo = vm.sexoSelecionado;

                var est = _.find(vm.estados, { Id: vm.estadoSelecionado });
                if (est != undefined)
                    vm.convenio.Estado = est.Uf;

                var cid = _.find(vm.cidades, { Id: vm.cidadeSelecionada });
                if (cid != undefined)
                    vm.convenio.Cidade = cid.Nome;

                if (vm.tipoSelecionado != undefined)
                    vm.convenio.Tipo = vm.tipoSelecionado;

                vm.convenio.CidadeSelecionada = vm.cidadeSelecionada;
                vm.convenio.EstadoSelecionado = vm.estadoSelecionado;

                if (vm.SituacaoA == 'Ativo')
                    vm.convenio.Situacao = "Ativo";
                else
                    vm.convenio.Situacao = "Inativo";

                var blocker = blockUI.instances.get('blockModalCconvenio');
                blocker.start();

                cadastroservice
                    .saveConvenio(vm.convenio)
                    .then(function (result) {
                        vm.convenio = result.data;
                        if (id == 0) {
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                            id = result.data.IdConvenio;
                            vm.convenio.IdConvenio = result.data.IdConvenio;
                            vm.uploader.url = servicebase.urlApi() + '/cadastros/uploadthumbNail?idconveio=' + result.data.IdConvenio;
                            var aba4 = angular.element(document.querySelector('#tablogo'));
                            aba4.addClass("active");
                            var li4 = angular.element(document.querySelector('#lilogo'));
                            li4.addClass("active");
                            vm.novo = true;

                            var aba1 = angular.element(document.querySelector('#tabdados'));
                            aba1.removeClass("active");
                            var li1 = angular.element(document.querySelector('#lidados'));
                            li1.removeClass("active");

                            var aba2 = angular.element(document.querySelector('#tabendereco'));
                            aba2.removeClass("active");
                            var li2 = angular.element(document.querySelector('#liEndereco'));
                            li2.removeClass("active");

                            var aba3 = angular.element(document.querySelector('#tabcontato'));
                            aba3.removeClass("active");
                            var li3 = angular.element(document.querySelector('#liContato'));
                            li3.removeClass("active");

                        }
                        else {
                            notification.showSuccessBar("Alteração realizada com sucesso");
                            $modalInstance.close();
                        }
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.data.Message;


                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

    }
})();