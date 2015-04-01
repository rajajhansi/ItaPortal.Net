(function () {
    'use strict';

    angular
        .module('app')
        .factory('authInterceptorService', authInterceptorService);

    authInterceptorService.$inject = ['$q', '$location', 'localStorageService'];

    function authInterceptorService($1, $location, localStorageService) {
        
        var _request = function (config) {
            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status == 401) {
                $location.path('/login');
            }
            return $1.reject(rejection);
        }

        var authInterceptorServiceFactory = {
            request: _request,
            responseError: _responseError
        };

        return authInterceptorServiceFactory;
    }
})();