(function () {
    'use strict';
    var controllerId = 'signOut';
    angular
        .module('app')
        .controller(controllerId, signOut);

    signOut.$inject = ['$location', 'common', 'authService'];

    function signOut($location, common, authService) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'signOut';
        


        vm.logOut = function () {
            authService.logOut();
            $location.path('/');
        }

        //vm.authentication = authService.authentication;

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { log('Activated Singout View'); });
            vm.logOut();
        }
    }
})();
