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

    app.directive("sdLinkSelector", ['$compile', '$templateCache', 'assetService', sdLinkSelector]);

    function sdLinkSelector($compile, $templateCache, assetService) {
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
            var vm = this;



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
                pageService.getPages().then(function (pages) {
                    vm.pages = pages;
                    _.each(vm.pages, function (page) {
                        var englishTranslation = _.find(page.pageTranslation, { locale: "en-US" });
                        page.pageName = englishTranslation.name;
                    });
                })
            }
        }
    }
})();