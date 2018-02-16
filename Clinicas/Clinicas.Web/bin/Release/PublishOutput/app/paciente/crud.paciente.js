(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('pacienteCrud', pacienteCrud);

    pacienteCrud.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'pacienteservice','cadastroservice', 'comumservice', 'id'];

    function pacienteCrud($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, pacienteservice,cadastroservice, comumservice, id) {

        var vm = this;
        vm.State = "Incluir Paciente";
        vm.FormMessage = "";
        vm.step = 1; //passo inicial, não mostra a imagem
        vm.paciente = {
            Foto: []
        };
        vm.basefoto = undefined;

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.save = save;
        vm.cancel = cancel;
        vm.alterartipo = alterartipo; // Alterar tipo pessoa 
        vm.addCarteira = addCarteira;  
        vm.excluircarteira = excluircarteira;  
        vm.resetCarteira = resetCarteira;
        vm.verificaCarteiraRepetida = verificaCarteiraRepetida;

        vm.conveniopaciente = {
            NumeroCarteira: "",
            ValidadeCarteira: "",
            Plano: ""
        };
        vm.carteirasAdicionadas = [];

        //Feature Start
        init();

        //********************************************************      webcan      ***************************************

        var _video = null,
            patData = null;

        vm.patOpts = { x: 0, y: 0, w: 25, h: 25 };

        // Setup a channel to receive a video property
        // with a reference to the video element
        // See the HTML binding in main.html
        vm.channel = {};

        vm.webcamError = false;
        vm.onError = function (err) {
            $scope.$apply(
                function () {
                    vm.webcamError = err;
                }
            );
        };

        vm.onSuccess = function () {
            // The video element contains the captured camera data
            _video = vm.channel.video;
            $scope.$apply(function () {
                vm.patOpts.w = _video.width;
                vm.patOpts.h = _video.height;
                //$scope.showDemos = true;
            });
        };

        vm.onStream = function (stream) {
            // You could do something manually with the stream.
        };

        function convertURIToImageData(URI) {
            return new Promise(function (resolve, reject) {
                if (URI == null) return reject();
                var canvas = document.createElement('canvas'),
                    context = canvas.getContext('2d'),
                    image = new Image();
                image.addEventListener('load', function () {
                    canvas.width = image.width;
                    canvas.height = image.height;
                    context.drawImage(image, 0, 0, canvas.width, canvas.height);
                    resolve(context.getImageData(0, 0, canvas.width, canvas.height));
                }, false);
                image.src = URI;
            });
        }

        vm.makeSnapshot = function () {
            if (_video) {
                var patCanvas = document.querySelector('#snapshot');

                if (!patCanvas) return;

                patCanvas.width = _video.width;
                patCanvas.height = _video.height;
                var ctxPat = patCanvas.getContext('2d');

                var idata = getVideoData(vm.patOpts.x, vm.patOpts.y, vm.patOpts.w, vm.patOpts.h);
                ctxPat.putImageData(idata, 0, 0);

                sendSnapshotToServer(patCanvas.toDataURL());
                patData = idata;
                //vm.Foto = patCanvas.toDataURL();
                vm.step = 2;
                dataURLtoFile(patCanvas.toDataURL(), 'foto_paciente.png');
            }
        };

        $scope.$watch('vm.initCrop', function () {
            if (vm.initCrop) {
                vm.step = 3;
            }
        });

        function bufferToBase64(buf) {
            var binstr = Array.prototype.map.call(buf, function (ch) {
                return String.fromCharCode(ch);
            }).join('');
            return btoa(binstr);
        }

        /**
         * Redirect the browser to the URL given.
         * Used to download the image by passing a dataURL string
         */
        vm.downloadSnapshot = function downloadSnapshot(dataURL) {
            window.location.href = dataURL;
        };

        var getVideoData = function getVideoData(x, y, w, h) {
            var hiddenCanvas = document.createElement('canvas');
            hiddenCanvas.width = _video.width;
            hiddenCanvas.height = _video.height;
            var ctx = hiddenCanvas.getContext('2d');
            ctx.drawImage(_video, 0, 0, _video.width, _video.height);
            return ctx.getImageData(x, y, w, h);
        };

        /**
         * This function could be used to send the image data
         * to a backend server that expects base64 encoded images.
         *
         * In this example, we simply store it in the scope for display.
         */
        var sendSnapshotToServer = function sendSnapshotToServer(imgBase64) {
            vm.snapshotData = imgBase64;
        };

        function dataURLtoFile(dataurl, filename) {
            var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
                bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
            while (n--) {
                u8arr[n] = bstr.charCodeAt(n);
            }
            var files = new File([u8arr], filename, { type: mime });

            var fileReader = new FileReader();
            fileReader.readAsDataURL(files);

            fileReader.onload = function (e) {
                vm.imgSrc = this.result;
                $scope.$apply();
            };
        }

        vm.clear = function () {
            vm.step = 1;
            delete vm.imgSrc;
            delete $scope.result;
            delete $scope.resultBlob;
            delete vm.basefoto;
        };


        //****************************************************************************************************************

        function draw(dataURL) {
            var ctx = document.querySelector('#snapshot').getContext('2d');
            var img1 = new Image();
            img1.onload = function () {
                ctx.drawImage(img1, 40, 10);
            }
            img1.src = 'data:image/png;base64,' + dataURL;
        }
  
        
        //Implementations
        function init() {
            vm.FormMessage = "";

            var pEstados = comumservice.getEstados();
            pEstados.then(function (result) {
                vm.estados = result.data;
            });

            var pConvenios = cadastroservice.listarConvenios();
            pConvenios.then(function (result) {
                vm.convenios = result.data;
            });

            combos();

            if (id > 0) {

                vm.State = "Editar Paciente";
                var blocker = blockUI.instances.get('blockModalCpaciente');
                blocker.start();
                pacienteservice
                            .getPacienteById(id)
                            .then(function (result) {
                                vm.paciente = result.data;

                                var tipo = _.find(vm.tipos, { Key: result.data.Tipo });
                                vm.tipoSelecionado = tipo.Key;

                                if (result.data.Situacao == "Ativo")
                                    vm.SituacaoA = "Ativo";
                                else
                                    vm.SituacaoI = "Inativo";

                                vm.estadoSelecionado = vm.paciente.EstadoSelecionado
                                buscarCidadesByEstado(vm.estadoSelecionado, vm.paciente.CidadeSelecionada);

                                if (vm.paciente.Sexo != undefined) {
                                    vm.sexoSelecionado = vm.paciente.Sexo;
                                }
                                if (vm.paciente.Foto != null) {
                                    //draw(vm.paciente.Foto);
                                    vm.basefoto = 'data:image/png;base64,' + vm.paciente.Foto;
                                    vm.step = 3;
                                }

                                vm.carteirasAdicionadas = vm.paciente.Carteiras;
                            })
                            .catch(function (ex) {
                                vm.FormMessage = ex.Message;
                            })['finally'](function () {
                                blocker.stop();
                            });
            }
            else {
                vm.SituacaoA = "Ativo";
                vm.paciente.DataNascimento = "";
                vm.tipoSelecionado = "PF";
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
                vm.paciente.CPF = "";
                vm.paciente.Nome = "";
                vm.paciente.DataNascimento = "";
                vm.paciente.Sexo = "";
            } else {
                if (vm.tipoSelecionado == "PJ") {
                    vm.paciente.CNPJ = "";
                    vm.paciente.Nome = "";
                    vm.paciente.RazaoSocial = "";
                    vm.paciente.InscricaoEstadual = "";
                    vm.paciente.NomeFantasia = "";
                }
            }
        }

        $scope.$watch('vm.estadoSelecionado', function (newValue, oldValue) {
            var blocker = blockUI.instances.get('blockModalCpaciente');
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

        function validaAbas(a, b, c,d) {




            var aba4= angular.element(document.querySelector('#tabFoto'));
            aba4.removeClass("active");
            var li4 = angular.element(document.querySelector('#liFoto'));
            li4.removeClass("active");


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

            var aba4 = angular.element(document.querySelector('#tabconvenio'));
            d == "A" ? aba4.addClass("active") : aba4.removeClass("active");
            var li4 = angular.element(document.querySelector('#liConvenio'));
            d == "A" ? li4.addClass("active") : li4.removeClass("active");

        
        }

        function save() {

            $scope.showErrorsCheckValidity = true;


            if(vm.basefoto!=undefined){
                var base64 = vm.basefoto;//document.getElementById("snapshot").toDataURL("image/png");
                //Remover os caracteres que são adionados por padão antes do Base64 da imagem (data:image/png;base64,).
                //Pois só depois disso vem o Base64 que precisamos salvar.
                var dados = base64.substr(base64.indexOf(',') + 1, base64.length);
                vm.paciente.Foto = dados;
            }

            var formEndereco = $scope.forms.dadosendereco.$valid;
            var formdados = $scope.forms.dadospacientees.$valid;
            var formcontato = $scope.forms.dadoscontato.$valid;

            if (!formdados) {
                validaAbas("A", "I", "C","D");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formEndereco) {
                validaAbas("I", "A", "C","D");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            } else if (!formcontato) {
                validaAbas("C", "I", "A","D");
                vm.FormMessage = "";
                vm.FormMessage = "Existem campos obrigatórios sem o devido preenchimento";
            }
            else {

                if (vm.sexoSelecionado != undefined)
                    vm.paciente.Sexo = vm.sexoSelecionado;

                var est = _.find(vm.estados, { Id: vm.estadoSelecionado });
                if (est != undefined)
                    vm.paciente.Estado = est.Uf;

                var cid = _.find(vm.cidades, { Id: vm.cidadeSelecionada });
                if (cid != undefined)
                    vm.paciente.Cidade = cid.Nome;

                if (vm.tipoSelecionado != undefined)
                    vm.paciente.Tipo = vm.tipoSelecionado;

                vm.paciente.CidadeSelecionada = vm.cidadeSelecionada;
                vm.paciente.EstadoSelecionado = vm.estadoSelecionado;
                

                if (vm.SituacaoA == 'Ativo')
                    vm.paciente.Situacao = "Ativo";
                else
                    vm.paciente.Situacao = "Inativo";

                if (vm.carteirasAdicionadas.length > 0)
                    vm.paciente.Carteiras = vm.carteirasAdicionadas;

                var blocker = blockUI.instances.get('blockModalCpaciente');
                blocker.start();

                pacienteservice
                    .savePaciente(vm.paciente)
                    .then(function (result) {
                        vm.paciente = result.data;
                        if (id == 0)
                            notification.showSuccessBar("Cadastro realizado com sucesso");
                        else
                            notification.showSuccessBar("Alteração realizada com sucesso");

                        $modalInstance.close();
                    })
                    .catch(function (ex) {
                        vm.FormMessage = ex.data.Message;


                    })['finally'](function () {
                        blocker.stop();
                    });
            }
        }

        function verificaCarteiraRepetida(numero){
            for (var i = 0; i < vm.carteirasAdicionadas.length; i++) {
                if (vm.carteirasAdicionadas[i].NumeroCarteira == numero) {
                    return i;
                }
            }
            return -1;
        }

        // carteira
        function addCarteira() {
            vm.FormCarteiras = [];
            var erro = false;
            if (vm.convenioSelecionado == undefined)
                erro = true;
                // notification.showError("Campo Convênio Obrigatório");
                // vm.FormCarteiras.push('Selecione o convênio!');

            if (vm.conveniopaciente.NumeroCarteira == "")
                erro = true;
                //notification.showError('Campo Número da carteira Obrigatório ');

            if (vm.conveniopaciente.ValidadeCarteira == "")
                erro = true;
                // notification.showError('Campo Validade Carteira Obrigatório ');

            if (vm.conveniopaciente.Plano == "")
                erro = true;
                // notification.showError('Campo Convênio Obrigatório');

            if (erro) {
                notification.showError('Existem campos obrigatórios sem o devido preenchimento');
            }

            if (vm.verificaCarteiraRepetida(vm.conveniopaciente.NumeroCarteira) == 0) {
                notification.showError('Carteira em Duplicidade');
                return;
            }

            if (vm.FormCarteiras.length == 0) {
                var convenio = _.find(vm.convenios, { IdConvenio: vm.convenioSelecionado });
                var cart = { ValidadeCarteira: vm.conveniopaciente.ValidadeCarteira, NumeroCarteira: vm.conveniopaciente.NumeroCarteira, Plano: vm.conveniopaciente.Plano, IdConvenio: vm.convenioSelecionado, Convenio: convenio.Nome };
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

        //


    }
})();