(function () {

    var app = angular.module('deviserLayout', [
    'ui.router',
    'ui.bootstrap',
    'dndLists',
    'dev.modal',
    'dev.sdlib',    
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('LayoutCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'devUtil','editLayoutUtil', 'layoutService', 'pageService',
        'layoutTypeService', 'pageContentService', 'moduleService', 'pageModuleService','modalService', layoutCtrl]);


    ////////////////////////////////
    /*Function declarations only*/

    function layoutCtrl($scope, $timeout, $filter, $q, globals, devUtil, editLayoutUtil, layoutService, pageService,
        layoutTypeService, pageContentService, moduleService, pageModuleService, modalService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pageLayout = {};
        vm.selectedItem = {};
        vm.deletedElements = [];
        vm.layoutAllowedTypes = ["9341f92e-83d8-4afe-ad4a-a95deeda9ae3", "5a0a5884-da84-4922-a02f-5828b55d5c92"]; //Id of container and wrapper;     
       
        //Function binding
        vm.newGuid = devUtil.getGuid;
        vm.dragoverCallback = dragoverCallback;
        vm.dropCallback = dropCallback;
        vm.logListEvent = logListEvent;
        vm.logEvent = logEvent;
        vm.newLayout = newLayout;
        vm.selectLayout = selectLayout;
        vm.copyLayout = copyLayout;
        vm.saveLayout = saveLayout;
        vm.deleteLayout = deleteLayout;
        vm.selectItem = selectItem;
        vm.itemMoved = itemMoved;
        vm.deleteElement = deleteElement;
        vm.setColumnWidth = editLayoutUtil.setColumnWidth;
        //Init
        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            $q.all([
            getCurrentPage(),
            getLayouts(),
            getLayoutTypes()
            ]).then(function () {
                processLayoutTypes(vm.layoutTypes);
                if (vm.currentPage.layoutId) {
                    var selectedLayout = _.find(vm.layouts, function (layout) {
                        return layout.id === vm.currentPage.layoutId;
                    });
                    if (selectedLayout) {
                        selectLayout(selectedLayout);
                    }
                    else {
                        newLayout();
                    }
                }
            });
        }

        function getCurrentPage() {
            var defer = $q.defer();
            pageService.get(pageContext.currentPageId)
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
                layouts = _.filter(layouts, { isDeleted: false });
                _.forEach(layouts, function (item) {
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

        function processLayoutTypes(layoutTypes) {
            if (layoutTypes) {
                _.forEach(layoutTypes, function (layoutType) {
                    layoutType.layoutTypeId = layoutType.id;
                    layoutType.id = devUtil.getGuid();
                    layoutType.placeHolders = [];
                    layoutType.layoutTypeIds = layoutType.layoutTypeIds.replace(/\s+/g, '');
                    layoutType.allowedTypes = (layoutType.layoutTypeIds) ? layoutType.layoutTypeIds.split(",") : "";
                    layoutType.type = layoutType.name;
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
            vm.pageLayout.id = undefined;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = false;
            vm.isLayoutEdit = true;
            vm.pageLayout.placeHolders = [];
        }

        function selectLayout(layout) {
            vm.pageLayout = layout;
            vm.pageLayout.isChanged = true;
            processplaceHolders(vm.pageLayout.placeHolders);
        }

        function copyLayout() {
            vm.pageLayout.id = undefined;
            vm.pageLayout.name = "";
            vm.pageLayout.isChanged = true;
        }

        function saveLayout() {
            processplaceHolders(vm.pageLayout.placeHolders, true);

            //vm.pageLayout.config = JSON.stringify(vm.pageLayout.placeHolders);
            vm.pageLayout.pageId = pageContext.currentPageId;
            if (vm.pageLayout.id) {
                //Update layout
                layoutService.put(vm.pageLayout)
                .then(function (data) {
                    //console.log(data);
                    vm.pageLayout.id = data.id;
                    vm.pageLayout.placeHolders = data.placeHolders;
                    vm.pageLayout.isChanged = false;
                    showMessage("success", "Layout has been saved");
                    processplaceHolders(vm.pageLayout.placeHolders);
                    vm.isLayoutEdit = false;
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
                    getLayouts().then(function () {
                        processplaceHolders(vm.pageLayout.placeHolders);
                    });
                    vm.isLayoutEdit = false;
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
            }
        }       

        function processplaceHolders(placeHolders, toSave) {
            if (placeHolders) {
                _.forEach(placeHolders, function (item, index) {
                    console.log(item)
                    var masterLayout = _.find(vm.layoutTypes, { layoutTypeId: item.layoutTypeId });
                    if (masterLayout) {
                        item.label = masterLayout.label;
                    }

                    if (toSave && item.properties) {
                        _.forEach(item.properties, function (prop, index) {
                            prop.optionList = null;
                        });
                    }
                    else {
                        syncPropertyForElement(item);
                        //Refresh selected item
                        if (item.id === vm.selectedItem.id) {
                            vm.selectedItem = item;
                        }
                    }
                    
                    item.sortOrder = index + 1;

                    if (item.placeHolders) {
                        processplaceHolders(item.placeHolders, toSave);
                    }
                });
            }
        }

        function deleteLayout() {
            var labels = {
                title: 'Delete Confirmation',
                body: 'Are you sure to delete the layout',
                okLabel: 'Yes',
                cancelLabel: 'No'
            };

            var modalInstance = modalService.showConfirmation(labels);

            modalInstance.then(function () {
                layoutService.remove(vm.pageLayout.id).then(function (data) {
                console.log('Layout deletion agreed at:' + new Date());
                showMessage("success", "Layout has been removed");
                getLayouts();
                newLayout();
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                    })
            },           
                function () {
                    console.log('Layout deletion disagreed at:' + new Date());
                });
        }

        function selectItem(item) {
            var propertiesValue = item.properties;
            syncPropertyForElement(item);
            //columnwidth update - hard coded behaviour only for property 'column_width'
            var columnWidthProp = editLayoutUtil.getColumnWidthProperty(propertiesValue);            
            if (columnWidthProp && !columnWidthProp.value) {
                var columnWidth = _.find(columnWidthProp.optionList.list, { name: editLayoutUtil.const.defaultWidth })
                columnWidthProp.value = columnWidth.id;
            }                

            vm.selectedItem = item;
        }

        function syncPropertyForElement(element) {
            var propertiesValue = element.properties;
            var masterLayout = _.find(vm.layoutTypes, { layoutTypeId: element.layoutTypeId });
            var masterProperties = masterLayout.properties;
            _.forEach(masterProperties, function (prop) {
                if (prop) {
                    var propVal = _.find(propertiesValue, { id: prop.id });
                    if (propVal) {
                        //Property exist, update property label
                        propVal.label = prop.label;
                        propVal.description = prop.description;
                        propVal.defaultValue = prop.defaultValue;
                        propVal.optionList = prop.optionList;
                        propVal.optionListId = prop.optionListId;
                    }
                    else {
                        //Property not exist, add the property                      
                        element.properties.push(angular.copy(prop));
                    }
                }
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

        function itemMoved(placeHolders, index) {
            placeHolders.splice(index, 1);
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
