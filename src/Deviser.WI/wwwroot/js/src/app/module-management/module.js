(function () {

    var app = angular.module('deviser.module', [
        'ui.router',
        'ui.bootstrap',
        'ui.tree',
        'sd.sdlib',
        'deviser.services',
        'deviser.config'
    ]);

    app.controller('ModuleManagementCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'moduleService', 'moduleActionService', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, moduleService, moduleActionService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];

        /*Function bindings*/
        vm.add = add;
        vm.edit = edit;
        vm.isValidName = isValidName;
        vm.remove = remove;
        vm.save = save;
        vm.cancel = cancel;
        vm.hasError = hasError;
        vm.addModuleActions = addModuleActions;
        vm.editModuleActions = editModuleActions;
        vm.removeModuleActions = removeModuleActions;
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
            vm.module = {};
            vm.moduleAction = {};
            getModules();
            getModuleActions();
        }


        //////////////////////////////////
        ///*Function declarations only*/       
        function getModules() {
            moduleService.get().then(function (modules) {
                vm.modules = modules;
                vm.module = _.keyBy(modules, "name");
                vm.module.isActive = (vm.module.isActive === "true") ? true : false;
            }, function (error) {
                showMessage("error", "Cannot get all modules, please contact administrator");
            });
        }


        function getModuleActions() {
            moduleActionService.get().then(function (moduleActions) {
                vm.moduleActions = moduleActions;
                vm.moduleAction = _.keyBy(moduleActions, "name");
                vm.moduleAction.isDefault = (vm.moduleAction.isDefault === "true") ? true : false;
            }, function (error) {
                showMessage("error", "Cannot get all module actions, please contact administrator");
            });
        }


        function add() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedModule = {};

        }

        function edit(module) {
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
            $scope.moduleForm.submitted = true;
            if ($scope.moduleForm.$valid) {
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
        //if (vm.currentViewState == vm.viewStates.EDIT) {
        //    if ($scope.editModuleForm.$valid) {
        //        moduleService.put(vm.selectedModule).then(function (result) {
        //            console.log(result);
        //            getModules();
        //            vm.currentViewState = vm.viewStates.LIST;
        //            showMessage("success", "Module has been updated");
        //        }, function (error) {
        //            showMessage("error", "Cannot update module, please contact administrator");
        //        });
        //    }

        //}
        //else if (vm.currentViewState == vm.viewStates.NEW) {
        //    if ($scope.moduleForm.$valid) {
        //        moduleService.post(vm.newModule).then(function (result) {
        //            console.log(result);
        //            vm.currentViewState = vm.viewStates.LIST;
        //            vm.newModule = {};
        //            getModules();
        //            showMessage("success", "New module has been created");
        //        }, function (error) {
        //            showMessage("error", "Cannot create module, please contact administrator");
        //        });
        //    }

        //}
        function addModuleActions() {
            vm.currentViewState = vm.viewStates.NEWMODULEACTION;
            var actionItem = {
                displayName: ''
            }
            if (!vm.selectedModule.moduleAction) {
                vm.selectedModule.moduleAction = [];
            }
            vm.selectedModule.moduleAction.push(actionItem)
        }

        function editModuleActions(moduleAction) {
            vm.currentViewState = vm.viewStates.EDITMODULEACTION;
            vm.selectedModuleAction = moduleAction;
        }
        


        function removeModuleActions(moduleAction) {

            vm.selectedModule.moduleAction 
            //moduleActionService.removeModuleActions(moduleAction.id).then(function (result) {
            //    console.log(result);
            //    getModuleActions();
            //    vm.currentViewState = vm.viewStates.LISTMODULEACTION;
            //    showMessage("success", "Module action has been removed");
            //}, function (error) {
            //    showMessage("error", "Cannot remove Module action, please contact administrator");
            //});
        }


        function hasError(form, field, validation) {
            if (form && validation) {
                return (form[field].$dirty && form[field].$error[validation]) || (form.submitted && form[field].$error[validation]);
            }
            return (form[field].$dirty && form[field].$invalid) || (form.submitted && form[field].$invalid);
        }

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