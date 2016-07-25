(function () {

    var app = angular.module('deviser.siteSettings', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('SiteSettingsCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'sdUtil',
        'propertyService', 'optionListService', siteSettingsCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function siteSettingsCtrl($scope, $timeout, $filter, $q, globals, sdUtil,
        propertyService, optionListService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
       
        /*Function bindings*/       
        vm.save = save;
        vm.cancel = cancel;
        vm.hasError = hasError;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            getSiteSettings();
        }

        /*Event handlers*/
        
        function save() {
            $scope.propertyForm.submitted = true;
            if ($scope.propertyForm.$valid) {
                //Update site settings
            }
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
        function getSiteSettings() {
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