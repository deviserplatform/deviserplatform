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
        vm.save = save;
        vm.cancel = cancel;
        vm.hasError = hasError;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            vm.setting = {};
            getSiteSettings();
            getPages();
            updateSiteSettings();
        }

        /*Event handlers*/
        
        function save() {
            $scope.propertyForm.submitted = true;
            if ($scope.propertyForm.$valid) {
                //Update site settings
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

        /*Private functions*/
        function getSiteSettings() {
            siteSettingService.get().then(function (siteSettings) {
                vm.siteSettings = siteSettings;
                vm.setting.HomePageId = _.findWhere(vm.siteSettings, { settingName: "HomePageId" });
                vm.setting.LoginPage = _.findWhere(vm.siteSettings, { settingName: "LoginPage" });
                vm.setting.RegistrationPage = _.findWhere(vm.siteSettings, { settingName: "RegistrationPage" });
                vm.setting.SiteDesctiption = _.findWhere(vm.siteSettings, { settingName: "SiteDesctiption" });
                vm.setting.SMTPSettings = _.findWhere(vm.siteSettings, { settingName: "SMTPSettings" });
                vm.setting.SiteDefaultLayout = _.findWhere(vm.siteSettings, { settingName: "SiteDefaultLayout" });
                vm.setting.SiteName = _.findWhere(vm.siteSettings, { settingName: "SiteName" });
                vm.setting.DefaultAdminLayoutId = _.findWhere(vm.siteSettings, { settingName: "DefaultAdminLayoutId" });
                vm.setting.SiteRoot = _.findWhere(vm.siteSettings, { settingName: "SiteRoot" });
                vm.setting.RedirectAfterLogout = _.findWhere(vm.siteSettings, { settingName: "RedirectAfterLogout" });
                vm.setting.RedirectAfterLogin = _.findWhere(vm.siteSettings, { settingName: "RedirectAfterLogin" });
                vm.setting.EnableDisableRegistration = _.findWhere(vm.siteSettings, { settingName: "EnableDisableRegistration" });
                vm.setting.SiteDefaultTheme = _.findWhere(vm.siteSettings, { settingName: "SiteDefaultTheme" });


            }, function (error) {
                showMessage("error", "Cannot get all Site Settings, please contact administrator");
            });
        }
        function getPages() {
            pageService.getPages().then(function (pages) {
                vm.pages = pages;
                _.each(vm.pages, function (page) {
                    var englishTranslation = _.findWhere(page.pageTranslation, { locale: "en-US" });
                    page.pageName = englishTranslation.name;
                });
            })
        }


        function updateSiteSettings() {
            siteSettingService.updateSiteSettings(settings).then(function (result) {
                getSiteSettings();
                vm.settings = settings;
                vm.settings.HomePageId = _.findWhere(vm.settings, { settingName: "HomePageId" })
                HomePageId.settingValue = vm.settings.HomePageId.settingValue;
                siteSettings.push(HomePageId)

                //var homePageSetting = _.findWhere(vm.settings, {settingName:""});
                //homePageSetting.settingValue = vm.settings.homePageId.settingValue;
                //siteSettings.push(homePageSetting);


                showMessage("success", "Site Settings has been updated");                
            }, function (error) {
                showMessage("error", "Cannot update site setting please contact administrator");
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