(function () {

    var app = angular.module('deviserEdit', [
    'ui.router',
    'ui.bootstrap',
    'dndLists',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('editCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'layoutService', 'pageService',
       'contentTypeService', 'pageContentService', editCtrl]);


    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, layoutService, pageService, contentTypeService,
            pageContentService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.selectedItem = {};
        vm.pageContents = [];
        vm.save = save;

        getPageContents();

        ////////////////////////////////
        /*Function declarations only*/

        function getPageContents() {
            pageContentService.get(appContext.currentCulture, appContext.currentPageId).then(
                function (data) {
                    console.log(data);
                    vm.pageContents = data;
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
        }

        function save(content, contentId, containerId) {
            console.log(content);
            console.log(contentId);
            //var pageContent = {
            //    id: contentId,
            //    pageId: appContext.currentPageId,
            //    cultureCode: appContext.currentCulture,
            //    contentData: content,
            //    containerId: containerId
            //};
            pageContentService.put(content).then(
                function (data) {
                    console.log(data);
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
        }

        function showMessage(messageType, messageContent) {
            vm.message = {
                messageType: messageType,
                content: messageContent
            }

            $timeout(function () {
                vm.message = {
                    messageType: "",
                    content: ""
                };
            }, 3000);
        }

    }

}());