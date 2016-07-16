(function () {

    var app = angular.module('deviser.contentTypes', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('ContentTypesCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals',
        'contentTypeService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, contentTypeService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.newContent = newContent;
        vm.edit = edit;
        vm.isValidName = isValidName;
        vm.save = save;
        vm.activate = activate;
        vm.cancel = cancel;
        vm.hasError = hasError;
        vm.viewStates = {
            NEW: "NEW",
            EDIT: "EDIT",
            LIST: "LIST"
        };
        vm.currentViewState = vm.viewStates.LIST;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            getContentTypes();
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
                        getContentTypes();
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

        /*Private functions*/
        function getContentTypes() {
            contentTypeService.get().then(function (contentTypes) {
                vm.contentTypes = contentTypes;
            }, function (error) {
                showMessage("error", "Cannot get all content types, please contact administrator");
            });
        }

        function update(contentType) {
            contentTypeService.put(contentType).then(function (result) {
                console.log(result);
                getContentTypes();
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