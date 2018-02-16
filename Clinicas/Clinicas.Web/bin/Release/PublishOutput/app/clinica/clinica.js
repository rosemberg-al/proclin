(function () {
    'use strict';

    angular
        .module('app.clinica')
        .controller('ClinicaController', ClinicaController);

    ClinicaController.$inject = ['$scope', '$http', '$q', 'FileUploader', 'blockUI', 'servicebase', 'common', 'notification', '$modal', 'cadastroservice', 'comumservice', '$stateParams'];

    function ClinicaController($scope, $http, $q, FileUploader, blockUI, servicebase, common, notification, $modal, cadastroservice, comumservice, $stateParams) {

        var vm = this;
        vm.clinica = {
            Unidades: [],
            Logo: undefined
        };
        vm.unidade = undefined;

        common.setBreadcrumb('Sistema .Clinica');
        $scope.forms = {};
        //Funções
        vm.init = init;
        vm.save = save;
        vm.add = add;
        vm.excluir = excluir;


        vm.uploader = new FileUploader({
            url: servicebase.urlApi() + '/cadastros/uploadlogoclinica?id=0',
            queueLimit: 1,
            method: "POST",
            //formData: { model: vm.item },
            //headers: { Authorization: dsSession.getAuthorization() },
            //withCredentials: true
        });


        vm.uploader.onCompleteItem = function (fileItem, response, status, headers) {
            if (status == 200) {
                notification.showSuccess("Logo adicionada com sucesso.");
                vm.clinica.Logo = response;
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
            },
            name: 'enforceMaxFileSize',
            fn: function (item) {
                if (item.size > 15728640) {
                    notification.showError('A imagem é muito grande para upload. O tamanho máximo é 15MB.');
                    return false;// item.size <= 350525; // 10 MiB to bytes
                }
                return true;
            }
        });

        //Feature Start
        init();

        //Implementations
        function init() {
            var blocker = blockUI.instances.get('blockCrudClinica');
            blocker.start();

            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            cadastroservice
                .getDadosClinica()
                .then(function (result) {
                    vm.clinica = result.data;
                    vm.estadoSelecionado = vm.clinica.IdEstado;
                    buscarCidadesByEstado(vm.estadoSelecionado, vm.clinica.IdCidade);
                    vm.uploader.url = servicebase.urlApi() + '/cadastros/uploadlogoclinica?id=' + result.data.IdClinica;
                })
                .catch(function (ex) {
                    notification.showError(ex.data.Message);
                })['finally'](function () {
                    blocker.stop();
                });
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

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockCrudClinica');
            blocker.start();
            if (angular.isDefined(newValue)) {
                buscarCidadesByEstado(newValue, "");
            }
            blocker.stop();
        });

        function excluir(item) {
            _.remove(vm.clinica.Unidades, item);
        }

        function add(item) {
            if (vm.unidade == undefined) {
                notification.showError('Você deve preencher o nome para adicionar.');
                $scope.forms.unidade.unidade.$invalid = true;
                return;
            }
            $scope.forms.unidade.unidade.$invalid = false;
            var uni = _.find(vm.clinica.Unidades, { Nome: vm.unidade });
            if (uni != null) {
                notification.showError("A unidade já foi adicionada!");
            }
            else {
                var unidade = { IdUnidadeAtendimento: 0, Nome: vm.unidade };
                vm.clinica.Unidades.push(unidade);
                vm.unidade = undefined;
            }
        }

        function validaAbas(a, b) {
            var aba1 = angular.element(document.querySelector('#tabcontato'));
            b == "A" ? aba1.addClass("active") : aba1.removeClass("active");
            var li1 = angular.element(document.querySelector('#liContato'));
            b == "A" ? li1.addClass("active") : li1.removeClass("active");

            /* var aba2 = angular.element(document.querySelector('#tabendereco'));
            a == "A" ? aba2.addClass("active") : aba2.removeClass("active");
            var li2 = angular.element(document.querySelector('#liEndereco'));
            a == "A" ? li2.addClass("active") : li2.removeClass("active");

            var aba3 = angular.element(document.querySelector('#tablogo'));
            aba3.removeClass("active");
            var li3 = angular.element(document.querySelector('#lilogo'));
            li3.removeClass("active");
            */
            var aba4 = angular.element(document.querySelector('#tabUnidades'));
            aba4.removeClass("active");
            var li4 = angular.element(document.querySelector('#liUnidades'));
            li4.removeClass("active");
        }

        function save() {
            $scope.showErrorsCheckValidity = true;

            /// var formEndereco = $scope.forms.dadosenderecoclinica.$valid;
            var formdados = $scope.forms.dadosclinica.$valid;

            if (!formdados) {
                validaAbas("I", "A");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
                return;
            }/* else if (!formEndereco) {
                validaAbas("A", "I");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
                return;
            } */
            else {
                vm.FormMessage = "";
                if (vm.cidadeSelecionada != null)
                    vm.clinica.IdCidade = vm.cidadeSelecionada;

                if (vm.estadoSelecionado != null)
                    vm.clinica.IdEstado = vm.estadoSelecionado;


                var blocker = blockUI.instances.get('blockCrudClinica');
                blocker.start();

                cadastroservice
                    .saveClinica(vm.clinica)
                    .then(function (result) {
                        vm.clinica = result.data;
                        notification.showSuccessBar("Alteração realizada com sucesso!");
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