(function () {

    var app = angular.module('deviserLayout', [
    'ui.router',
    'ui.bootstrap',
    'dndLists',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('LayoutCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'layoutService', 'pageService',
        'contentTypeService', 'pageContentService', 'moduleService', 'pageModuleService', layoutCtrl]);


    ////////////////////////////////
    /*Function declarations only*/

    function layoutCtrl($scope, $timeout, $filter, $q, globals, layoutService, pageService, contentTypeService,
        pageContentService, moduleService, pageModuleService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pageLayout = {};
        vm.newGuid = getGuid;
        vm.dragoverCallback = dragoverCallback;
        vm.dropCallback = dropCallback;
        vm.logListEvent = logListEvent;
        vm.logEvent = logEvent;
        vm.newLayout = newLayout;
        vm.selectLayout = selectLayout;
        vm.copyLayout = copyLayout;
        vm.saveLayout = saveLayout;
        vm.deleteLayout = deleteLayout;
        vm.itemMoved = itemMoved;
        vm.deleteElement = deleteElement;
        vm.selectedItem = {}
        vm.deletedElements = [];

        //vm.models = {
        //    templates: [
        //        { type: "text", id: getGuid() },
        //        { type: "container", id: getGuid(), contentItems: [] },
        //        { type: "column", id: getGuid(), contentItems: [] }
        //    ]
        //};

        $q.all([
            getCurrentPage(),
            getLayouts(),
            getContentTypes(),
            getModules()
        ]).then(function () {
            if (vm.currentPage.layoutId) {
                vm.pageLayout = _.find(vm.layouts, function (layout) {
                    return layout.id === vm.currentPage.layoutId;
                });
            }
            processContentTypes(vm.contentTypes);
        });

        //$scope.$watch('models.dropzones', function (model) {
        //    vm.modelAsJson = angular.toJson(model, true);
        //}, true);

        /////////////////////////////////////////////
        /*Function declarations only*/
        function getCurrentPage() {
            var defer = $q.defer();
            pageService.get(appContext.currentPageId)
            .then(function (data) {
                vm.currentPage = data;
                defer.resolve('data received!');
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function getLayouts() {
            var defer = $q.defer();
            layoutService.get()
            .then(function (layouts) {
                console.log(layouts);
                //Processing the data
                layouts = _.where(layouts, { isDeleted: false });
                _.each(layouts, function (item) {
                    if (item && !item.contentItems) {
                        item.contentItems = [];
                    }
                });
                vm.layouts = layouts;
                defer.resolve('data received!');
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function getContentTypes() {
            var defer = $q.defer();
            contentTypeService.get()
            .then(function (data) {
                vm.contentTypes = data;
                defer.resolve('data received!');
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function getModules() {
            var defer = $q.defer();
            moduleService.get()
            .then(function (data) {
                var modules = data;
                vm.modules = [];
                _.each(modules, function (module) {
                    vm.modules.push({
                        id: getGuid(),
                        layoutTemplate: "module",
                        type: "module",
                        module: module
                    });
                });
                defer.resolve('data received!');
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function processContentTypes(contentTypes) {
            if (contentTypes) {
                _.each(contentTypes, function (contentType) {
                    contentType.id = getGuid();
                    if (contentType.isRepeater) {
                        contentType.contentItems = [];
                        if (contentType.type === "column") {
                            contentType.layoutTemplate = "column";
                        }
                        else {
                            contentType.layoutTemplate = "repeater";
                        }
                    }
                    else {
                        contentType.layoutTemplate = "item";
                    }
                });
            }
        }

        function newLayout() {
            vm.pageLayout.id = null;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = false;
            vm.pageLayout.contentItems = [];
        }

        function selectLayout(layout) {
            vm.pageLayout = layout;
            vm.pageLayout.isChanged = true;
            processContentItems(vm.pageLayout.contentItems);
        }

        function copyLayout() {
            vm.pageLayout.id = null;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = true;
        }

        function saveLayout() {
            processContentItems(vm.pageLayout.contentItems);

            /*TODO:
            Do all the following tasks in server side, this improves the efficienty and reduce number of calls between server and client.
            1) Modify the Layout service so that it can accept layoutDTO object which contains:
                - pageLayoutId
                - pageLayoutName
                - contentItems - recursive object instead of config string
                - pageId - currentPageId
                - isCopy flag - set true if the layout is being copied

            2) Detect whether the layout is new/copy/update - check for the isCopy flag to identify the state
                - If layout is new/update, jsut update the layout
                - If layout is copy, delete all contents and modules and create newone. 
            
            3) Loop entire contentItems tree, create newone.

            ------------------------------------------------

            1) Get content and modules of the current page and display it in the layout view
            2) After creating a content/module, display it in layout view.
            3) Display the deleted contents/modules in separate dnd-list (unassigned lsit)
            4) While changing/copying/creating new layout do not delete all content/modules, instead move all the old modules/content into unassigned list.
            5) If items can be moved from unassigned list to page container.


            Issue:
            
            1) What happens if a container has more than one content/module? How the contents/modules are displayed in layout module?

            -------------------------------------------------
            Layout Design Decisions
            -------------------------------------------------
            1) Layout should have only containers, rows and columns (i.e repeater elements). It can have properties such as css class and other HTML attributes.
            2) In database, table PageContent should be renamed to PageElement, all elements should be translatable.
            3) PageElement has all the information about the element such as element type, properties, element data, page module id (if element is a module).
            4) Do not separate layout view and element view. However, layout and element information should be stored in different entity. This shows clear separation of page layout and page elements.
            5) Load the layout information, after that load the page elements. 

            



            */

            //vm.pageLayout.config = JSON.stringify(vm.pageLayout.contentItems);
            vm.pageLayout.pageId = appContext.currentPageId;
            if (vm.pageLayout.id > 0) {
                //Update layout
                layoutService.put(vm.pageLayout)
                .then(function (data) {
                    //console.log(data);
                    vm.pageLayout.id = data.id;                    
                    vm.pageLayout.contentItems = data.contentItems;
                    vm.pageLayout.isChanged = false;
                    updatePageLayoutPage(vm.pageLayout.id);
                    showMessage("success", "Layout has been saved");
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
            }
            else {
                //Create new layout
                layoutService.post(vm.pageLayout)
                .then(function (data) {
                    console.log(data);
                    vm.pageLayout.id = data.id;                    
                    vm.pageLayout.contentItems = data.contentItems;
                    vm.pageLayout.isChanged = false;
                    updatePageLayoutPage(vm.pageLayout.id);
                    showMessage("success", "Layout has been saved");
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
            }
        }

        function processContentItems(contentItems) {
            if (contentItems) {
                _.each(contentItems, function (item) {
                    console.log(item)
                    if (item.contentItems) {
                        processContentItems(item.contentItems);
                    }
                });
            }
        }

        function deleteLayout() {
            layoutService.remove(vm.pageLayout.id).then(function (data) {
                console.log(data);
                showMessage("success", "Layout has been removed");
                getLayouts();
                newLayout();
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function updatePageLayoutPage(layoutId) {
            vm.currentPage.layoutId = layoutId;
            pageService.put(vm.currentPage).then(function (data) {
                console.log(data);
                //vm.pageLayout = data;
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function dragoverCallback(event, index, external, type) {
            //console.log(index);
            return index > 0;
        }

        function dropCallback(event, index, item) {
            createElement(item);
            return item;
        }

        function deleteElement(event, index, item) {
            deleteItem(item);
            return item;
        }

        function itemMoved(item, index) {
            item.contentItems.splice(index, 1);
        }

        function deleteItem(item) {            
            if (item.type === "text") {
                deleteContent(item.id);
            }
            else if (item.type === "module") {
                deleteModule(item.id);
            }
        }

        function createElement(item) {
            if (item.type === "text") {
                createContent(item.id);
            }
            else if (item.type === "module") {
                createModule(item.module.id, item.id);
            }
        }

        function createContent(containerId) {
            var content = {
                pageId: appContext.currentPageId,
                containerId: containerId,
                cultureCode: "en-US" //TODO: get this from appContext
            }
            pageContentService.post(content).then(function (data) {
                console.log(data);
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function createModule(moduleId, containerId) {
            var pageModule = {
                pageId: appContext.currentPageId,
                moduleId: moduleId,
                containerId: containerId
                //Modules are not multilingual
            }
            pageModuleService.post(pageModule).then(function (data) {
                console.log(data);
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function deleteContent(contentId) {
            pageContentService.remove(contentId).then(function (data) {
                console.log(data);
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function deleteModule(moduleId) {
            pageModuleService.remove(moduleId).then(function (data) {
                console.log(data);
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function logListEvent(action, event, index, external, type) {
            var message = external ? 'External ' : '';
            message += type + ' element is ' + action + ' position ' + index;
            vm.logEvent(message, event);
        }

        function logEvent(message, event) {
            console.log(message, '(triggered by the following', event.type, 'event)');
            console.log(event);
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

        function getGuid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        }

    }

}());
