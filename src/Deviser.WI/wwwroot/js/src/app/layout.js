(function () {

    var app = angular.module('deviserLayout', [
    'ui.router',
    'ui.bootstrap',
    'dndLists',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('LayoutCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'sdUtil', 'layoutService', 'pageService',
        'layoutTypeService', 'pageContentService', 'moduleService', 'pageModuleService', layoutCtrl]);


    ////////////////////////////////
    /*Function declarations only*/

    function layoutCtrl($scope, $timeout, $filter, $q, globals, sdUtil, layoutService, pageService,
        layoutTypeService, pageContentService, moduleService, pageModuleService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pageLayout = {};
        vm.newGuid = sdUtil.getGuid;
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
        vm.layoutAllowedTypes = ["container"];

        //vm.models = {
        //    templates: [
        //        { type: "text", id: sdUtil.getGuid() },
        //        { type: "container", id: sdUtil.getGuid(), contentItems: [] },
        //        { type: "column", id: sdUtil.getGuid(), contentItems: [] }
        //    ]
        //};

        $q.all([
            getCurrentPage(),
            getLayouts(),
            getLayoutTypes()
        ]).then(function () {
            if (vm.currentPage.layoutId) {
                var selectedLayout = _.find(vm.layouts, function (layout) {
                    return layout.id === vm.currentPage.layoutId;
                });
                if (selectedLayout) {
                    vm.pageLayout = selectedLayout;
                }
                else {
                    newLayout();
                }
            }
            processContentTypes(vm.layoutTypes);
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

        function getLayoutTypes() {
            var defer = $q.defer();
            layoutTypeService.get()
            .then(function (data) {
                vm.layoutTypes = data;
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
                    contentType.id = sdUtil.getGuid();
                    contentType.contentItems = [];
                    if (contentType.type === "column") {
                        contentType.layoutTemplate = "column";
                    }
                    else if (contentType.type === "container") {
                        contentType.layoutTemplate = "container";
                    }
                    else if (contentType.type === "row") {
                        contentType.layoutTemplate = "row";
                    }
                    else {
                        contentType.layoutTemplate = "repeater";
                    }
                });
            }
        }

        function newLayout() {
            vm.pageLayout = {};
            vm.pageLayout.id = 0;
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
            vm.pageLayout.id = 0;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = true;
        }

        function saveLayout() {
            processContentItems(vm.pageLayout.contentItems);

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

        function dragoverCallback(event, index, item) {
            console.log(item)
            return index > 0;
        }

        function dropCallback(event, index, item) {
            //createElement(item);
            return item;
        }

        function deleteElement(event, index, item) {
            //deleteItem(item);
            return item;
        }

        function itemMoved(item, index) {
            item.contentItems.splice(index, 1);
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

       

    }

}());
