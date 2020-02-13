(function () {

    var app = angular.module('deviser.contact', [
        'deviser.config',
        'deviser.services'
    ]);

    app.controller('ContactCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals','conatctFormService', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, conatctFormService) {
        var vm = this;
        var contact = {};
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.submit = submit;
        init();


        //////////////////////////////////
        ///*Function declarations only*/ 
        function init() {
            vm.form = {};
        }

        function submit() {
            contact.data = angular.toJson(vm.form);
            contact.pageModuleId = $('#ContactForm').closest('.dev-module-container').data('pageModuleId');
            conatctFormService.post(contact).then(function (success) {
                showMessage("success", "Your message have been successfully sent.");
            }, function (error) {
                showMessage("error", "Couldnt send your message, please try after sometime.");
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