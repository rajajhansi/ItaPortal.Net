(function () {
    'use strict';

    angular
        .module('app')
        .controller('carousel', carousel);

    carousel.$inject = ['$location']; 

    function carousel($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'carousel';

        vm.myInterval = 1000;
        vm.slides = [
            {
                image: 'http://cdn.qiibo.com/wp-content/uploads/2013/03/galaxy-wallpaper-background-40801-hd-wallpapers-2.jpg'
            },
            {
                image: 'http://www.cyclesoles.com/wp-content/uploads/2013/10/hd-wallpapers-mountain-bike-desktop-biking-wallpaper-tricks-1920x1080-wallpaper-1000x400.jpg'
            },
            {
                image: 'http://www.cyclesoles.com/wp-content/uploads/2013/10/bicycle-hd-for-and-85908-1000x400.jpg'
            }
        ];
        activate();

        function activate() { }
    }
})();
