(function () {
    'use strict';

    var appPaciente = angular.module('app.paciente', ['webcam', 'ImageCropper']);

    appPaciente.config(["$stateProvider", function ($stateProvider) {

        $stateProvider
            .state("paciente", {
                parent: 'app',
                url: "/paciente",
                templateUrl: "app/paciente/lista.paciente.html",
                controller: 'Listarpaciente as vm'
            })
            .state("prontuario", {
                parent: 'app',
                url: "/paciente/prontuario/:id",
                templateUrl: "app/prontuarionovo/profile.html",
                controller: 'Profile as vm'
            })
            .state("ultimosatendimentos", {
                parent: 'app',
                url: "/paciente/ultimosatendimentos/:id",
                templateUrl: "app/prontuarionovo/ultimosatendimentos.html",
                controller: 'UltimosAtendimentos as vm'
            })
            .state("anamnese", {
                parent: 'app',
                url: "/paciente/anamnese/:id",
                templateUrl: "app/prontuarionovo/anamnese.html",
                controller: 'Anamnese as vm'
            })
            .state("historiapregressa", {
                parent: 'app',
                url: "/paciente/historiapregressa/:id",
                templateUrl: "app/prontuarionovo/historiapregressa.html",
                controller: 'HistoriaPregressa as vm'
            })
            .state("mantropometricas", {
                parent: 'app',
                url: "/paciente/mantropometricas/:id",
                templateUrl: "app/prontuarionovo/mantropometricas.html",
                controller: 'MAntropometricas as vm'
            }) 
            .state("atestado", {
                parent: 'app',
                url: "/paciente/atestado/:id",
                templateUrl: "app/prontuarionovo/atestado.html",
                controller: 'Atestado as vm'
            })
            .state("prescricoes", {
                parent: 'app',
                url: "/paciente/prescricoes/:id",
                templateUrl: "app/prontuarionovo/prescricao.html",
                controller: 'PrescricaoController as vm'
            })
            .state("receituario", {
                parent: 'app',
                url: "/paciente/receituario/:id",
                templateUrl: "app/prontuarionovo/receituario.html",
                controller: 'Receituario as vm'
            })
            .state("odontograma", {
                parent: 'app',
                url: "/paciente/odontograma/:id",
                templateUrl: "app/prontuarionovo/odontograma.html",
                controller: 'Odontograma as vm'
            })
            .state("prontuariopaciente", {
                parent: 'app',
                url: "/prontuario",
                templateUrl: "app/paciente/lista.pacienteprontuario.html",
                controller: 'ListaPacienteProntuario as vm'
            });

    }]);

})();