(function () {

    var app = angular.module('deviser.language', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'sd.sdlib',
    'deviser.services',
    'deviser.config'
    ]);

    app.controller('LanguageCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals',
        'languageService', languageCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function languageCtrl($scope, $timeout, $filter, $q, globals, languageService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.add = add;
        vm.save = save;
        vm.activate = activate;
        vm.cancel = cancel;
        vm.viewStates = {
            NEW: "NEW",
            LIST: "LIST"
        };
        vm.currentViewState = vm.viewStates.LIST;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       
        function init() {
            getLanguages();
            getSiteLanguages();
        }

        function getLanguages() {
            languageService.get().then(function (languages) {
                vm.languages = languages;
            }, function (error) {
                showMessage("error", "Cannot get all languages, please contact administrator");
            });
        }

        function getSiteLanguages() {
            languageService.getSiteLanguages().then(function (languages) {
                vm.siteLanguages = languages;
            }, function (error) {
                showMessage("error", "Cannot get site languages, please contact administrator");
            });

        }

        function add() {
            vm.currentViewState = vm.viewStates.NEW;
        }

        function save() {
            languageService.post(vm.selectedLanguage).then(function (result) {
                console.log(result);
                vm.currentViewState = vm.viewStates.LIST;
                vm.selectedLanguage = {};
                getSiteLanguages();
                showMessage("success", "New language has been added");
            }, function (error) {
                showMessage("error", "Cannot add language, please contact administrator");
            });
        }

        function activate(language) {
            languageService.put(language).then(function (result) {
                console.log(result);
                getSiteLanguages();
                showMessage("success", "Role has been updated");
            }, function (error) {
                showMessage("error", "Cannot update role, please contact administrator");
            });
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