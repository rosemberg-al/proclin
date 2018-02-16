(function () {
    'use strict';

    angular.module('app.core')
           .factory('download', download);

    download.$inject = ['$http','notification', '$window', '$document'];

    function download($http,notification, $window, $document) {

        var service = {
            requestSave: requestSave,
            requestLoad: requestLoad,
            request: request
        };

        return service;

        //Salvar o arquivo 
        function requestSave(options) {

            var url = options.url;
            var dados = options.dados;
            var nomeDoArquivo = options.nomeDoArquivo;
            var contentType = options.contentType;
            var accept = options.accept;

            return $http.get(url, {
                //responseType: 'arraybuffer',
                params: dados,
                headers: {
                    'Content-type': contentType,
                    'Accept': accept
                }
            }).then(function (arquivo) {
                console.log(arquivo);
                var blob = new Blob([arquivo.data], {
                    type: accept
                });
                console.log(blob);
                saveAs(blob, nomeDoArquivo);
                return arquivo;
            }).catch(function (error) {
               // exception.throwEx(error);
       
                throw error;
            });
        }

        //Abrir o arquivo em outra janela
        function requestLoad(options) {
        
            return $http.get(options.url, {
                cache: false,
                params: options.params,
                responseType: 'arraybuffer'
            }).success(function (data, status, headers) {
                if (status == 200) {
                    headers = headers();
                    var filename = headers['x-filename'];
                    var contentType = headers['content-type'];
                    var blob = new Blob([data], { type: contentType });
                    var urlFile = $window.URL.createObjectURL(blob);
                    $window.open(urlFile, options.name, options.window);
                }
            }).catch(function (error) {
                throw error;
            });
        }

        //Efetua o dowload do arquivo no browser
        function request(options) {
            return $http.get(options.url, {
                cache: false,
                params: options.params,
                responseType: 'arraybuffer'
            }).success(function (data, status, headers) {
                if (status == 200) {
                    headers = headers();
                    var filename = headers['x-filename'];
                    var contentType = headers['content-type'];
                    var blob = new Blob([data], { type: contentType });
                    var urlFile = $window.URL.createObjectURL(blob);

                    var a = document.createElement('a');
                    a.href = urlFile;
                    a.download = options.filename;
                    a.target = '_blank';
                    a.click();
                    $window.URL.revokeObjectURL(urlFile);
                }
            });

        }
    }
})();