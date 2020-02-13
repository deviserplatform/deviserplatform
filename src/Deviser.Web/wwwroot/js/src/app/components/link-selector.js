(function () {
    var app = angular.module('modules.app.linkSelector', [
    'ui.router',
    'ui.bootstrap',
    'ui.sortable',
    'ui.select',
    'deviser.services',
    'ngFileUpload',
    'angular-img-cropper'
    ]);

    app.directive("devLinkSelector", ['$compile', '$templateCache', 'assetService', devLinkSelector]);

    function devLinkSelector($compile, $templateCache, assetService) {
        var returnObject = {
            restrict: "A",
            controller: ['$scope', '$uibModal', 'pageService', ctrl],
            controllerAs: 'locVM',
            bindToController: true,
            link: link,
            scope: {
                selectedLink: '=',
                properties: '&',
                label: '@'
            }
        };

        return returnObject;
        /////////////////////////////////////////////
        /*Function declarations only*/
        function link(scope, element, attrs) {
            template = $templateCache.get("app/components/linkSelector.tpl.html");
            element.html(template);
            $compile(element.contents())(scope);
        }

        function ctrl($scope, $uibModal, pageService) {
            var vm = this,
                formatPages = processPage;
            vm.pages = [];
            

            init();

            /////////////////////////////////////////////
            /*Function declarations only*/
            function init() {
                getPages();
                if (!vm.selectedLink){
                    vm.selectedLink = {};
                }

                if (!vm.selectedLink.linkType) {
                    vm.selectedLink.linkType = 'PAGE';
                }
            }

            //Event handlers

            function getPages() {
                pageService.get().then(function (pages) {
                    vm.nestedPages = pages;
                    vm.pages = [];
                    formatPages(vm.nestedPages);
                })
            }


            function processPage(page, levelPrefix) {
                if (!levelPrefix) {
                    levelPrefix = '';
                }
                else if (levelPrefix === '') {
                    levelPrefix = '--';
                }

                if (page.pageLevel != 0) {
                    var englishTranslation = _.find(page.pageTranslation, { locale: "en-US" });
                    page.pageName = levelPrefix + englishTranslation.name;
                    vm.pages.push(page);
                }

                if (page.childPage && page.childPage.length > 0) {
                    _.each(page.childPage, function (child) {
                        formatPages(child, levelPrefix + '--');    
                    });
                }
            }
            
        }
    }
})();