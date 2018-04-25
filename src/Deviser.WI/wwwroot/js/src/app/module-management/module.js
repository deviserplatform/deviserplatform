(function () {

    var app = angular.module('deviser.module', [
        'ui.router',
        'ui.bootstrap',
        'ui.tree',
        'ui.select',
        'sd.sdlib',
        'deviser.services',
        'deviser.config'        
    ]);

    app.controller('ModuleManagementCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'moduleService', 'moduleActionService', 'propertyService', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, moduleService, moduleActionService, propertyService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];

        /*Function bindings*/
        vm.add = add;
        vm.edit = edit;
         vm.activate = activate;
        vm.isValidName = isValidName;
        vm.remove = remove;
        vm.save = save;
        vm.cancel = cancel;
        //vm.hasError = hasError;
        vm.addModuleActions = addModuleActions;
        vm.editModuleActions = editModuleActions;
        vm.removeModuleActions = removeModuleActions;
        vm.saveModuleAction = saveModuleAction;
        vm.addProperty = addProperty;
        vm.isPropExist = isPropExist;
        vm.removeProperty = removeProperty;
        vm.propertyTransform = propertyTransform;
        vm.viewStates = {
            NEW: "NEW",
            LIST: "LIST",
            EDIT: "EDIT",
            NEWMODULEACTION: "NEWMODULEACTION",
            LISTMODULEACTION: "LISTMODULEACTION",
            EDITMODULEACTION: "EDITMODULEACTION"
        };
        vm.currentViewState = vm.viewStates.LIST;

        init();
        function init() {
            vm.modules = {};
            vm.moduleActions = {};
            getModules();
            getModuleActions();
            getModuleActionTypes();
            getProperties();
        }


        //////////////////////////////////
        ///*Function declarations only*/       
        function getModules() {
            moduleService.get().then(function (modules) {
                vm.modules = modules;
                
            }, function (error) {
                showMessage("error", "Cannot get all modules, please contact administrator");
            });
        }


        function getModuleActions() {
            moduleActionService.get().then(function (moduleActions) {
                vm.moduleActions = moduleActions;
            }, function (error) {
                showMessage("error", "Cannot get all module actions, please contact administrator");
            });
        }

        function getModuleActionTypes() {
            moduleService.getModuleActionTypes().then(function (moduleActionType) {
                vm.moduleActionTypes = moduleActionType;
            }, function (error) {
                showMessage("error", "Cannot get all module action types, please contact administrator");
            });
        }

        function getProperties() {
            propertyService.get().then(function (properties) {
                vm.properties = properties;
            }, function (error) {
                showMessage("error", "Cannot get all properties, please contact administrator");
            });
        }

        function activate(module) {
            update(module);
        }

        function add() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedModule = {};

        }

        function edit(module) {
            $scope.editModuleForm.submitted = false;
            $scope.newModuleForm.submitted = false;
            vm.currentViewState = vm.viewStates.EDIT;
            vm.selectedModule = module;
        }

        function isValidName(name) {
            return !_.find(vm.modules, { name: name });
        }

        function remove(module) {
            moduleService.remove(module.id).then(function (result) {
                console.log(result);
                getModules();
                vm.currentViewState = vm.viewStates.LIST;
                showMessage("success", "Module has been removed");
            }, function (error) {
                showMessage("error", "Cannot remove Module, please contact administrator");
            });
        }

        function save() {
            $scope.newModuleForm.submitted = true;
            if ($scope.newModuleForm.$valid) {
                $scope.newModuleForm.submitted = false;
                if (vm.currentViewState == vm.viewStates.NEW && vm.isValidName(vm.selectedModule.name)) {
                    moduleService.post(vm.selectedModule).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.selectedModule = {};
                        getModules();
                        showMessage("success", "New module has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add module, please contact administrator");
                    });
                }
                else {
                    update(vm.selectedModule);
                }   
            }
        }
        
        function update(module) {
            moduleService.put(module, module.id).then(function (result) {
                console.log(result);
                getModules();
                showMessage("success", "Modulehas been updated");
                vm.currentViewState = vm.viewStates.LIST;
            }, function (error) {
                showMessage("error", "Cannot update module, please contact administrator");
            });
        }

        function addModuleActions() {
            vm.currentViewState = vm.viewStates.NEWMODULEACTION;
            var moduleAction = {
                displayName: ''
            }
            if (!vm.selectedModule.moduleAction) {
                vm.selectedModule.moduleAction = [];
            }
            vm.selectedModule.moduleAction.push(moduleAction);
            vm.selectedModuleAction = moduleAction;
        }

        function editModuleActions(moduleAction) {
            vm.currentViewState = vm.viewStates.EDITMODULEACTION;
            vm.selectedModuleAction = moduleAction;
        }



        function removeModuleActions(moduleAction) {
            var index = vm.selectedModule.moduleAction.indexOf(moduleAction);
            vm.selectedModule.moduleAction.splice(index, 1);
        }

        function saveModuleAction() {
            $scope.editModuleForm.submitted = true;
            if ($scope.editModuleForm.$valid) {
                $scope.editModuleForm.submitted = false;
                vm.currentViewState = vm.viewStates.LISTMODULEACTION;
            }               
        }

        function addProperty() {
            if (!isPropExist()) {
                if (!vm.selectedModuleAction.properties) {
                    vm.selectedModuleAction.properties = [];
                }

                if (vm.selectedProperty.id) {
                    //Add existing property
                    vm.selectedModuleAction.properties.push(vm.selectedProperty)
                }
                else {
                    //Add new property to service and then add it to selected content type
                    propertyService.post(vm.selectedProperty).then(function (property) {
                        console.log(property);
                        vm.selectedProperty = property;
                        vm.selectedModuleAction.properties.push(vm.selectedProperty)
                        getProperties();
                        showMessage("success", "New property has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add new property, please contact administrator");
                    });
                }
            }
        }


        function isPropExist() {
            var isExist = _.find(vm.selectedModuleAction.properties, { id: vm.selectedProperty.id });
            return isExist;
        }


        function removeProperty(property) {
            vm.selectedModuleAction.properties = _.reject(vm.selectedModuleAction.properties, function (prop) {
                return prop.id === property.id;
            })
        }

        function propertyTransform(propertyLabel) {
            var propertyName = propertyLabel.replace(/\s+/g, '');
            propertyName = propertyName.toLowerCase();
            var item = {
                name: propertyName,
                label: propertyLabel
            };
            return item;
        }


        //function hasError(form, field, validation) {
        //    if (form && validation) {
        //        return (form[field].$dirty && form[field].$error[validation]) || (form.submitted && form[field].$error[validation]);
        //    }
        //    return (form[field].$dirty && form[field].$invalid) || (form.submitted && form[field].$invalid);
        //}

        function cancel() {
            vm.currentViewState = vm.viewStates.LIST;
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