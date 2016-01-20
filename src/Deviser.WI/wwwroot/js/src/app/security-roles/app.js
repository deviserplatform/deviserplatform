(function () {

    var app = angular.module('deviser.securityRoles', [
    'ui.router',
    'ui.bootstrap',
    'ui.tree',
    'ngPasswordStrength',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('SecurityRolesCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'roleService', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, roleService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.add = add;
        vm.edit = edit;
        vm.remove = remove;
        vm.save = save;
        vm.cancel = cancel;        
        vm.hasError = hasError;
        vm.viewStates = {
            NEW: "NEW",
            LIST: "LIST",
            EDIT: "EDIT"
        };
        vm.currentViewState = vm.viewStates.LIST;

        getRoles();

        //////////////////////////////////
        ///*Function declarations only*/       
        function getRoles() {
            roleService.get().then(function (roles) {
                vm.roles = roles;
            }, function (error) {
                showMessage("error", "Cannot get all roles, please contact administrator");
            });
        }

        function add() {
            vm.currentViewState = vm.viewStates.NEW;
        }

        function edit(role) {
            vm.selectedRole = role;
            vm.currentViewState = vm.viewStates.EDIT;
        }

        function remove(role) {
            roleService.remove(role.id).then(function (result) {
                console.log(result);
                getRoles();
                vm.currentViewState = vm.viewStates.LIST;
                showMessage("success", "Role has been removed");
            }, function (error) {
                showMessage("error", "Cannot remove role, please contact administrator");
            });
        }

        function save() {
            $scope.roleForm.submitted = true;

            if (vm.currentViewState == vm.viewStates.EDIT) {
                if ($scope.editRoleForm.$valid) {
                    roleService.put(vm.selectedRole).then(function (result) {
                        console.log(result);
                        getRoles();
                        vm.currentViewState = vm.viewStates.LIST;
                        showMessage("success", "Role has been updated");
                    }, function (error) {
                        showMessage("error", "Cannot update role, please contact administrator");
                    });
                }
            }
            else if (vm.currentViewState == vm.viewStates.NEW) {
                if ($scope.roleForm.$valid) {
                    roleService.post(vm.newRole).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.newRole = {};
                        getRoles();
                        showMessage("success", "New role has been created");
                    }, function (error) {
                        showMessage("error", "Cannot create role, please contact administrator");
                    });
                }
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