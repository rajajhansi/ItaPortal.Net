(function () {
    'use strict';
    var controllerId = 'signup';
    angular
        .module('app')
        .controller('signup', signup);

    signup.$inject = ['$location', 'common', '$timeout', 'authService', 'lookupService'];

    function signup($location, common, $timeout, authService, lookupService) {
        /* jshint validthis:true */
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        vm.title = 'Sign Up';
        vm.active = "";
        vm.savedSuccessfully = false;
        vm.message = "";       
        vm.securityQuestions = {}; //[ {'id': 1, 'name': 'Question 1'} ];
        

        vm.registration = {
            firstName: "",
            lastName: "",
            email: "",
            userName: "",
            password: "",
            confirmPassword: "",
            securityQuestion: "",
            securityAnswer: ""
        };

        vm.signUp = function (form) {
            vm.registration.userName = vm.registration.email;
            vm.active = "active";
            authService.saveRegistration(vm.registration).then(function (repsonse) {
                vm.savedSuccessfully = true;
                vm.message = "User has been registered successfully, you will be redirected to login page in 2 seconds";
                startTimer();
            },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                vm.message = "Failed to register due to:" + errors.join(' ');
            });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                vm.active = "";
                $('#signUpModal').modal('hide');                
                $('#signInModal').modal('show');
            }, 2000);
        };

        activate();

        function activate() {                     
            lookupService.getSecurityQuestions().then(function (securityQuestions) {
                vm.securityQuestions = securityQuestions;                
            }, function (err) {
                alert(err);
            });

            common.activateController([], controllerId)
                .then(function () { log('Activated SignUp View'); });
        }
    }
})();
