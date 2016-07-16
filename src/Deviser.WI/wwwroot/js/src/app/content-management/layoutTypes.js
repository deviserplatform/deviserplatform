(function () {

    var app = angular.module('deviser.layoutTypes', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('LayoutTypesCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals',
        'layoutTypeService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, layoutTypeService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.newContentType = newContentType;
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
            getLayoutTypes();
        }

        /*Event handlers*/

        function newContentType() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedLayoutType = {};
        }

        function edit(layout) {
            vm.currentViewState = vm.viewStates.EDIT;
            vm.selectedLayoutType = layout;
            var allowedTypeIds = vm.selectedLayoutType.layoutTypeIds.replace(/\s+/g, '').split(',');
            vm.selectedLayoutType.allowedLayoutTypes = _.filter(vm.layoutTypes, function (layoutType) {
                return _.contains(allowedTypeIds, layoutType.id);
            });
        }

        function isValidName(name) {
            return !_.findWhere(vm.layoutTypes, { name: name });
        }

        function save() {
            $scope.layoutForm.submitted = true;
            if ($scope.layoutForm.$valid) {
                if (vm.currentViewState == vm.viewStates.NEW && vm.isValidName(vm.selectedLayoutType.name)) {
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
                    var allowedTypeIds = _.pluck(vm.selectedLayoutType.allowedLayoutTypes, 'id');
                    vm.layoutTypeIds = allowedTypeIds.join(',');
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

        /*Private functions*/
        function getLayoutTypes() {
            layoutTypeService.get().then(function (layoutTypes) {
                vm.layoutTypes = layoutTypes;
            }, function (error) {
                showMessage("error", "Cannot get all layout types, please contact administrator");
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