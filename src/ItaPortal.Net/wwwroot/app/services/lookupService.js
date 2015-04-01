(function () {
    'use strict';

    angular
        .module('app')
        .factory('lookupService', lookupService);

    lookupService.$inject = ['$http', '$q', 'config'];

    function lookupService($http, $q, config) {
        
        var getSecurityQuestions = function () {
            var deferred = $q.defer();
            $http.get(config.serviceBaseUrl + 'lookup/securityquestions').then(function (response) {
                 deferred.resolve(response.data);
            });

            return deferred.promise;
        };

        var lookupServiceFactory = {
            getSecurityQuestions: getSecurityQuestions
        };
        return lookupServiceFactory;
    }
})();