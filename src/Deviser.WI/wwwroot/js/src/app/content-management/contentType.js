(function () {

    var app = angular.module('deviser.contentTypes', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('ContentTypesCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'sdUtil',
        'contentTypeService', 'propertyService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, sdUtil,
        contentTypeService, propertyService) {
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
        vm.newContent = newContent;
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
            getContentTypes();
            getContentDataTypes();
            getProperties();
        }

        /*Event handlers*/
        function newContent() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedContentType = {};
        }

        function edit(contentType) {
            vm.currentViewState = vm.viewStates.EDIT;
            vm.selectedContentType = contentType;
        }

        function isValidName(name) {
            return !_.findWhere(vm.contentTypes, { name: name });
        }

        function save() {
            $scope.contentTypeForm.submitted = true;
            if ($scope.contentTypeForm.$valid) {
                if (vm.currentViewState == vm.viewStates.NEW && vm.isValidName(vm.selectedContentType.name)) {
                    contentTypeService.post(vm.selectedContentType).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.selectedContentType = {};
                        init();
                        showMessage("success", "New content type has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add layout type, please contact administrator");
                    });
                }
                else {                    
                    update(vm.selectedContentType);
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
                if (!vm.selectedContentType.properties) {
                    vm.selectedContentType.properties = [];
                }

                if (vm.selectedProperty.id) {
                    //Add existing property
                    vm.selectedContentType.properties.push(vm.selectedProperty)
                }
                else {
                    //Add new property to service and then add it to selected content type
                    propertyService.post(vm.selectedProperty).then(function (property) {
                        console.log(property);
                        vm.selectedProperty = property;
                        vm.selectedContentType.properties.push(vm.selectedProperty)
                        getProperties();
                        showMessage("success", "New property has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add new property, please contact administrator");
                    });
                }
            }
        }

        function removeProperty(property) {
            vm.selectedContentType.properties = _.reject(vm.selectedContentType.properties, function (prop) {
                return prop.id === property.id;
            })
        }

        function isPropExist() {
            var isExist = _.findWhere(vm.selectedContentType.properties, { id: vm.selectedProperty.id });
            return isExist;
        }

        /*Private functions*/
        function getContentTypes() {
            contentTypeService.get().then(function (contentTypes) {
                vm.contentTypes = contentTypes;
            }, function (error) {
                showMessage("error", "Cannot get all content types, please contact administrator");
            });
        }

        function getContentDataTypes() {
            contentTypeService.getContentDataType().then(function (contentDataTypes) {
                vm.contentDataTypes = contentDataTypes;
            }, function (error) {
                showMessage("error", "Cannot get all content data types, please contact administrator");
            });
        }

        function getProperties() {
            propertyService.get().then(function (properties) {                
                vm.properties = properties;
            }, function (error) {
                showMessage("error", "Cannot get all properties, please contact administrator");
            });
        }

        function update(contentType) {
            contentTypeService.put(contentType).then(function (result) {
                console.log(result);
                init();
                showMessage("success", "Content Type has been updated");
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