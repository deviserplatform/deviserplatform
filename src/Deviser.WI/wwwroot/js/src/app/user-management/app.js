(function () {

    var app = angular.module('deviser.userManagement', [
    'ui.router',
    'ui.bootstrap',
    'ui.tree',
    'ui.select',
    'ngSanitize',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('UserManagementCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'userService', 'userExistService',
        'passwordResetService', 'roleService', 'userRoleService', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, userService, userExistService,
        passwordResetService, roleService, userRoleService) {
        var vm = this;
        var allRoles;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.add = add;
        vm.edit = edit;
        vm.remove = remove;
        vm.save = save;
        vm.cancel = cancel;
        vm.checkUserExist = checkUserExist;
        vm.resetPassword = resetPassword;
        vm.addRole = addRole;
        vm.removeRole = removeRole;
        vm.hasError = hasError;

        vm.viewStates = {
            NEW: "NEW",
            LIST: "LIST",
            EDIT: "EDIT"
        };
        vm.isUserExist = true;
        vm.currentViewState = vm.viewStates.LIST;


        getUsers();

        getRoles();

        //////////////////////////////////
        ///*Function declarations only*/       
        function getUsers() {
            userService.get().then(function (users) {
                vm.users = users;
            }, function (error) {
                showMessage("error", "Cannot get users, please contact administrator");
            });
        }

        function getRoles() {
            roleService.get().then(function (roles) {
                vm.roles = allRoles = roles;
            }, function (error) {
                showMessage("error", "Cannot get roles, please contact administrator");
            });
        }

        function add() {
            vm.currentViewState = vm.viewStates.NEW;
        }

        function edit(user) {
            vm.selectedUser = user;
            filterRoles();
            vm.currentViewState = vm.viewStates.EDIT;
        }

        function remove(user) {
            userService.remove(user.id).then(function (result) {
                console.log(result);
                getUsers();
                showMessage("success", "User has been removed");
            }, function (error) {
                showMessage("error", "Cannot remove the user, please contact administrator");
            });
        }

        function addRole() {
            var userRoleObj = {
                userId: vm.selectedUser.id,
                roleName: vm.selectedRole.name
            };
            userRoleService.post(userRoleObj).then(function (user) {
                vm.selectedUser = user;
                filterRoles();
            }, function (error) {
                showMessage("error", "Cannot add role, please contact administrator");
            });
        }

        function removeRole(role) {
            var userRoleObj = {
                userId: vm.selectedUser.id,
                roleName: role.name
            };
            userRoleService.remove(userRoleObj.userId, userRoleObj.roleName).then(function (user) {
                vm.selectedUser = user;
                filterRoles();
            }, function (error) {
                showMessage("error", "Cannot remove role, please contact administrator");
            });
        }

        function filterRoles() {
            if (vm.selectedUser) {
                vm.roles = _.reject(allRoles, function (role) {
                    return _.findWhere(vm.selectedUser.roles, { id: role.id });
                });
            }


        }

        function save() {
            $scope.userForm.submitted = true;

            if (vm.currentViewState == vm.viewStates.EDIT) {
                if ($scope.editUserForm.$valid) {
                    userService.put(vm.selectedUser).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        showMessage("success", "Profile has been updated");
                    }, function (error) {
                        showMessage("error", "Cannot update user, please contact administrator");
                    });
                }
            }
            else if (vm.currentViewState == vm.viewStates.NEW) {
                if ($scope.userForm.$valid && !vm.isUserExist) {
                    userService.post(vm.newUser).then(function (result) {
                        console.log(result);
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.newUser = {};
                        getUsers();
                        showMessage("success", "New user has been created");
                    }, function (error) {
                        if (error.errors) {
                            var validationError = '';
                            _.each(error.errors, function (er) {
                                validationError += er.description;
                            });
                            showMessage("error", validationError);
                        }
                        else {
                            showMessage("error", "Cannot update user, please contact administrator");
                        }
                    });
                }
            }
        }

        function resetPassword() {
            $scope.passwordResetForm.submitted = true;
            if ($scope.passwordResetForm.$valid) {
                vm.passwordReset.userId = vm.selectedUser.id;
                passwordResetService.post(vm.passwordReset).then(function (result) {
                    console.log(result);
                    if (result && result.succeeded === true) {
                        vm.currentViewState = vm.viewStates.LIST;
                        vm.passwordReset = {};
                        getUsers();
                        showMessage("success", "Password has been reset");
                    }
                    else {
                        if (result.errors) {
                            var validationError = '';
                            _.each(result.errors, function (er) {
                                validationError += er.description;
                            });
                            showMessage("error", validationError);
                        }
                        else {
                            showMessage("error", "Cannot reset password, please contact administrator");
                        }
                    }

                }, function (error) {
                    showMessage("error", "Cannot reset password, please contact administrator");
                });
            }
        }

        function hasError(form, field, validation) {
            if (form && validation) {
                return (form[field].$dirty && form[field].$error[validation]) || (form.submitted && form[field].$error[validation]);
            }
            return (form[field].$dirty && form[field].$invalid) || (form.submitted && form[field].$invalid);
        };


        function cancel() {
            vm.currentViewState = vm.viewStates.LIST;
        }

        function checkUserExist(userName) {
            if (!$scope.userForm.email.$error.email && userName) {
                var postObj = {
                    userName: userName
                }
                userExistService.post(postObj).then(function (result) {
                    vm.isUserExist = result;
                }, function (error) {
                    showMessage("error", "Cannot check email address, please contact administrator");
                });
            }
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