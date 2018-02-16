angular.module("app")
    .directive("promedMask", PromedMask);

function PromedMask() {

    function somenteNumeros(valor) {
        if (!valor) {
            return valor;
        }

        return valor.replace(/\D/g, '');
    }

    function aplicarMascara(valor) {
        if (!valor) {
            return valor;
        }

        var valorFormatado;

        if (valor.length > 2) {
            var ponto = valor.length > 9 ? 7 : valor.length - 2;            
            valorFormatado = valor.substr(0, ponto) + '.' + valor.substr(ponto, 2);
        }
        else {
            valorFormatado = valor;
        }

        return valorFormatado;
    }

    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {

            ngModel.$parsers.push(function (valor) {
                if (!valor) {
                    return valor;
                }

                var varlorSemMascara = somenteNumeros(valor);
                var valorFormatado = aplicarMascara(varlorSemMascara);

                if (ngModel.$viewValue != valorFormatado) {
                    ngModel.$setViewValue(valorFormatado);
                    ngModel.$render();
                }

                return valorFormatado;
            });
        }
    };
}