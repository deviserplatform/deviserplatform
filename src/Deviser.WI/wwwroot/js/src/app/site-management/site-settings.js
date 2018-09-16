(function () {

    var app = angular.module('deviser.siteSettings', [
        'ui.router',
        'ui.bootstrap',
        'ui.select',
        'dev.sdlib',
        'dev.modal',
        'deviser.services',
        'deviser.config'
    ]);

    app.controller('SiteSettingsCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'devUtil', 'modalService',
        'siteSettingService', 'pageService', 'layoutService', 'themeService', 'languageService', 'applicationService', siteSettingsCtrl]);

    ////////////////////////////////
    /*Function declarations only*/
    function siteSettingsCtrl($scope, $timeout, $filter, $q, globals, devUtil, modalService,
        siteSettingService, pageService, layoutService, themeService, languageService, applicationService) {
        var vm = this;

        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];

        /*Function bindings*/
        vm.update = updateSiteSettings;
        vm.restartApplication = restartApplication;
        vm.cancel = cancel;

        init();

        //////////////////////////////////
        ///*Function declarations only*/       

        /*Controller Initialization*/
        function init() {
            vm.setting = {};
            getSiteSettings();
            getPages();
            getLayouts();
            getThemes();
            getSiteLanguages();
            initSignalR();
        }

        function initSignalR() {
            var connection = new signalR.HubConnectionBuilder()
                .withUrl("/appHub")
                .configureLogging(signalR.LogLevel.Information)
                .build();

            connection.on("OnStarted", function (message) {
                console.log(message);
                onStarted();
            });

            connect(connection);

            function connect(conn) {
                conn.start().catch(function (e) {
                    sleep(3000);
                    console.log("Reconnecting Socket");
                    connect(conn);
                });
            }

            connection.onclose(function (e) {
                connect(connection);
            });

            function sleep(milliseconds) {
                var start = new Date().getTime();
                for (var i = 0; i < 1e7; i++) {
                    if (new Date().getTime() - start > milliseconds) {
                        break;
                    }
                }
            }
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

        function restartApplication() {
            modalService.showProgressModel('Please wait...');
            applicationService.restart().then(function (result) {
                //reloadAfterRestart();
            }, function (error) {
                //reloadAfterRestart();
            });
        }

        function onStarted() {
            location.reload();
        }

        function cancel() {
            getSiteSettings();
        }

        /*Private functions*/
        function getSiteSettings() {
            siteSettingService.get().then(function (siteSettings) {
                vm.siteSettings = siteSettings;
                vm.setting = _.keyBy(siteSettings, "settingName");

                _.forEach(vm.setting, function (setting) {
                    if (setting && setting.settingValue) {
                        if (setting.settingValue === 'true') {
                            setting.settingValue = true;
                        }
                        else if (setting.settingValue === 'false') {
                            setting.settingValue = false;
                        }
                    }
                });

                //vm.setting.SMTPEnableSSL.settingValue = (vm.setting.SMTPEnableSSL.settingValue === "true") ? true : false;
                //vm.setting.RegistrationEnabled.settingValue = (vm.setting.RegistrationEnabled.settingValue === "true") ? true : false;

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

        function getLayouts() {
            layoutService.get().then(function (layouts) {
                vm.layouts = layouts;
            })
        }
        function getThemes() {
            themeService.get().then(function (themes) {
                vm.themes = themes;
            })
        }

        function getSiteLanguages() {
            languageService.getSiteLanguages().then(function (languages) {
                vm.languages = _.filter(languages, { isActive: true });
            }, function (error) {
                showMessage("error", "Cannot get all languages, please contact administrator");
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