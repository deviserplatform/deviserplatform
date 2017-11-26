(function () {

    var app = angular.module('deviserEdit', [
        'ui.router',
        'ui.sortable',
        'ui.bootstrap',
        'ui.select',
        'dndLists',
        'textAngular',
        'sd.sdlib',
        'deviser.services',
        'deviser.config',
        'modules.app.imageManager',
        'modules.app.languageSelector',
        'modules.app.linkSelector'
    ]);

    app.config(['$provide', config]);

    app.directive("sdContentPreview", ['$compile', '$templateCache', sdContentPreviewDir]);

    app.controller('EditCtrl', ['$scope', '$timeout', '$filter', '$q', '$uibModal', 'globals', 'sdUtil', 'layoutService', 'pageService',
        'contentTypeService', 'pageContentService', 'moduleService', 'moduleActionService', 'pageModuleService', editCtrl]);

    app.controller('EditContentCtrl', ['$scope', '$uibModalInstance', '$q', 'sdUtil', 'languageService',
        'pageContentService', 'contentTranslationService', 'contentInfo', editContentCtrl]);

    app.controller('EditModuleCtrl', ['$scope', '$timeout', '$uibModalInstance', '$q', '$sce', 'sdUtil', 'globals', 'moduleActionService', 'moduleInfo', editModuleCtrl]);

    app.controller('ModulePermissionCtrl', ['$scope', '$timeout', '$uibModalInstance', '$q', 'sdUtil', 'globals', 'roleService', 'pageModuleService',
        'pageModule', modulePermissionCtrl]);

    app.controller('ContentPermissionCtrl', ['$scope', '$timeout', '$uibModalInstance', '$q', 'sdUtil', 'globals', 'roleService', 'pageContentService',
        'pageContent', contentPermissionCtrl]);


    ////////////////////////////////
    /*Function declarations only*/
    function config($provide) {
        $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function (taRegisterTool, taOptions) {
            taOptions.toolbar = [
                ['h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'p', 'pre', 'quote'],
                ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear'],
                ['justifyLeft', 'justifyCenter', 'justifyRight', 'indent', 'outdent'],
                ['html', 'wordcount', 'charcount']
            ];
            return taOptions;
        }]);
    }

    function sdContentPreviewDir($compile, $templateCache) {
        var returnObject = {
            restrict: "A",
            controller: ['$scope', '$uibModal', ctrl],
            controllerAs: 'ecVM',
            bindToController: true,
            link: link,
            scope: {
                content: '='
            }
        };

        return returnObject;
        /////////////////////////////////////////////
        /*Function declarations only*/
        function link(scope, element, attrs) {
            template = $templateCache.get('contenttypes/' + scope.ecVM.content.contentType.name + '.html', );
            element.html(template);
            $compile(element.contents())(scope);
        }

        function ctrl($scope, $uibModal) {
            var vm = this;

            init();

            /////////////////////////////////////////////
            /*Function declarations only*/
            function init() {
                vm.templateMode = 'PREVIEW';
                vm.contentType = vm.content.contentType;
                vm.contentTranslations = vm.content.pageContentTranslation;
                var currentCultureCode = pageContext.currentLocale;
                //load correct translation
                var translation = getTranslationForLocale(currentCultureCode);
                vm.contentTranslation = translation;
                deserializeContentTranslation();
            }

            function getTranslationForLocale(locale) {
                var translation = _.find(vm.contentTranslations, { cultureCode: locale });
                if (!translation) {
                    if (vm.contentType.dataType === 'string') {
                        translation = {
                            cultureCode: locale,
                            contentData: ''
                        };
                    }
                    else if (vm.contentType.dataType === 'object') {
                        translation = {
                            cultureCode: locale,
                            contentData: {}
                        };
                    }
                    else {
                        translation = {
                            cultureCode: locale,
                            contentData: {
                                items: []
                            }
                        };
                    }

                }
                return translation;
            }

            function deserializeContentTranslation() {
                if (vm.contentType.dataType &&
                    (vm.contentType.dataType === 'array' || vm.contentType.dataType === 'object') &&
                    typeof (vm.contentTranslation.contentData) === 'string') {
                    vm.contentTranslation.contentData = JSON.parse(vm.contentTranslation.contentData);
                }
            }
        }
    }

    function editCtrl($scope, $timeout, $filter, $q, $uibModal, globals, sdUtil, layoutService, pageService,
        contentTypeService, pageContentService, moduleService, moduleActionService, pageModuleService) {
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
        vm.editModule = editModule;
        vm.openModuleActionEdit = openModuleActionEdit;
        vm.changeModulePermission = changeModulePermission;
        vm.changeContentPermission = changeContentPermission;
        vm.saveProperties = saveProperties;

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/

        //Controller initialization
        function init() {
            vm.uaLayout = {}; //unassigned layout
            var pageLayout = {};

            getCurrentPage().then(function () {
                $q.all([
                    getLayout(),
                    getContentTypes(),
                    getModuleActions(),
                    getPageContents(),
                    getPageModules()
                ]).then(function () {
                    loadPageContents();
                });
            });
        }

        //Event handlers
        $scope.$watch(function () {
            return vm.selectedItem.properties;
        }, function (newValue, oldValue) {
            vm.selectedItem.isPropertyChanged = true;
        }, true);

        function insertedCallback(event, index, item) {
            var parentScope = $(event.currentTarget).scope().$parent;
            var containerId = parentScope.item.id;
            console.log('-----------------------');
            console.log('insertedCallback');
            console.log('-----------------------');

            console.log('updatingElement');
            //Update elements should be called each time even if the item is not new,
            //because when a item moved from one placeholder to another placeholder container should be updated!
            updateElements(parentScope.item);
        }

        function dropCallback(event, index, item) {
            item.sortOrder = index + 1;
            return item;
        }

        function itemMoved(placeHolders, index, event) {
            var parentItem = $(event.currentTarget).closest('.dnd-list').scope().parentItem;
            placeHolders.splice(index, 1);
            updateElements(parentItem);
            console.log('itemMoved');
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
            //contentType.id = vm.newGuid();
        }

        function editContent(content) {
            var defer = $q.defer();
            var modalInstance = $uibModal.open({
                animation: true,
                size: 'lg',
                openedClass: 'edit-content-modal',
                backdrop: 'static',
                templateUrl: 'contenttypes/' + content.contentType.name + '.html',
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

        function editModule(item) {
            vm.selectedItem = item;
            var moduleId;
            if (item.pageModule) {
                moduleId = item.pageModule.moduleId
            }
            else {
                moduleId = item.moduleAction.moduleId;
            }

            moduleActionService.getEditActions(moduleId).then(function (editActions) {
                if (!vm.selectedItem.pageModule) {
                    vm.selectedItem.pageModule = {};
                }
                vm.selectedItem.pageModule.editActions = editActions;
            }, function (response) {
                console.log(response);
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function openModuleActionEdit(moduleAction) {
            var defer = $q.defer();
            var modalInstance = $uibModal.open({
                animation: true,
                size: 'lg',
                openedClass: 'edit-content-modal',
                backdrop: 'static',
                templateUrl: 'app/components/editModule.tpl.html',
                controller: 'EditModuleCtrl as emVM',
                resolve: {
                    moduleInfo: function () {
                        var returnObject = {
                            moduleActionId: moduleAction.id,
                            pageModuleId: vm.selectedItem.id
                        };
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

        function changeModulePermission(pageModule) {
            var defer = $q.defer();
            var modalInstance = $uibModal.open({
                animation: true,
                size: 'lg',
                backdrop: 'static',
                templateUrl: 'app/components/permissionEditor.tpl.html',
                controller: 'ModulePermissionCtrl as peVM',
                resolve: {
                    pageModule: function () {
                        var returnObject = pageModule;
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

        function changeContentPermission(pageContent) {
            var defer = $q.defer();
            var modalInstance = $uibModal.open({
                animation: true,
                size: 'lg',
                backdrop: 'static',
                templateUrl: 'app/components/permissionEditor.tpl.html',
                controller: 'ContentPermissionCtrl as peVM',
                resolve: {
                    pageContent: function () {
                        var returnObject = pageContent;
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

        function saveProperties() {
            //It Saves content with properties
            if (vm.selectedItem.layoutTemplate === "content") {

                //Prepare content to update
                var pageContent = {};//vm.selectedItem.pageContent;

                var properties = [];
                _.forEach(vm.selectedItem.properties, function (srcProp) {
                    if (srcProp) {
                        var prop = {
                            id: srcProp.id,
                            name: srcProp.name,
                            label: srcProp.label,
                            value: srcProp.value
                        }
                        properties.push(prop);
                    }
                });
                pageContent.id = vm.selectedItem.pageContent.id;
                pageContent.pageId = vm.selectedItem.pageContent.pageId;
                pageContent.title = vm.selectedItem.title;
                pageContent.contentTypeId = vm.selectedItem.pageContent.contentTypeId;
                pageContent.properties = properties;
                pageContent.containerId = vm.selectedItem.pageContent.containerId;
                pageContent.sortOrder = vm.selectedItem.sortOrder;

                pageContentService.put(pageContent).then(function (response) {
                    console.log(response);
                    vm.selectedItem.isPropertyChanged = false;
                }, function (response) {
                    console.log(response);
                }, true);
            }
            else if (vm.selectedItem.layoutTemplate === "module") {
                var pageModule = vm.selectedItem.pageModule;
                                
                pageModule.title = vm.selectedItem.title;
                pageModule.containerId = vm.selectedItem.pageModule.containerId;                
                pageModule.sortOrder = vm.selectedItem.sortOrder;

                pageModuleService.put(pageModule).then(function (response) {
                    console.log(response);
                }, function (response) {
                    console.log(response);
                }, true);
            }
        }

        /*Private functions*/
        function getCurrentPage() {
            var defer = $q.defer();
            pageService.get(pageContext.currentPageId)
                .then(function (data) {
                    vm.currentPage = data;
                    defer.resolve(data);
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                    defer.reject(SYS_ERROR_MSG);
                });
            return defer.promise;
        }

        function getPageModules() {
            var defer = $q.defer();
            pageModuleService.getByPage(pageContext.currentPageId)
                .then(function (data) {
                    vm.pageModules = data;
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
                    var contentTypes = data;
                    vm.contentTypes = [];
                    _.forEach(contentTypes, function (contentType) {
                        vm.contentTypes.push({
                            layoutTemplate: "content",
                            type: "content",
                            contentType: contentType,
                            properties: contentType.properties
                        });
                    });
                    defer.resolve('data received!');
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                    defer.reject(SYS_ERROR_MSG);
                });
            return defer.promise;
        }

        function getModuleActions() {
            var defer = $q.defer();
            moduleActionService.get()
                .then(function (data) {
                    var moduleActions = data;
                    vm.modules = [];
                    _.forEach(moduleActions, function (moduleAction) {
                        vm.modules.push({
                            layoutTemplate: "module",
                            type: "module",
                            moduleAction: moduleAction
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
            pageContentService.get(pageContext.currentPageId, pageContext.currentLocale)
                .then(function (pageContents) {
                    vm.pageContents = pageContents;
                    defer.resolve(pageContents);
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                    defer.reject(SYS_ERROR_MSG);
                });
            return defer.promise;
        }

        function loadPageContents() {

            var unAssignedContents = [],
                unAssignedModules = [];

            //First, position elements in correct order and then assign the pageLayout to VM.
            positionPageContents(pageLayout.placeHolders);
            vm.pageLayout = pageLayout;
            vm.pageLayout.pageId = vm.currentPage.id;

            var unAssignedSrcConents = _.reject(vm.pageContents, function (content) {
                return _.includes(containerIds, content.containerId);
            });

            var unAssignedSrcModules = _.reject(vm.pageModules, function (module) {
                return _.includes(containerIds, module.containerId);
            });

            _.forEach(unAssignedSrcConents, function (pageContent) {
                var content = getPageContentWithProperties(pageContent);
                content.isUnassigned = true;
                unAssignedContents.push(content);
            });

            _.forEach(unAssignedSrcModules, function (pageModule) {
                var index = pageModule.sortOrder - 1;
                var module = {
                    id: pageModule.id,
                    layoutTemplate: "module",
                    type: "module",
                    moduleAction: pageModule.moduleAction,
                    pageModule: pageModule,
                    sortOrder: pageModule.sortOrder,
                    isUnassigned: true
                };//JSON.parse(pageModule.module);
                unAssignedModules.push(module);
            })

            vm.uaLayout.placeHolders = [];

            vm.uaLayout.placeHolders = vm.uaLayout.placeHolders.concat(unAssignedContents);
            vm.uaLayout.placeHolders = vm.uaLayout.placeHolders.concat(unAssignedModules);

        }

        function positionPageContents(placeHolders) {
            if (placeHolders) {
                _.forEach(placeHolders, function (item) {
                    //console.log(item)

                    //adding containerId to filter unallocated items in a separate dndlist
                    containerIds.push(item.id);

                    //Load content items if found
                    var pageContents = _.filter(vm.pageContents, { containerId: item.id });
                    if (pageContents) {
                        _.forEach(pageContents, function (pageContent) {
                            var content = getPageContentWithProperties(pageContent);
                            //item.placeHolders.splice(index, 0, contentTypeInfo); //Insert placeHolder into specified index
                            if (!content.title) {
                                content.title = content.contentType.label + ' ' + item.placeHolders.length + 1;
                            }
                            item.placeHolders.push(content);
                        });
                    }

                    //Load modules if found
                    var pageModules = _.filter(vm.pageModules, { containerId: item.id });
                    if (pageModules) {
                        _.forEach(pageModules, function (pageModule) {
                            var index = pageModule.sortOrder - 1;
                            var module = {
                                id: pageModule.id,
                                layoutTemplate: "module",
                                type: "module",
                                title: pageModule.title,
                                moduleAction: pageModule.moduleAction,
                                pageModule: pageModule,
                                sortOrder: pageModule.sortOrder
                            };//JSON.parse(pageModule.module);
                            //item.placeHolders.splice(index, 0, module); //Insert placeHolder into specified index

                            if (!module.title) {
                                module.title = module.moduleAction.displayName + ' ' + item.placeHolders.length + 1;
                            }

                            item.placeHolders.push(module);
                        })
                    }

                    item.placeHolders = _.sortBy(item.placeHolders, ['sortOrder']);

                    if (item.placeHolders) {
                        positionPageContents(item.placeHolders);
                    }
                });
            }
        }

        function getPageContentWithProperties(pageContent) {
            var propertiesValue = pageContent.properties;
            var masterContentType = _.find(vm.contentTypes, function (element) {
                return element.contentType.id === pageContent.contentType.id;
            });
            var properties = angular.copy(masterContentType.properties);

            //Loading values to the properties
            _.forEach(properties, function (prop) {
                var propVal = _.find(propertiesValue, { id: prop.id });
                if (propVal) {
                    prop.value = propVal.value;
                }
                if (prop.optionList && prop.optionList.list) {
                    prop.optionList = angular.fromJson(prop.optionList.list);
                }
            });

            var content = {
                id: pageContent.id,
                layoutTemplate: 'content',
                type: 'content',
                title: pageContent.title,
                properties: properties,
                pageContent: pageContent,
                contentType: pageContent.contentType,
                sortOrder: pageContent.sortOrder
            }

            return content;
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
            _.forEach(elementsToSort.contents, function (item) {
                var pageContent = {};

                if (item.element.id) {
                    pageContent.id = item.element.pageContent.id;
                    pageContent.pageId = item.element.pageContent.pageId;
                    pageContent.contentTypeId = item.element.pageContent.contentTypeId;
                }
                else {
                    //New page content

                    pageContent.id = vm.newGuid(); //Id shoud be generated only on client side
                    pageContent.pageId = pageContext.currentPageId;
                    pageContent.contentTypeId = item.element.contentType.id;
                    pageContent.hasEditPermission = true; //New content always has edit permission
                    item.element.id = pageContent.id;
                    item.element.pageContent = pageContent;

                }

                var properties = [];
                _.forEach(item.element.properties, function (srcProp) {
                    if (srcProp) {
                        var prop = {
                            id: srcProp.id,
                            name: srcProp.name,
                            label: srcProp.label,
                            value: srcProp.value
                        }
                        properties.push(prop);
                    }
                });
                pageContent.properties = properties;
                pageContent.containerId = item.containerId;
                pageContent.sortOrder = item.element.sortOrder;



                contents.push(pageContent);
            });

            //Updating only page contents, not content translations 
            if (contents && contents.length > 0) {
                pageContentService.putContents(contents).then(function (response) {
                    console.log(response);
                    defer.resolve('page content updated');
                }, function (response) {
                    console.log(response);
                    defer.reject(SYS_ERROR_MSG);
                });
            }
            else {
                defer.reject(SYS_ERROR_MSG);
            }

            return defer.promise;
        }

        function updateModules(elementsToSort) {
            var defer = $q.defer();
            var modules = []; //pageModules
            _.forEach(elementsToSort.modules, function (item) {
                var module = {
                    pageId: pageContext.currentPageId,
                    containerId: item.containerId,
                    sortOrder: item.element.sortOrder
                };
                if (item.element.id) {
                    //Update
                    module.id = item.element.id;
                }
                else {
                    //New element
                    module.id = vm.newGuid();
                    item.element.id = module.id;
                }

                if (item.element.pageModule) {
                    //pagemodule from db
                    module.moduleId = item.element.pageModule.module.id;
                    module.moduleActionId = item.element.pageModule.moduleActionId;
                }
                else {
                    //newely created module                    
                    module.moduleId = item.element.moduleAction.moduleId;
                    module.moduleActionId = item.element.moduleAction.id;
                    module.hasEditPermission = true; //New content always has edit permission
                    item.element.pageModule = module;
                }

                modules.push(module);

            });

            if (modules && modules.length > 0) {
                pageModuleService.putModules(modules).then(function (data) {
                    console.log(data);
                    defer.resolve('page content updated');
                }, function (error) {
                    defer.reject(SYS_ERROR_MSG);
                });
            }
            else {
                defer.reject(SYS_ERROR_MSG);
            }

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
        vm.properties = contentInfo.properties;
        vm.viewStates = {
            NEW: "NEW",
            EDIT: "EDIT",
            LIST: "LIST"
        };
        vm.currentViewState = vm.viewStates.LIST;

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
                _.forEach(vm.contentTranslation.contentData.items, function (item, index) {
                    item.viewOrder = index + 1;
                });
            }
        };

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            vm.templateMode = 'EDITCONTENT';
            $q.all([
                getPageContents(),
                getSiteLanguages()
            ]).then(function () {
                var currentCultureCode = pageContext.currentLocale;
                vm.selectedLocale = _.find(vm.languages, { cultureCode: currentCultureCode });
                //load correct translation
                var translation = getTranslationForLocale(vm.selectedLocale.cultureCode);
                vm.contentTranslation = translation;
                deserializeContentTranslation();

            });
        }

        //Event handlers
        function changeLanguage() {
            var translation = getTranslationForLocale(vm.selectedLocale.cultureCode);
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
            vm.selectedItem = {};
            vm.currentViewState = vm.viewStates.NEW;
        }

        function updateItem() {
            if (!vm.selectedItem.id) {
                vm.selectedItem.id = sdUtil.getGuid();
                vm.selectedItem.viewOrder = vm.contentTranslation.contentData.items.length + 1;
                vm.contentTranslation.contentData.items.push(vm.selectedItem);
            }
            vm.currentViewState = vm.viewStates.LIST;
        }

        function editItem(item) {
            vm.selectedItem = item;
            vm.currentViewState = vm.viewStates.EDIT;
        }

        function removeItem(item) {
            var index = vm.contentTranslation.contentData.items.indexOf(slideItem);
            vm.contentTranslation.contentData.items.splice(index, 1);
            vm.isChanged = true;
        }

        function cancelDetailView() {
            vm.currentViewState = vm.viewStates.LIST;
        }

        //Private functions        
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
                    //var contentTranslation = _.find(pageContent.pageContentTranslation, { cultureCode: pageContext.currentLocale });
                    //if (contentTranslation) {
                    //    vm.contentTranslation = contentTranslation;
                    //}
                    //else {
                    //    vm.contentTranslation = {};
                    //}
                    vm.contentType = pageContent.contentType;
                    defer.resolve('data received!');
                }, function (error) {
                    showMessage("error", SYS_ERROR_MSG);
                    defer.reject(SYS_ERROR_MSG);
                });
            return defer.promise;
        }

        function getTranslationForLocale(locale) {
            var translation = _.find(vm.contentTranslations, { cultureCode: locale });
            if (!translation) {
                if (vm.contentType.dataType === 'string') {
                    translation = {
                        cultureCode: locale,
                        contentData: ''
                    };
                }
                else if (vm.contentType.dataType === 'object') {
                    translation = {
                        cultureCode: locale,
                        contentData: {}
                    };
                }
                else {
                    translation = {
                        cultureCode: locale,
                        contentData: {
                            items: []
                        }
                    };
                }

            }
            return translation;
        }

        function serializeContentTranslation() {
            if (vm.contentType.dataType && (vm.contentType.dataType === 'array' || vm.contentType.dataType === 'object')) {
                vm.contentTranslation.contentData = angular.toJson(vm.contentTranslation.contentData);
            }
        }

        function deserializeContentTranslation() {
            if (vm.contentType.dataType &&
                (vm.contentType.dataType === 'array' || vm.contentType.dataType === 'object') &&
                typeof (vm.contentTranslation.contentData) === 'string') {
                vm.contentTranslation.contentData = JSON.parse(vm.contentTranslation.contentData);
            }
        }

    };

    function editModuleCtrl($scope, $timeout, $uibModalInstance, $q, $sce, sdUtil, globals, moduleActionService, moduleInfo) {
        var vm = this;
        vm.pageModuleId = moduleInfo.pageModuleId;
        vm.moduleActionId = moduleInfo.moduleActionId;

        vm.cancel = cancel;

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            getModuleActionContent();
        }

        //Event handlers 
        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        function getModuleActionContent() {
            moduleActionService.getEditActionView(pageContext.currentUrl, vm.pageModuleId, vm.moduleActionId).then(function (response) {
                console.log(response);
                vm.editView = $sce.trustAsHtml(response);
            }, function (response) {
                console.log(response);
            });
        }
    }

    function modulePermissionCtrl($scope, $timeout, $uibModalInstance, $q, sdUtil, globals, roleService, pageModuleService, pageModule) {
        var vm = this;
        var moduleViewPermissionId = globals.appSettings.permissions.moduleView;
        var moduleEditPermissionId = globals.appSettings.permissions.moduleEdit;
        var administratorRoleId = globals.appSettings.roles.administrator;
        var pageModuleInfo = pageModule; //In order to re-use permission template for both module and content

        vm.label = {
            title: 'Module Permission',
            tableRowTitleView: 'View Module',
            tableRowTitleEdit: 'Edit Module'
        }
        vm.administratorRoleId = administratorRoleId;

        /*Event handler bindings*/
        vm.isView = isView;
        vm.isEdit = isEdit;
        vm.changeViewPermission = changeViewPermission;
        vm.changeEditPermission = changeEditPermission;
        vm.save = save;
        vm.cancel = cancel;

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            getRoles();
            getPageModule();
        }

        //Event handlers
        function isView(role) {
            if (vm.pageModule) {
                var permission = _.find(vm.pageModule.modulePermissions, function (p) {
                    return p.roleId === role.id && p.permissionId === moduleViewPermissionId
                });
                return permission;
            }
            return false;
        }

        function isEdit(role) {
            if (vm.pageModule) {
                var permission = _.find(vm.pageModule.modulePermissions, function (p) {
                    return p.roleId === role.id && p.permissionId === moduleEditPermissionId;
                });
                return permission;
            }
            return false;
        }

        function changeViewPermission(role) {
            if (role.id !== administratorRoleId) {
                if (isView(role)) {
                    //Remove
                    vm.pageModule.modulePermissions = _.reject(vm.pageModule.modulePermissions, function (p) {
                        return p.roleId === role.id && p.permissionId === moduleViewPermissionId
                    })
                }
                else {
                    //Add
                    var permission = {
                        pageModuleId: vm.pageModule.id,
                        roleId: role.id,
                        permissionId: moduleViewPermissionId
                    };
                    vm.pageModule.modulePermissions.push(permission);
                }
            }
        }

        function changeEditPermission(role) {
            if (role.id !== administratorRoleId) {
                if (isEdit(role)) {
                    //Remove
                    vm.pageModule.modulePermissions = _.reject(vm.pageModule.modulePermissions, function (p) {
                        return p.roleId === role.id && p.permissionId === moduleEditPermissionId
                    })
                }
                else {
                    //Add
                    var permission = {
                        pageModuleId: vm.pageModule.id,
                        roleId: role.id,
                        permissionId: moduleEditPermissionId
                    };
                    vm.pageModule.modulePermissions.push(permission);
                }
            }
        }

        function save() {
            var pageModule = {
                id: vm.pageModule.id,
                pageId: vm.pageModule.pageId,
                inheritViewPermissions: vm.inheritViewPermissions,
                inheritEditPermissions: vm.inheritEditPermissions,
                modulePermissions: vm.pageModule.modulePermissions
            }
            pageModuleService.putPermission(pageModule).then(function (data) {
                console.log(data);
                $uibModalInstance.dismiss('cancel'); //Everything ok, close the modal dialog
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        /*Private functions*/
        function getRoles() {
            roleService.get().then(function (roles) {
                vm.roles = roles;
            }, function (error) {
                showMessage("error", "Cannot get all roles, please contact administrator");
            });
        }

        function getPageModule() {
            pageModuleService.get(pageModuleInfo.id).then(function (pageModule) {
                vm.pageModule = pageModule;
                vm.inheritViewPermissions = vm.pageModule.inheritViewPermissions;
                vm.inheritEditPermissions = vm.pageModule.inheritEditPermissions;
            }, function (error) {
                showMessage("error", "Cannot get all roles, please contact administrator");
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

    function contentPermissionCtrl($scope, $timeout, $uibModalInstance, $q, sdUtil, globals, roleService, pageContentService, pageContent) {
        var vm = this;
        var contentViewPermissionId = globals.appSettings.permissions.contentView;
        var contentEditPermissionId = globals.appSettings.permissions.contentEdit;
        var administratorRoleId = globals.appSettings.roles.administrator;
        var pageContentInfo = pageContent; //In order to re-use permission template for both module and content

        vm.label = {
            title: 'Content Permission',
            tableRowTitleView: 'View Content',
            tableRowTitleEdit: 'Edit Content'
        }
        vm.administratorRoleId = administratorRoleId;

        /*Event handler bindings*/
        vm.isView = isView;
        vm.isEdit = isEdit;
        vm.changeViewPermission = changeViewPermission;
        vm.changeEditPermission = changeEditPermission;
        vm.save = save;
        vm.cancel = cancel;

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            getRoles();
            getPageContent();
        }

        //Event handlers
        function isView(role) {
            if (vm.pageContent) {
                var permission = _.find(vm.pageContent.contentPermissions, function (p) {
                    return p.roleId === role.id && p.permissionId === contentViewPermissionId
                });
                return permission;
            }
            return false;
        }

        function isEdit(role) {
            if (vm.pageContent) {
                var permission = _.find(vm.pageContent.contentPermissions, function (p) {
                    return p.roleId === role.id && p.permissionId === contentEditPermissionId;
                });
                return permission;
            }
            return false;
        }

        function changeViewPermission(role) {
            if (role.id !== administratorRoleId) {
                if (isView(role)) {
                    //Remove
                    vm.pageContent.contentPermissions = _.reject(vm.pageContent.contentPermissions, function (p) {
                        return p.roleId === role.id && p.permissionId === contentViewPermissionId
                    })
                }
                else {
                    //Add
                    var permission = {
                        pageContentId: vm.pageContent.id,
                        roleId: role.id,
                        permissionId: contentViewPermissionId
                    };
                    vm.pageContent.contentPermissions.push(permission);
                }
            }
        }

        function changeEditPermission(role) {
            if (role.id !== administratorRoleId) {
                if (isEdit(role)) {
                    //Remove
                    vm.pageContent.contentPermissions = _.reject(vm.pageContent.contentPermissions, function (p) {
                        return p.roleId === role.id && p.permissionId === contentEditPermissionId
                    })
                }
                else {
                    //Add
                    var permission = {
                        pageContentId: vm.pageContent.id,
                        roleId: role.id,
                        permissionId: contentEditPermissionId
                    };
                    vm.pageContent.contentPermissions.push(permission);
                }
            }
        }

        function save() {
            var pageContent = {
                id: vm.pageContent.id,
                pageId: vm.pageContent.pageId,
                inheritViewPermissions: vm.inheritViewPermissions,
                inheritEditPermissions: vm.inheritEditPermissions,
                contentPermissions: vm.pageContent.contentPermissions
            }
            pageContentService.putPermission(pageContent).then(function (data) {
                console.log(data);
                $uibModalInstance.dismiss('cancel'); //Everything ok, close the modal dialog
            }, function (error) {
                showMessage("error", SYS_ERROR_MSG);
            });
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        /*Private functions*/
        function getRoles() {
            roleService.get().then(function (roles) {
                vm.roles = roles;
            }, function (error) {
                showMessage("error", "Cannot get all roles, please contact administrator");
            });
        }

        function getPageContent() {
            pageContentService.get(pageContentInfo.id).then(function (pageContent) {
                vm.pageContent = pageContent;
                vm.inheritViewPermissions = vm.pageContent.inheritViewPermissions;
                vm.inheritEditPermissions = vm.pageContent.inheritEditPermissions;
            }, function (error) {
                showMessage("error", "Cannot get all roles, please contact administrator");
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

}());