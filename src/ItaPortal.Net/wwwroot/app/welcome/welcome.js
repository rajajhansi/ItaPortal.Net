(function () {
    'use strict';
    var controllerId = 'welcome';
    angular
        .module('app')
        .controller(controllerId, welcome);

    welcome.$inject = ['$location', 'common'];

    function welcome($location, common) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        /* jshint validthis:true */
        var vm = this;
        vm.title = 'welcome';
        

        vm.myInterval = 5000;
        var slides = vm.slides = [];

        vm.addSlide = function () {
            var slideWidth = 1000 + slides.length;
            slides.push({
                image: 'http://placekitten.com/' + slideWidth + '/300',
                text: ['More', 'Extra', 'Lots of', 'Surplus'][slides.length % 4] + ' ' +
                    ['Cats', 'Kittys', 'Felines', 'Cutes'][slides.length % 4]
            });
        };
        for (var i = 0; i < 4; i++) {
            vm.addSlide();
        }
        activate();

        function activate() {           
            common.activateController([], controllerId)
                .then(function () { log('Activated Welcome View'); });
            //$('#content').inputMachinator();
        }
    }
})();
