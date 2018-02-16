(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .service('servicebase', servicebase); //Define o nome a função do seu .service

    servicebase.$inject = ['$http']; //Lista de dependências

    function servicebase($http) {

        var vm = this;

        vm.urlApi = urlApi;
        vm.convertMoeda = convertMoeda;
        vm.convertMoedaView = convertMoedaView;


        //Implementação das funções
        function urlApi() {
           //  return "http://appproclin-api.azurewebsites.net";
            return "http://localhost:52149";
        }

        // converte moeda salvar banco de dados 
        function convertMoeda(res) {
            var res = String(res).replace(".", "");
            res = String(res).replace(",", ".");
            // alert(moeda)
            return parseFloat(res);
        }

        // convert moeda view 
        function convertMoedaView(num) {
            var x = 0;
            if (num < 0) {
                num = Math.abs(num);
                x = 1;
            }
            if (isNaN(num)) num = "0";
            var cents = Math.floor((num * 100 + 0.5) % 100);
            num = Math.floor((num * 100 + 0.5) / 100).toString();

            if (cents < 10) cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
                num = num.substring(0, num.length - (4 * i + 3)) + '.'
                      + num.substring(num.length - (4 * i + 3));
            var ret = num + ',' + cents;
            if (x == 1) ret = ' - ' + ret; return ret;
        }



    }
})();