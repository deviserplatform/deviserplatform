(function () {

    var app = angular.module('deviser.properties', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('PropertiesCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'sdUtil',
        'propertyService', 'optionListService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, sdUtil,
        propertyService, optionListService) {
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
        vm.newProperty = newProperty;
        vm.edit = edit;
        vm.isValidName = isValidName;
        vm.save = save;
        vm.activate = activate;
        vm.cancel = cancel;
        vm.hasError = hasError;


        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            getProperties();
            getOptionList();
        }

        /*Event handlers*/
        function newProperty() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedProperty = {};
        }

        function edit(property) {
            vm.currentViewState = vm.viewStates.EDIT;
            vm.selectedProperty = property;
            if(vm.selectedProperty.propertyOptionListId){
                vm.selectedProperty.moreOption = true;
            }
            
        }

        function isValidName(name) {
            return !_.findWhere(vm.properties, { name: name });
        }

        function save() {
            $scope.propertyForm.submitted = true;
            if ($scope.propertyForm.$valid) {                
                if (vm.selectedProperty.moreOption && vm.selectedProperty.propertyOptionList) {
                    vm.selectedProperty.propertyOptionListId = vm.selectedProperty.propertyOptionList.id;
                }
                else {
                    vm.selectedProperty.propertyOptionListId = null;
                }

                vm.selectedProperty.propertyOptionList = null;

                if (vm.currentViewState == vm.viewStates.NEW && vm.isValidName(vm.selectedProperty.name)) {
                    propertyService.post(vm.selectedProperty).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.selectedProperty = {};
                        getProperties();
                        showMessage("success", "New property has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add new property, please contact administrator");
                    });
                }
                else {
                    update(vm.selectedProperty);
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

        /*Private functions*/
        function getProperties() {
            propertyService.get().then(function (properties) {
                vm.properties = properties;
            }, function (error) {
                showMessage("error", "Cannot get all properties, please contact administrator");
            });
        }

        function getOptionList() {
            optionListService.get().then(function (optionLists) {
                vm.optionLists = optionLists;
            }, function (error) {
                showMessage("error", "Cannot get option lists, please contact administrator");
            });
        }

        function update(property) {
            propertyService.put(property).then(function (result) {
                console.log(result);
                getProperties();
                showMessage("success", "Property has been updated");
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