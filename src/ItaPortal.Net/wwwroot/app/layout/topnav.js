(function () {
    'use strict';
    var controllerId = 'topnav';
    angular
        .module('app')
        .controller('topnav', topnav);

    topnav.$inject = ['$scope','$location', 'common', 'authService']; 

    function topnav($scope, $location, common, authService) {

        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        /* jshint validthis:true */
        var vm = this;
        vm.title = 'topnav';
        vm.userName = 'Guest';
        vm.authService = authService;

        $scope.$watch('vm.authService.authentication', function () {
            updateUserName();
        }, true);

        function updateUserName()
        {
            vm.userName = (authService.authentication.isAuth) ? authService.authentication.userName : 'Guest';
        }
        activate();

        function activate() {
            updateUserName()
        }
    }
})();
