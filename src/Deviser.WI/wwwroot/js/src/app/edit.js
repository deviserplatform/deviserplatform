(function () {

    var app = angular.module('deviserEdit', [
    'ui.router',
    'ui.bootstrap',
    'dndLists',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('EditCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'sdUtil', 'layoutService', 'pageService',
        'contentTypeService', 'pageContentService', 'moduleService', 'pageModuleService', editCtrl]);


    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, sdUtil, layoutService, pageService,
        contentTypeService, pageContentService, moduleService, pageModuleService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pageLayout = {};
        vm.newGuid = sdUtil.getGuid;
        vm.dragoverCallback = dragoverCallback;
        vm.dropCallback = dropCallback;
        vm.logListEvent = logListEvent;
        vm.logEvent = logEvent;
        vm.saveLayout = saveLayout;
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
            getContentTypes()
        ]).then(function () {
            if (vm.currentPage.layoutId) {
                var selectedLayout = _.find(vm.layouts, function (layout) {
                    return layout.id === vm.currentPage.layoutId;
                });
                if (selectedLayout) {
                    vm.pageLayout = selectedLayout;
                }
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

        function processContentTypes(contentTypes) {
            if (contentTypes) {
                _.each(contentTypes, function (contentType) {
                    contentType.id = sdUtil.getGuid();
                    contentType.contentItems = [];
                    contentType.layoutTemplate = "content";
                });
            }
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
                    showMessage("success", "Layout has been saved");
                    getLayouts();
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
            }, globals.appSettings.alertLifeTime);
        }

    }

}());