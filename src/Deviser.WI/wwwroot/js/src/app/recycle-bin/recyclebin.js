(function () {

    var app = angular.module('deviser.recycleBin', [
        'ui.router',
        'ui.bootstrap',
        'ui.tree',
        'sd.sdlib',
        'deviser.services',
        'deviser.config'
    ]);

    app.controller('RecycleBinCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'pageService', 'pageContentService', 'pageModuleService', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, pageService, pageContentService, pageModuleService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        getPages();
        getPageContents();
        getPageModules();
        //////////////////////////////////
        ///*Function declarations only*/       

        function getPages() {
            pageService.get().then(function (pages) {
                vm.pages = pages;
            }, function (error) {
                showMessage("error", "Cannot get the pages, please contact administrator");
            });
        }

        function getPageContents() {
            pageContentService.get().then(function (pageContents) {
                vm.pageContents = pageContents;
            }, function (error) {
                showMessage("error", "Cannot get the page content elements, please contact administrator");
            });

        }

        function getPageModules() {
            pageModuleService.get().then(function (pageModules) {
                vm.pageModules = pageModules;
            }, function (error) {
                showMessage("error", "Cannot get the page modules, please contact administrator");
            });

        }

        function showMessage(messageType, messageContent) {
            vm.message = {
                messageType: messageType,
                content: messageContent
            }

            $timeout(function () {
                vm.message = {};
            }, globals.appSettings.alertLifeTime)
        }
    }

}());