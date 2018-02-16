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

            if (ex.status === 500){
                notification.showError("Ocorreu um erro ao processar sua solicitação. Por favor, tente novamente mais tarde");                
            }
            else if (ex.status === 401) {
                notification.showError("Sua sessão foi finalizada.<br/> Por favor, entre novamente com suas credenciais");
            }
            else {
                if (ex.data != undefined) {
                    if (angular.isDefined(ex.data.Message))
                        notification.showError(ex.data.Message);
                    else {

                        if (typeof ex.data === 'string' || ex.data instanceof String)
                            notification.showError(ex.data);
                        else if (typeof ex.statusText === 'string' || ex.statusText instanceof String)
                            notification.showError(ex.statusText);
                    }
                }
                else if (ex.Message != undefined) {
                    notification.showError(ex.Message);
                }
                else if (ex.error_description != undefined) {
                    notification.showError(ex.error_description);
                }
            }
        }
    }
})();
