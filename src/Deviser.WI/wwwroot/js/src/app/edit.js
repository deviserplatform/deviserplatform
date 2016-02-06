(function () {

    var app = angular.module('deviserEdit', [
    'ui.router',
    'ui.bootstrap',
    'dndLists',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('EditCtrl', ['$scope', '$timeout', '$filter', '$q', '$modal', 'globals', 'sdUtil', 'layoutService', 'pageService',
        'contentTypeService', 'pageContentService', 'moduleService', 'pageModuleService', editCtrl]);

    app.controller('EditContentCtrl', ['$scope', '$modalInstance', 'pageContentService', 'contentInfo', editContentCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, $modal, globals, sdUtil, layoutService, pageService,
        contentTypeService, pageContentService, moduleService, pageModuleService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pageLayout = {};
        vm.selectedItem = {}
        vm.deletedElements = [];
        vm.layoutAllowedTypes = ["container"];
        vm.currentLanguage = "en-US";

        //Method binding
        vm.newGuid = sdUtil.getGuid;
        //vm.dragoverCallback = dragoverCallback;
        vm.dropCallback = dropCallback;
        vm.itemMoved = itemMoved;
        vm.deleteElement = deleteElement;
        vm.selectItem = selectItem;
        vm.copyElement = copyElement;
        vm.editContent = editContent;
        
        init();

        /////////////////////////////////////////////
        /*Function declarations only*/

        //Event handlers
        function dropCallback(event, index, item) {
            var parentScope = $(event.currentTarget).scope().$parent;
            createElement(item, parentScope.item.id);
            return item;
        }

        function itemMoved(item, index) {
            item.placeHolders.splice(index, 1);
        }

        function deleteElement(event, index, item) {
            deleteItem(item);
            return item;
        }

        function selectItem(item) {
            if (item.layoutTemplate === "content" || item.layoutTemplate === "module") {
                vm.selectedItem = item;
            }
        }

        function copyElement(contentType) {
            contentType.id = vm.newGuid();
        }

        function editContent(content) {
            var defer = $q.defer();
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'contenttypes/' + content.type + '.html',
                controller: 'EditContentCtrl as ecVM',
                resolve: {
                    contentInfo: function () {
                        var returnObject = content;

                        return returnObject;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                //$log.info('Modal Oked at: ' + new Date());
                defer.resolve('data received!');
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        //Private functions
        function init() {
            $q.all([
            getCurrentPage(),
            getLayouts(),
            getContentTypes(),
            getModules(),
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
                        id: sdUtil.getGuid(),
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
            pageContentService.get(appContext.currentPageId, vm.currentLanguage)
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
                    var pageContents = _.where(vm.pageContents, { containerId: item.id });
                    if (pageContents) {
                        _.each(pageContents, function (content) {
                            var index = content.sortOrder - 1;
                            var contentTypeInfo = JSON.parse(content.typeInfo);
                            item.placeHolders.splice(index, 0, contentTypeInfo); //Insert placeHolder into specified index
                        });
                    }

                    //Load modules if found
                    var pageModules = _.where(vm.currentPage.pageModule, { containerId: item.id });
                    if (pageModules) {
                        _.each(pageModules, function (pageModule) {
                            var index = pageModule.sortOrder - 1;
                            var module = {
                                id:pageModule.id,
                                layoutTemplate: "module",
                                type: "module",
                                module: pageModule.module
                            };//JSON.parse(pageModule.module);
                            item.placeHolders.splice(index, 0, module); //Insert placeHolder into specified index
                        })
                    }

                    if (item.placeHolders) {
                        iterateLayout(item.placeHolders);
                    }
                });
            }
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
                createModule(item, containerId);
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

        function createModule(item, containerId) {
            var pageModule = {
                id: item.id,
                pageId: appContext.currentPageId,
                moduleId: item.module.id,
                containerId: containerId,
                sortOrder: item.index
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

    function editContentCtrl($scope, $modalInstance, pageContentService, contentInfo) {
        var vm = this;

        vm.contentId = contentInfo.id;
        vm.save = save;
        vm.cancel = cancel;

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        //Event handlers
        function save() {            
            pageContentService.put(vm.pageContent).then(
                function (data) {
                    console.log(data);
                    $modalInstance.close('ok');
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
        }

        function cancel() {
            $modalInstance.dismiss('cancel');
        }

        //Private functions
        function init() {
            getPageContents();
        }

        function getPageContents() {
            pageContentService.get(vm.contentId).then(
                function (data) {
                    console.log(data);
                    vm.pageContent = data;
                    vm.typeInfo = JSON.parse(vm.pageContent.typeInfo);
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                });
        }

    };

}());