(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('AlterarFoto', AlterarFoto);

    AlterarFoto.$inject = ['$scope', '$http', '$q', '$modal', '$modalInstance', 'blockUI', 'common', 'notification', 'pacienteservice', 'cadastroservice', 'comumservice', 'id'];

    function AlterarFoto($scope, $http, $q, $modal, $modalInstance, blockUI, common, notification, pacienteservice, cadastroservice, comumservice, id) {

        var vm = this;
        vm.State = "Foto";
        vm.FormMessage = "";
        vm.paciente = {};
        
        vm.step = 1; //passo inicial, não mostra a imagem

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;
        vm.cancel = cancel;
        vm.alterarfoto = alterarfoto;


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

        //Implementations
        function init() {

        }

        function alterarfoto() {

            var base64 = vm.basefoto;
            //Remover os caracteres que são adionados por padão antes do Base64 da imagem (data:image/png;base64,).
            //Pois só depois disso vem o Base64 que precisamos salvar.
            var dados = base64.substr(base64.indexOf(',') + 1, base64.length);

            var model = {
                Foto: dados,
                IdPaciente: id
            };


            pacienteservice
                .alterarfoto(model)
                .then(function (result) {
                    notification.showSuccessBar("Foto alterada com sucesso!");
                    $modalInstance.close();
                })
                .catch(function (ex) {
                    if (ex.data != null)
                        vm.FormMessage = ex.data.Message;
                })['finally'](function () {
                    blocker.stop();
                });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

    }
})();