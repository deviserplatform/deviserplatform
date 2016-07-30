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
        'siteSettingService','pageService', siteSettingsCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function siteSettingsCtrl($scope, $timeout, $filter, $q, globals, sdUtil,
        siteSettingService, pageService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
       
        /*Function bindings*/       
        vm.update = updateSiteSettings;
        vm.cancel = cancel;
        
        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            vm.setting = {};
            getSiteSettings();
            getPages();           
        }

        /*Event handlers*/
        function updateSiteSettings() {
            var settings = _.values(vm.setting);
            siteSettingService.put(settings).then(function (result) {
                getSiteSettings();
                showMessage("success", "Site Settings has been updated");
            }, function (error) {
                showMessage("error", "Cannot update site setting please contact administrator");
            });
        }
        function cancel() {
            getSiteSettings();
            //vm.setting = _.keyBy(vm.siteSettings, "settingName");
            //vm.setting.EnableDisableRegistration.settingValue = (vm.setting.EnableDisableRegistration.settingValue === "true") ? true : false;
        }

        /*Private functions*/
        function getSiteSettings() {
            siteSettingService.get().then(function (siteSettings) {
                vm.siteSettings = siteSettings;     
                vm.setting = _.keyBy(siteSettings, "settingName");
                vm.setting.EnableDisableRegistration.settingValue = (vm.setting.EnableDisableRegistration.settingValue === "true") ? true : false;

            }, function (error) {
                showMessage("error", "Cannot get all Site Settings, please contact administrator");
            });
        }

        function getPages() {
            pageService.getPages().then(function (pages) {
                vm.pages = pages;
                _.each(vm.pages, function (page) {
                    var englishTranslation = _.find(page.pageTranslation, { locale: "en-US" });
                    page.pageName = englishTranslation.name;
                });
            })
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