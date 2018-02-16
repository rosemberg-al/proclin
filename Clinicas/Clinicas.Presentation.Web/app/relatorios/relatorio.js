(function () {
    'use strict';

    angular
        .module('app.relatorio')
        .controller('Relatorio', Relatorio);

    Relatorio.$inject = ['$scope', '$http', 'blockUI', 'common', 'notification', 'exception', 'ds.cadastros'];

    function Relatorio($scope, $http, blockUI, common, notification, exception, dsCadastros) {

        var vm = this;
        common.setBreadcrumb('pagina-inicial .relatório .paciente');

        vm.url = "http://localhost:51332/relatorio/";

        vm.State = "Incluir Procedimento";
        vm.FormMessage = "";
        vm.procedimento = {};

        $scope.forms = {};
        vm.formValid = true;

        //Funções
        vm.init = init;

        vm.rel_paciente = rel_paciente;


        vm.OpenWindowWithPost = OpenWindowWithPost;
        vm.OpenWindowWithGet = OpenWindowWithGet;


        //Feature Start
        init();

        function init() {

        }

        function OpenWindowWithPost(url, windowoption, name, params) {
            var form = document.createElement("form");
            form.setAttribute("method", "post");
            form.setAttribute("action", url);
            form.setAttribute("target", name);
            for (var i in params) {
                if (params.hasOwnProperty(i)) {
                    var input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = i;
                    input.value = params[i];
                    form.appendChild(input);
                }
            }
            document.body.appendChild(form);
            //note I am using a post.htm page since I did not want to make double request to the page
            //it might have some Page_Load call which might screw things up.
            window.open("post.htm", name, windowoption);
            form.submit();
            document.body.removeChild(form);
        }

        function OpenWindowWithGet(url, windowoption, name, params) {
            var form = document.createElement("form");
            form.setAttribute("method", "get");
            form.setAttribute("action", url);
            form.setAttribute("target", name);
            for (var i in params) {
                if (params.hasOwnProperty(i)) {
                    var input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = i;
                    input.value = params[i];
                    form.appendChild(input);
                }
            }
            document.body.appendChild(form);
            //note I am using a post.htm page since I did not want to make double request to the page
            //it might have some Page_Load call which might screw things up.
            window.open("post.htm", name, windowoption);
            form.submit();
            document.body.removeChild(form);
        }

        function rel_paciente() {


            var obj = {};

            vm.OpenWindowWithGet(vm.url + 'rel_paciente', "width=1000, height=600, left=100, top=100, resizable=yes, scrollbars=yes", "NewFile", obj);
        }


    }
})();