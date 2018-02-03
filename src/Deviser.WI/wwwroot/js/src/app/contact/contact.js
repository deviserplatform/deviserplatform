(function () {

    var app = angular.module('deviser.contact', [
        'deviser.config'
    ]);

    app.controller('ContactCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', editCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];

        init();


        //////////////////////////////////
        ///*Function declarations only*/ 
        function init() {
            vm.form = {};
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