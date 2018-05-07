(function () {

    var app = angular.module('deviser.language', [
    'ui.router',
    'ui.bootstrap',
    'ui.select',
    'dev.sdlib',
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
        vm.siteLanguage = pageContext.siteLanguage;

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

        function getSiteLanguages(isOnActivate) {
            languageService.getSiteLanguages().then(function (languages) {

                vm.siteLanguages = languages;
                var activeLanguages = _.filter(languages, 'isActive');

                if (isOnActivate) {
                    console.log(pageContext);
                    if (activeLanguages.length == 1) {
                        showMessage('success', 'URLs has been updated based on active languages, this page will be reloaded automatically');
                        var reloadUrl = pageContext.currentUrl;
                        _.forEach(vm.siteLanguages, function (lang) {
                            lang = lang.cultureCode.toLowerCase() + '/';
                            if (_.startsWith(reloadUrl, lang)) {
                                reloadUrl = _.replace(reloadUrl, lang, '');
                            }
                        });

                        setTimeout(function () {
                            window.location = '/' + reloadUrl;
                        }, 5000);
                    }
                    else if (activeLanguages.length == 2) {
                        showMessage('success', 'URLs has been updated based on active languages, this page will be reloaded automatically');
                        var reloadUrl = pageContext.currentUrl;
                        var lang = pageContext.currentLocale.toLowerCase() + '/';
                        if (!_.startsWith(reloadUrl, lang)) {
                            reloadUrl = lang + reloadUrl;
                        }
                        setTimeout(function () {
                            window.location = '/' + reloadUrl;
                        }, 5000);
                    }

                }
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
            if (language.cultureCode !== pageContext.siteLanguage) {
                languageService.put(language).then(function (result) {
                    console.log(result);
                    getSiteLanguages(true);
                    showMessage("success", "Language has been updated");
                }, function (error) {
                    showMessage("error", "Cannot update language, please contact administrator");
                });
            }

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