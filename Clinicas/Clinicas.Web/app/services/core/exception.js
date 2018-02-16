(function () {
    'use strict';

    angular.module('app.core')
           .factory('exception', exception);

    exception.$inject = ['appConfig', 'logger', 'notification'];

    function exception(appConfig, logger, notification) {

        var service = {
            throwEx: throwEx
        };

        return service;
        
        function throwEx(ex) {
            if (appConfig.logExceptions) {
                logger.logEx(ex.data, ex.statusText);
            }

            if (ex.status === 500) {
                if (angular.isDefined(ex.error_description))
                    console.log(ex.error_description);
                if (angular.isDefined(ex.data))
                    console.log(ex.data);
                else if (angular.isDefined(ex.statusText))
                    console.log(ex.statusText);
                notification.showError("Ocorreu um erro ao processar sua requisição. Por favor, tente novamente mais tarde");                
            }
            else if (ex.status == 400) {
                if (angular.isDefined(ex.statusText))
                    notification.showError(ex.statusText);
                else if (angular.isDefined(ex.error_description))
                    notification.showError(ex.error_description);
            }
            else if (ex.status === 401) {
                notification.showError("Sua sessão foi finalizada.<br/> Por favor, entre novamente com suas credenciais");
            }
            else {
                notification.showError(ex.data);
            }
        }
    }
})();
