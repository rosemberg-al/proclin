/* ============================================================
 * Directive: status-imc
 * AngularJS directive para o status (label) do calculo de IMC
 * ============================================================ */
angular.module('app')
     .directive('statusImc', [function () {
         return {
             restrict: 'AE',
             template: '<span class="label label-rounded"></span>',
             replace: true,
             scope: { imc: '@', sexo: '@' },
             link: function ($scope, element, attrs) {

                 $scope.$watch('statusImc', function () {

                     if ($scope.sexo == 'Feminino') {
                         if ($scope.imc < 19.1) {
                             element.text($scope.imc);
                             element.addClass('label-default');
                         }
                         else if ($scope.imc >= 19.1 && $scope.imc <= 25.8) {
                             element.text($scope.imc);
                             element.addClass('label-success');
                         }
                         else if ($scope.imc >= 25.8 && $scope.imc < 27.3) {
                             element.text($scope.imc);
                             element.addClass('label-system');
                         }
                         else if ($scope.imc >= 27.3 && $scope.imc < 32.3) {
                             element.text($scope.imc);
                             element.addClass('label-warning');
                         }
                         else if ($scope.imc >= 32.3) {
                             element.text($scope.imc);
                             element.addClass('label-danger');
                         }
                         else {
                             if (angular.isDefined($scope.imc))
                                 console.warn('Nenhum estilo CSS definido para status "' + $scope.imc + '" na diretiva status-imc. Usando os valores padrões.');
                         }
                     }
                     else {
                         if ($scope.imc < 20.7) {
                             element.text($scope.imc);
                             element.addClass('label-default');
                         }
                         else if ($scope.imc >= 20.7 && $scope.imc <= 26.4) {
                             element.text($scope.imc);
                             element.addClass('label-success');
                         }
                         else if ($scope.imc >= 26.4 && $scope.imc < 27.8) {
                             element.text($scope.imc);
                             element.addClass('label-system');
                         }
                         else if ($scope.imc >= 27.8 && $scope.imc < 31.1) {
                             element.text($scope.imc);
                             element.addClass('label-warning');
                         }
                         else if ($scope.imc >= 31.1) {
                             element.text($scope.imc);
                             element.addClass('label-danger');
                         }
                         else {
                             if (angular.isDefined($scope.imc))
                                 console.warn('Nenhum estilo CSS definido para status "' + $scope.imc + '" na diretiva status-imc. Usando os valores padrões.');
                         }
                     }
                 });
             }
         };
     }]);