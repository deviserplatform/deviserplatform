﻿(function () {

    var app = angular.module('deviser.layoutTypes', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'dev.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('LayoutTypesCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals',
        'layoutTypeService', 'propertyService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals,
        layoutTypeService, propertyService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];       
        vm.viewStates = {
            NEW: "NEW",
            EDIT: "EDIT",
            LIST: "LIST"
        };
        vm.currentViewState = vm.viewStates.LIST;

        /*Function bindings*/
        vm.newContentType = newContentType;
        vm.edit = edit;
        vm.isValidName = isValidName;
        vm.save = save;
        vm.activate = activate;
        vm.cancel = cancel;
        vm.hasError = hasError;
        vm.propertyTransform = propertyTransform;
        vm.addProperty = addProperty;
        vm.removeProperty = removeProperty;
        vm.isPropExist = isPropExist;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            getLayoutTypes();
            getProperties();
        }

        /*Event handlers*/

        function newContentType() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedLayoutType = {};
        }

        function edit(layout) {
            vm.currentViewState = vm.viewStates.EDIT;
            vm.selectedLayoutType = layout;

            if (vm.selectedLayoutType.layoutTypeIds) {
                var allowedTypeIds = vm.selectedLayoutType.layoutTypeIds.replace(/\s+/g, '').split(',');
                vm.selectedLayoutType.allowedLayoutTypes = allowedTypeIds;
            }
        }

        function isValidName(name) {
            return !_.find(vm.layoutTypes, { name: name });
        }

        function save() {
            $scope.layoutForm.submitted = true;
            if ($scope.layoutForm.$valid) {
                if (vm.currentViewState === vm.viewStates.NEW && vm.isValidName(vm.selectedLayoutType.name)) {
                    layoutTypeService.post(vm.selectedLayoutType).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.selectedLayoutType = {};
                        getLayoutTypes();
                        showMessage("success", "New content type has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add layout type, please contact administrator");
                    });
                }
                else {                    
                    vm.selectedLayoutType.layoutTypeIds = vm.selectedLayoutType.allowedLayoutTypes.join(',');
                    delete vm.selectedLayoutType.allowedLayoutTypes;
                    update(vm.selectedLayoutType);
                }
            }
        }

        function activate(layoutType) {
            update(layoutType);
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

        function propertyTransform(propertyLabel) {
            var propertyName = propertyLabel.replace(/\s+/g, '');
            propertyName = propertyName.toLowerCase();
            var item = {
                name: propertyName,
                label: propertyLabel
            };
            return item;
        }

        function addProperty() {
            if (!isPropExist()) {
                if (!vm.selectedLayoutType.properties) {
                    vm.selectedLayoutType.properties = [];
                }

                if (vm.selectedProperty.id) {
                    //Add existing property
                    vm.selectedLayoutType.properties.push(vm.selectedProperty)
                }
                else {
                    //Add new property to service and then add it to selected content type
                    propertyService.post(vm.selectedProperty).then(function (property) {
                        console.log(property);
                        vm.selectedProperty = property;
                        vm.selectedLayoutType.properties.push(vm.selectedProperty)
                        getProperties();
                        showMessage("success", "New property has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add new property, please contact administrator");
                    });
                }
            }
        }

        function removeProperty(property) {
            vm.selectedLayoutType.properties = _.reject(vm.selectedLayoutType.properties, function (prop) {
                return prop.id === property.id;
            })
        }

        function isPropExist() {
            var isExist = _.find(vm.selectedLayoutType.properties, { id: vm.selectedProperty.id });
            return isExist;
        }

        /*Private functions*/
        function getLayoutTypes() {
            layoutTypeService.get().then(function (layoutTypes) {
                vm.layoutTypes = layoutTypes;
            }, function (error) {
                showMessage("error", "Cannot get all layout types, please contact administrator");
            });
        }

        function getProperties() {
            propertyService.get().then(function (properties) {
                vm.properties = properties;
            }, function (error) {
                showMessage("error", "Cannot get all properties, please contact administrator");
            });
        }

        function update(layoutType) {            
            layoutTypeService.put(layoutType).then(function (result) {
                console.log(result);
                getLayoutTypes();
                showMessage("success", "Layout type has been updated");
                vm.currentViewState = vm.viewStates.LIST;
            }, function (error) {
                showMessage("error", "Cannot update layout type, please contact administrator");
            });
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