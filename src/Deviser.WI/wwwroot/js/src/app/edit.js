(function () {

    var app = angular.module('deviserEdit', [
    'ui.router',
    'ui.sortable',
    'ui.bootstrap',
    'ui.select',
    'dndLists',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config',
    'modules.app.imageManager'
    ]);

    app.controller('EditCtrl', ['$scope', '$timeout', '$filter', '$q', '$uibModal', 'globals', 'sdUtil', 'layoutService', 'pageService',
        'contentTypeService', 'pageContentService', 'moduleService', 'pageModuleService', editCtrl]);

    app.controller('EditContentCtrl', ['$scope', '$uibModalInstance', '$q', 'sdUtil', 'languageService',
        'pageContentService', 'contentTranslationService', 'contentInfo', editContentCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, $uibModal, globals, sdUtil, layoutService, pageService,
        contentTypeService, pageContentService, moduleService, pageModuleService) {
        var vm = this;

        var containerIds = [];

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pageLayout = {};
        vm.selectedItem = {}
        vm.deletedElements = [];
        vm.layoutAllowedTypes = ["container"];

        //Method binding
        vm.newGuid = sdUtil.getGuid;
        //vm.dragoverCallback = dragoverCallback;
        vm.dropCallback = dropCallback;
        vm.insertedCallback = insertedCallback;
        vm.itemMoved = itemMoved;
        vm.deleteElement = deleteElement;
        vm.selectItem = selectItem;
        vm.copyElement = copyElement;
        vm.editContent = editContent;
        vm.uaLayout = {};

        var pageLayout = {};

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/

        //Event handlers
        function insertedCallback(event, index) {
            var parentScope = $(event.currentTarget).scope().$parent;
            var containerId = parentScope.item.id;

            updateElements(parentScope.item);

            //item.sortOrder = index + 1;
            //createElement(item, containerId);            
            //return item;
        }

        function dropCallback(event, index, item) {
            item.sortOrder = index + 1;
            return item;
        }

        function itemMoved(item, index) {
            item.placeHolders.splice(index, 1);
            updateElements(item);
            //sorting elements after old element has been moved
            //sortElements();
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
            var modalInstance = $uibModal.open({
                animation: true,
                size: 'lg',
                openedClass: 'edit-content-modal',
                backdrop: 'static',                
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

            getCurrentPage().then(function () {

                $q.all([
                    getLayout(),
                    getContentTypes(),
                    getModules(),
                    getPageContents()
                ]).then(function () {
                    loadPageContents();
                    processContentTypes(vm.contentTypes);
                });

            });
        }

        function getCurrentPage() {
            var defer = $q.defer();
            pageService.get(appContext.currentPageId)
            .then(function (data) {
                vm.currentPage = data;
                defer.resolve(data);
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function getLayout() {
            var defer = $q.defer();
            layoutService.get(vm.currentPage.layoutId)
            .then(function (layout) {
                //console.log(layout);
                //vm.pageLayout = layout;
                pageLayout = layout;
                defer.resolve('data received');
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
            var defer = $q.defer();
            pageContentService.get(appContext.currentPageId, appContext.currentCulture)
            .then(function (pageContents) {
                vm.pageContents = pageContents;
                defer.resolve(pageContents);
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
                    contentType.layoutTemplate = "content";
                });
            }
        }

        function loadPageContents() {

            var unAssignedContents = [],
                unAssignedModules = [];

            //First, position elements in correct order and then assign the pageLayout to VM.
            positionPageContents(pageLayout.placeHolders);
            vm.pageLayout = pageLayout;
            vm.pageLayout.pageId = vm.currentPage.id;

            var unAssignedSrcConents = _.reject(vm.pageContents, function (content) {
                return _.contains(containerIds, content.containerId);
            });

            var unAssignedSrcModules = _.reject(vm.currentPage.pageModule, function (module) {
                return _.contains(containerIds, module.containerId);
            });

            _.each(unAssignedSrcConents, function (content) {
                var index = content.sortOrder - 1;
                var contentTypeInfo = JSON.parse(content.typeInfo);
                unAssignedContents.push(contentTypeInfo);
            });

            _.each(unAssignedSrcModules, function (pageModule) {
                var index = pageModule.sortOrder - 1;
                var module = {
                    id: pageModule.id,
                    layoutTemplate: "module",
                    type: "module",
                    module: pageModule.module
                };//JSON.parse(pageModule.module);
                unAssignedModules.push(module);
            })

            vm.uaLayout.placeHolders = [];

            vm.uaLayout.placeHolders = vm.uaLayout.placeHolders.concat(unAssignedContents);
            vm.uaLayout.placeHolders = vm.uaLayout.placeHolders.concat(unAssignedModules);

        }

        function positionPageContents(placeHolders) {
            if (placeHolders) {
                _.each(placeHolders, function (item) {
                    //console.log(item)

                    //adding containerId to filter unallocated items in a separate dndlist
                    containerIds.push(item.id);

                    //Load content items if found
                    var pageContents = _.where(vm.pageContents, { containerId: item.id });
                    if (pageContents) {
                        _.each(pageContents, function (content) {
                            var index = content.sortOrder - 1;
                            var contentTypeInfo = JSON.parse(content.typeInfo);
                            //contentTypeInfo.sortOrder = content.sortOrder;
                            //item.placeHolders.splice(index, 0, contentTypeInfo); //Insert placeHolder into specified index
                            item.placeHolders.push(contentTypeInfo);
                        });
                    }

                    //Load modules if found
                    var pageModules = _.where(vm.currentPage.pageModule, { containerId: item.id });
                    if (pageModules) {
                        _.each(pageModules, function (pageModule) {
                            var index = pageModule.sortOrder - 1;
                            var module = {
                                id: pageModule.id,
                                layoutTemplate: "module",
                                type: "module",
                                module: pageModule.module,
                                sortOrder: pageModule.sortOrder
                            };//JSON.parse(pageModule.module);
                            //item.placeHolders.splice(index, 0, module); //Insert placeHolder into specified index
                            item.placeHolders.push(module);
                        })
                    }

                    item.placeHolders = _.sortBy(item.placeHolders, 'sortOrder');

                    if (item.placeHolders) {
                        positionPageContents(item.placeHolders);
                    }
                });
            }
        }

        function updateElements(container) {

            var elementsToSort = {
                contents: [],
                modules: []
            };

            _.forEach(container.placeHolders, function (item, index) {
                item.sortOrder = index + 1;
                if (item.layoutTemplate === "content") {
                    elementsToSort.contents.push({
                        element: item,
                        containerId: container.id
                    });
                }
                else if (item.layoutTemplate === "module") {
                    elementsToSort.modules.push({
                        element: item,
                        containerId: container.id
                    });
                }
            });

            //updatePageContents(elementsToSort);

            //Clone current layout and get layout only (without contents and modules)
            var layoutOnly = jQuery.extend(true, {}, vm.pageLayout);
            filterLayout(layoutOnly);
            console.log("--------------------------");
            console.log("Layout only");
            console.log(layoutOnly)

            $q.all([
                updatePageContents(elementsToSort),
                updateModules(elementsToSort),
                updateLayoutOnly(layoutOnly)
            ]).then(function () {
                //init();
                showMessage("success", "Layout has been saved");
            });
        }

        function filterLayout(item) {

            item.placeHolders = _.reject(item.placeHolders, function (item) {
                return (item.layoutTemplate === "content" || item.layoutTemplate === "module");
            })

            _.forEach(item.placeHolders, function (item) {
                if (item.placeHolders) {
                    filterLayout(item);
                }
            });
        }

        function updatePageContents(elementsToSort) {
            var defer = $q.defer();
            var contents = [];
            _.each(elementsToSort.contents, function (item) {
                contents.push({
                    id: item.element.id,
                    pageId: appContext.currentPageId,
                    typeInfo: angular.toJson(item.element),
                    containerId: item.containerId,
                    sortOrder: item.element.sortOrder
                    //cultureCode: appContext.currentCulture //TODO: get this from appContext
                });
            });

            pageContentService.putContents(contents).then(function (response) {
                console.log(response);
                defer.resolve('page content updated');
            }, function (response) {
                console.log(response);
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function updateModules(elementsToSort) {
            var defer = $q.defer();
            var modules = [];
            _.each(elementsToSort.modules, function (item) {
                modules.push({
                    id: item.element.id,
                    pageId: appContext.currentPageId,
                    moduleId: item.element.module.id,
                    containerId: item.containerId,
                    sortOrder: item.element.sortOrder
                    //Modules are not multilingual
                });
            });

            pageModuleService.putModules(modules).then(function (data) {
                console.log(data);
                defer.resolve('page content updated');
            }, function (error) {
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function updateLayoutOnly(layoutOnly) {
            var defer = $q.defer();
            layoutService.put(layoutOnly)
                .then(function (data) {
                    //console.log(data);                    
                    defer.resolve('layout only updated');
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                    defer.reject(SYS_ERROR_MSG);
                });
            return defer.promise;
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
                typeInfo: angular.toJson(item),
                containerId: containerId,
                sortOrder: item.sortOrder,
                cultureCode: appContext.currentCulture //TODO: get this from appContext
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
                sortOrder: item.sortOrder
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

    function editContentCtrl($scope, $uibModalInstance, $q, sdUtil, languageService, pageContentService, contentTranslationService, contentInfo) {
        var vm = this;

        vm.contentId = contentInfo.id;
        vm.changeLanguage = changeLanguage;
        vm.save = save;
        vm.cancel = cancel;
        vm.newItem = newItem;
        vm.updateItem = updateItem;
        vm.editItem = editItem;
        vm.removeItem = removeItem;
        vm.cancelDetailView = cancelDetailView;
        vm.sortableOptions = {
            stop: function (e, ui) {
                _.each(vm.contentTranslation.contentData.items, function (item, index) {
                    item.viewOrder = index + 1;
                });
            }
        };

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        //Event handlers
        function changeLanguage() {
            var translation = getTranslationForLocale(vm.selectedLocale);
            vm.contentTranslation = translation;
            deserializeContentTranslation();
        }

        function save() {
            if (vm.contentTranslation.id) {
                serializeContentTranslation();
                contentTranslationService.put(vm.contentTranslation).then(
                    function (data) {
                        console.log(data);
                        $uibModalInstance.close('ok');
                    }, function (error) {
                        showMessage("error", SYS_ERROR_MSG);
                    });
            }
            else {
                vm.contentTranslation.pageContentId = vm.contentId;
                serializeContentTranslation();
                contentTranslationService.post(vm.contentTranslation).then(
                    function (data) {
                        console.log(data);
                        $uibModalInstance.close('ok');
                    }, function (error) {
                        showMessage("error", SYS_ERROR_MSG);
                    });
            }
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        function newItem() {
            vm.selectedItem = { };
            vm.isDetailView = true;
        }

        function updateItem() {
            if (!vm.selectedItem.id) {
                vm.selectedItem.id = sdUtil.getGuid();
                vm.selectedItem.viewOrder = vm.contentTranslation.contentData.items.length + 1;
                vm.contentTranslation.contentData.items.push(vm.selectedItem);
            }
            vm.isDetailView = false;
        }

        function editItem(item) {
            vm.selectedItem = item;
            vm.isDetailView = true;
        }

        function removeItem(item) {
            var index = vm.contentTranslation.contentData.items.indexOf(slideItem);
            vm.contentTranslation.contentData.items.splice(index, 1);
            vm.isChanged = true;
        }

        function cancelDetailView() {
            vm.isDetailView = false;
        }

        //Private functions
        function init() {
            $q.all([
                getPageContents(),
                getSiteLanguages()
            ]).then(function () {
                vm.selectedLocale = appContext.currentCulture;
                //load correct translation
                var translation = getTranslationForLocale(vm.selectedLocale);
                vm.contentTranslation = translation;
                deserializeContentTranslation();
            });
        }

        function getSiteLanguages() {
            var defer = $q.defer();
            languageService.getSiteLanguages().then(function (languages) {
                vm.languages = languages;
                defer.resolve('data received!');
            }, function (error) {
                showMessage("error", "Cannot get all languages, please contact administrator");
                defer.reject(SYS_ERROR_MSG);
            });
            return defer.promise;
        }

        function getPageContents() {
            var defer = $q.defer();
            pageContentService.get(vm.contentId).then(
                function (pageContent) {
                    console.log(pageContent);
                    vm.contentTranslations = pageContent.pageContentTranslation;
                    //var contentTranslation = _.findWhere(pageContent.pageContentTranslation, { cultureCode: appContext.currentCulture });
                    //if (contentTranslation) {
                    //    vm.contentTranslation = contentTranslation;
                    //}
                    //else {
                    //    vm.contentTranslation = {};
                    //}
                    vm.typeInfo = JSON.parse(pageContent.typeInfo);
                    defer.resolve('data received!');
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                    defer.reject(SYS_ERROR_MSG);
                });
            return defer.promise;
        }

        function getTranslationForLocale(locale) {
            var translation = _.findWhere(vm.contentTranslations, { cultureCode: locale });
            if (!translation) {
                translation = {
                    cultureCode: locale,
                    contentData: {
                        items: []
                    }
                };
            }
            return translation;
        }

        function serializeContentTranslation() {
            if (vm.typeInfo.dataType && (vm.typeInfo.dataType === 'array' || vm.typeInfo.dataType === 'object')) {
                vm.contentTranslation.contentData = angular.toJson(vm.contentTranslation.contentData);
            }
        }

        function deserializeContentTranslation() {
            if (vm.typeInfo.dataType && (vm.typeInfo.dataType === 'array' || vm.typeInfo.dataType === 'object')) {
                vm.contentTranslation.contentData = JSON.parse(vm.contentTranslation.contentData);
            }
        }

    };

}());