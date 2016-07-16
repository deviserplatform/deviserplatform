(function () {

    var app = angular.module('deviser.optionList', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('OptionListCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'sdUtil',
        'optionListService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, sdUtil, optionListService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.viewStates = {
            NEW: "NEW",
            EDIT: "EDIT",
            LIST: "LIST"
        };
        vm.currentViewState = vm.viewStates.LIST;


        /*Event binding*/
        vm.newList = newList;
        vm.edit = edit;
        vm.isValidName = isValidName;
        vm.save = save;
        vm.activate = activate;
        vm.cancel = cancel;
        vm.hasError = hasError;
        vm.addListItem = addListItem;
        vm.removeListItem = removeListItem;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            getOptionLists();
        }

        /*Event handlers*/
        function newList() {
            vm.currentViewState = vm.viewStates.NEW;
            vm.selectedOptionList = {};
        }

        function edit(optionList) {
            vm.currentViewState = vm.viewStates.EDIT;
            vm.selectedOptionList = optionList;
            vm.selectedOptionList.items = [];
            if (vm.selectedOptionList.list) {
                vm.selectedOptionList.items = angular.fromJson(vm.selectedOptionList.list);
            }
        }

        function isValidName(name) {
            return !_.findWhere(vm.optionLists, { name: name });
        }

        function save() {
            $scope.optionListForm.submitted = true;
            if ($scope.optionListForm.$valid) {

                if (vm.selectedOptionList.items) {
                    vm.selectedOptionList.list = angular.toJson(vm.selectedOptionList.items);
                }
                
                if (vm.currentViewState == vm.viewStates.NEW && vm.isValidName(vm.selectedOptionList.name)) {
                    optionListService.post(vm.selectedOptionList).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.selectedOptionList = {};
                        getOptionLists();
                        showMessage("success", "New option list has been added");
                    }, function (error) {
                        showMessage("error", "Cannot add option list, please contact administrator");
                    });
                }
                else {
                    update(vm.selectedOptionList);
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

        function addListItem() {
            var listItem = {
                id: sdUtil.getGuid(),
                name: '',
                label:''
            }
            vm.selectedOptionList.items.push(listItem)
        }

        function removeListItem(item) {
            vm.selectedOptionList.items = _.reject(vm.selectedOptionList.items, function (itm) {
                return itm.id === item.id;
            })
        }

        /*Private functions*/
        function getOptionLists() {
            optionListService.get().then(function (optionLists) {
                vm.optionLists = optionLists;
            }, function (error) {
                showMessage("error", "Cannot get option lists, please contact administrator");
            });
        }

        function update(optionList) {
            optionListService.put(optionList).then(function (result) {
                console.log(result);
                getOptionLists();
                showMessage("success", "Option list has been updated");
                vm.currentViewState = vm.viewStates.LIST;
            }, function (error) {
                showMessage("error", "Cannot update option list, please contact administrator");
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