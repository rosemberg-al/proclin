(function () {
    'use strict';

    angular
        .module('app.paciente')
        .controller('Profile', Profile);

    Profile.$inject = ['$scope', '$http', '$q', '$modal', 'DTOptionsBuilder', 'blockUI', 'common', 'notification', 'pacienteservice','$stateParams'];

    function Profile($scope, $http, $q, $modal, DTOptionsBuilder, blockUI, common, notification, pacienteservice,$stateParams) {

        var vm = this;

        $scope.forms = {};
        vm.pesq = {};
        vm.formValid = true;
        vm.IdPaciente = $stateParams.id;

        //Breadcrumb
        common.setBreadcrumb('Cadastro .Paciente');

        vm.tipoBusca = 'Nome';

        //Funções
        vm.init = init;
        vm.alterarfoto = alterarfoto;

        vm.ultimosAtendimentos = ultimosAtendimentos;


        
        //Feature Start
        init();

        //Implementations
        function init() {
        
            alert($stateParams.id);
        }

        function alterarfoto() {
            var modalInstance = $modal.open({
                templateUrl: 'app/paciente/alterar_foto.html',
                controller: 'AlterarFoto as vm',
                size: 'xs',
                backdrop: 'static',
                resolve: {
                    id: function () {
                        return 1;
                    },
                }
            });
            modalInstance.result.then(function () {
                init();
            });
        }

        function ultimosAtendimentos(){



        }


    }
})();