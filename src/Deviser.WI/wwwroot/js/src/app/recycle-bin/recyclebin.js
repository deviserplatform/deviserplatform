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
        vm.restorePage = restorePage;
        vm.restorePageContent = restorePageContent;
        vm.restorePageModule = restorePageModule;
        vm.deletePage = deletePage;
        vm.deletePageContent = deletePageContent;
        vm.deletePageModule = deletePageModule;
        getDeletedPages();
        getDeletedPageContents();
        getDeletedPageModules();
        //////////////////////////////////
        ///*Function declarations only*/       

        function getDeletedPages() {
            pageService.getDeletedPages().then(function (pages) {
                _.forEach(pages, function (page) {
                    page.pageTranslation = _.find(page.pageTranslation, { locale: pageContext.siteLanguage})
                });

                vm.pages = pages;
            }, function (error) {
                showMessage("error", "Cannot get the pages, please contact administrator");
            });
        }

        function getDeletedPageContents() {
            pageContentService.getDeletedPageContents().then(function (pageContents) {
                _.forEach(pageContents, function (pageContents) {
                    pageContents.page.pageTranslation = _.find(pageContents.page.pageTranslation, { locale: pageContext.siteLanguage })
                });
                vm.pageContents = pageContents;
            }, function (error) {
                showMessage("error", "Cannot get the page content elements, please contact administrator");
            });

        }

        function getDeletedPageModules() {
            pageModuleService.getDeletedPageModules().then(function (pageModules) {
                _.forEach(pageModules, function (pageModules) {
                    pageModules.page.pageTranslation = _.find(pageModules.page.pageTranslation, { locale: pageContext.siteLanguage })
                });
                vm.pageModules = pageModules;
            }, function (error) {
                showMessage("error", "Cannot get the page modules, please contact administrator");
            });

        }

        function restorePage(id) {           
            pageService.restorePage(id).then(function (page) {
                if (page.isDeleted === false)
                    getDeletedPages();
                showMessage("success", "The page has been restored");
            },function(error){
                showMessage("error", "Cannot restore the page, please contact administrator");
            });
        }

        function restorePageContent(id) {
            pageContentService.restorePageContent(id).then(function (pageContent) {
                if (pageContent.isDeleted === false)
                    getDeletedPageContents();
                showMessage("success", "The page content has been restored");
            }, function (error) {
                showMessage("error", "Cannot restore the page content, please contact administrator");
            });
        }

        function restorePageModule(id) {
            pageModuleService.restorePageModule(id).then(function (pageModule){
                if (pageModule.isDeleted === false)
                    getDeletedPageModules();
                showMessage("success", "The page module has been restored");
            }, function (error) {
                showMessage("error", "Cannot restore the page module, please contact administrator");
            });
        }

        function deletePage(id) {
            pageService.deletePage(id).then(function (result) {
                getDeletedPages();
                showMessage("success", "The page has been deleted");
            }, function (error) {
                showMessage("error", "Cannot delete the page, please contact administrator");
            });       

        }

        function deletePageContent(id) {
            pageContentService.deletePageContent(id).then(function (result) {
                getDeletedPageContents();
                showMessage("success", "The page content has been deleted");
            }, function (error) {
                showMessage("error", "Cannot delete the page content, please contact administrator");
            });

        }

        function deletePageModule(id) {
            pageModuleService.deletePageModule(id).then(function (result) {
                getDeletedPageModules();
                showMessage("success", "The page module has been deleted");
            }, function (error) {
                showMessage("error", "Cannot delete the page module, please contact administrator")
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