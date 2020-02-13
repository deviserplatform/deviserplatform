(function () {

    var app = angular.module('deviser.filemanger', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'dev.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('FileManagerCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals',
        'fileService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, fileService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];

        /*Funciton binding*/
        vm.upload = upload

        init();

        //////////////////////////////////
        ///*Function declarations only*/       
        function init() {
            getFiles();
        }

        function upload() {


        }

        function getFiles() {
            fileService.get().then(function (files) {
                vm.files = files;
            }, function (error) {
                showMessage("error", "Cannot get all files and folders, please contact administrator");
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