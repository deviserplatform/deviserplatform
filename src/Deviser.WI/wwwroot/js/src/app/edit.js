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
        //vm.dragoverCallback = dragoverCallback;
        vm.dropCallback = dropCallback;
        vm.itemMoved = itemMoved;
        vm.deleteElement = deleteElement;
        vm.selectItem = selectItem;
        vm.copyElement = copyElement;
        vm.selectedItem = {}
        vm.deletedElements = [];
        vm.layoutAllowedTypes = ["container"];
        vm.currentLanguage = "en-US";

        //vm.models = {
        //    templates: [
        //        { type: "text", id: sdUtil.getGuid() },
        //        { type: "container", id: sdUtil.getGuid(), placeHolders: [] },
        //        { type: "column", id: sdUtil.getGuid(), placeHolders: [] }
        //    ]
        //};

        $q.all([
            getCurrentPage(),
            getLayouts(),
            getContentTypes(),
            getPageContents()
        ]).then(function () {
            if (vm.currentPage.layoutId) {
                var selectedLayout = _.find(vm.layouts, function (layout) {
                    return layout.id === vm.currentPage.layoutId;
                });
                if (selectedLayout) {
                    vm.pageLayout = selectedLayout;
                    loadPageContents();
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
                deferred.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function getPageContents() {
            pageContentService.get(vm.currentLanguage, appContext.currentPageId)
            .then(function (pageContents) {
                vm.pageContents = pageContents;
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            })
        }

        function processContentTypes(contentTypes) {
            if (contentTypes) {
                _.each(contentTypes, function (contentType) {
                    contentType.id = sdUtil.getGuid();
                    contentType.layoutTemplate = "content";
                });
            }
        }

        function loadPageContents() {
            iterateLayout(vm.pageLayout.placeHolders);
        }

        function iterateLayout(placeHolders) {
            if (placeHolders) {
                _.each(placeHolders, function (item) {
                    //console.log(item)

                    //Load content items if found
                    var placeHolderInfos = _.where(vm.pageContents, { containerId: item.id });
                    if (placeHolderInfos) {
                        _.each(placeHolderInfos, function (placeHolderInfo) {
                            var index = placeHolderInfo.sortOrder - 1;
                            var placeHolder = JSON.parse(placeHolderInfo.typeInfo);
                            item.placeHolders.splice(index, 0, placeHolder); //Insert placeHolder into specified index
                        });
                    }

                    if (item.placeHolders) {
                        iterateLayout(item.placeHolders);
                    }
                });
            }
        }

        //function dragoverCallback(event, index, item) {
        //    console.log(item)
        //    return index > 0;
        //}

        function copyElement(contentType) {
            contentType.id = vm.newGuid();
        }

        function dropCallback(event, index, item) {
            var parentScope = $(event.currentTarget).scope().$parent;
            createElement(item, parentScope.item.id);
            return item;
        }

        function deleteElement(event, index, item) {
            //deleteItem(item);
            return item;
        }

        function selectItem(item) {
            if (item.layoutTemplate === "content" || item.layoutTemplate === "module") {
                vm.selectedItem = item;
            }
        }

        function itemMoved(item, index) {
            item.placeHolders.splice(index, 1);
        }

        function deleteItem(item) {
            if (item.layoutTemplate === "content") {
                deleteContent(item.id);
            }
            else if (item.layoutTemplate === "module") {
                deleteModule(item.id);
            }
        }

        function createElement(item, containerId) {
            if (item.layoutTemplate === "content") {
                createContent(item, containerId);
            }
            else if (item.layoutTemplate === "module") {
                createModule(item.module.id, containerId);
            }
        }

        function createContent(item, containerId) {
            var content = {
                id: item.id,
                pageId: appContext.currentPageId,
                typeInfo: JSON.stringify(item),
                containerId: containerId,
                sortOrder : item.index,
                cultureCode: vm.currentLanguage //TODO: get this from appContext
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
            }, globals.appSettings.alertLifeTime);
        }

    }

}());