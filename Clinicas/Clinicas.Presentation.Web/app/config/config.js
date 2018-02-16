/**
*
*  Configurações da App
*  ==============================================================
*  1. RoutePrefix/ApiPrefix
*  2. Logging
*  3. BuscaCEP
*  
**/

(function () {
    'use strict';

    /*
    * App Global Configuration constant
    */

    angular
        .module('app.config')
        .constant('appConfig',
        {
            idModulo: 'Portal',                                    // Módulo
            authUrl: 'http://apiproclinseg.azurewebsites.net',     // Prefixo as urls de chamada da api de autenticação
            routePrefix: '',                                    // Prefixo para rotas do angular
            apiPrefix: 'http://localhost:5100',                // Prefixo as urls de chamada na API (sem '/' no final)
            loggerUrl: undefined,                               // Url logs API
            logExceptions: false
        });

})();
