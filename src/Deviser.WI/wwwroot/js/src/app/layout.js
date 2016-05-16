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
        vm.selectedItem = {}
        vm.deletedElements = [];
        vm.layoutAllowedTypes = [];

        //Function binding
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

        //Init
        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            $q.all([
            getCurrentPage(),
            getLayouts(),
            getLayoutTypes(),
            getLayoutAllowedTypes()
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
                processLayoutTypes(vm.layoutTypes);
            });
        }

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
                    if (item && !item.placeHolders) {
                        item.placeHolders = [];
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

        function getLayoutAllowedTypes() {
            var defer = $q.defer();
            layoutTypeService.getAllowedRootTypes()
            .then(function (data) {
                vm.layoutAllowedTypes = data;
                defer.resolve('data received!');
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }
        
        function processLayoutTypes(layoutTypes) {
            if (layoutTypes) {
                _.each(layoutTypes, function (layoutType) {
                    layoutType.id = sdUtil.getGuid();
                    layoutType.placeHolders = [];
                    if (layoutType.type === "column") {
                        layoutType.layoutTemplate = "column";
                    }
                    else if (layoutType.type === "container") {
                        layoutType.layoutTemplate = "container";
                    }
                    else if (layoutType.type === "row") {
                        layoutType.layoutTemplate = "row";
                    }
                    else {
                        layoutType.layoutTemplate = "repeater";
                    }
                });
            }
        }

        function newLayout() {
            vm.pageLayout = {};
            vm.pageLayout.id = 0;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = false;
            vm.pageLayout.placeHolders = [];
        }

        function selectLayout(layout) {
            vm.pageLayout = layout;
            vm.pageLayout.isChanged = true;
            processplaceHolders(vm.pageLayout.placeHolders);
        }

        function copyLayout() {
            vm.pageLayout.id = 0;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = true;
        }

        function saveLayout() {
            processplaceHolders(vm.pageLayout.placeHolders);

            //vm.pageLayout.config = JSON.stringify(vm.pageLayout.placeHolders);
            vm.pageLayout.pageId = appContext.currentPageId;
            if (vm.pageLayout.id > 0) {
                //Update layout
                layoutService.put(vm.pageLayout)
                .then(function (data) {
                    //console.log(data);
                    vm.pageLayout.id = data.id;
                    vm.pageLayout.placeHolders = data.placeHolders;
                    vm.pageLayout.isChanged = false;                    
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
                    vm.pageLayout.placeHolders = data.placeHolders;
                    vm.pageLayout.isChanged = false;
                    showMessage("success", "Layout has been saved");
                    getLayouts();
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
            }
        }

        function processplaceHolders(placeHolders) {
            if (placeHolders) {
                _.each(placeHolders, function (item, index) {
                    console.log(item)
                    item.sortOrder = index + 1;
                    if (item.placeHolders) {
                        processplaceHolders(item.placeHolders);
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
            item.placeHolders.splice(index, 1);
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
            }, globals.appSettings.alertLifeTime);
        }

    }

}());
