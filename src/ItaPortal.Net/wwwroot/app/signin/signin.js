(function () {
    'use strict';
    var controllerId = 'signin';
    angular
        .module('app')
        .controller('signin', signin);

    signin.$inject = ['$location', 'common', '$timeout', 'authService'];

    function signin($location, common, $timeout, authService) {
        /* jshint validthis:true */
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        vm.title = 'Sign In';
        vm.active = "";
        vm.credential = {
            userName: "",
            password: ""
        };

        vm.message = "";


        vm.login = function () {
            vm.active = "active";

            authService.login(vm.credential).then(function (response) {
                var timer = $timeout(function () {                   
                    $timeout.cancel(timer);
                    vm.active = "";
                    $('#signInModal').modal('hide');
                    $location.path('/admin');
                }, 2000);
            },
            function (err) {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    vm.message = err.error_description;
                    common.logger.logError(vm.message, null, controllerId, true);
                    vm.active = "";
                }, 2000);               
            });            
        };

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { log('Activated SignIn View'); });
        }
    }
})();
