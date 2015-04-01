(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());
    
    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/welcome/welcome.html',
                    title: 'welcome',
                    controller: 'welcome',
                    controllerAs: 'vm',
                    //settings: {
                    //    nav: 1,
                    //    content: '<i class="fa fa-dashboard"></i> Dashboard'
                    //}
                }
            }, {
                url: '/admin',
                config: {
                    title: 'admin',
                    templateUrl: 'app/admin/admin.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }, {
                url: '/signout',
                config: {
                    templateUrl: 'app/signout/signout.html',
                    title: 'signout',
                    controller: 'signOut',
                    controllerAs: 'vm',
                }
            }
                /*,
            {
                url: '/signin',
                config: {
                    title: 'signin',
                    templateUrl: '/app/signin/signin.html',
                    controller: 'signin',
                    controllerAs: 'vm',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            },
            {
                url: '/signup',
                config: {
                    title: 'signup',
                    templateUrl: '/app/signup/signup.html',
                    controller: 'signup',
                    controllerAs: 'vm',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }*/
        ];
    }
})();