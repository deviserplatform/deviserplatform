(function () {
    var app = angular.module('modules.app.languageSelector', [
    'ui.router',
    'ui.bootstrap',
    'ui.sortable',
    'ui.select',
    'deviser.services',
    'ngFileUpload',
    'angular-img-cropper'
    ]);

    app.directive("devLanguageSelector", ['$compile', '$templateCache', 'assetService', devLanguageSelector]);

    function devLanguageSelector($compile, $templateCache, assetService) {
        var returnObject = {
            restrict: "A",
            controller: ['$scope', '$uibModal', 'languageService', ctrl],
            controllerAs: 'locVM',
            bindToController: true,
            link: link,
            scope: {
                selectedLocale: '=',
                changeLanguage:'&',
                label: '@'
            }
        };

        return returnObject;
        /////////////////////////////////////////////
        /*Function declarations only*/
        function link(scope, element, attrs) {
            template = $templateCache.get("app/components/languageSelector.tpl.html");
            element.html(template);
            $compile(element.contents())(scope);
        }

        function ctrl($scope, $uibModal, languageService) {
            var vm = this;

            vm.changeLocalLanguage = changeLocalLanguage;

            init();

            /////////////////////////////////////////////
            /*Function declarations only*/
            function init() {
                getSiteLanguages();
            }

            //Event handlers
            function changeLocalLanguage() {
                console.log(vm);
                vm.changeLanguage();
            }

            function getSiteLanguages() {
                languageService.getSiteLanguages().then(function (languages) {
                    vm.languages = languages;
                }, function (error) {
                    showMessage("error", "Cannot get all languages, please contact administrator");
                });
            }
        }
    }
})();