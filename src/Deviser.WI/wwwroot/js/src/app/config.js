(function () {
    var app = angular.module('deviser.config', []);

    app.config(function (globalsProvider) {
        globalsProvider.appSettings.serviceBaseUrl = "/api";
        globalsProvider.appSettings.systemErrorMsg = "Unexpected error has been occured, Please contact system administrator";
    });

    app.provider('globals', function () {
        this.$get = function () {
            // code to initialize/configure the SERVICE goes here (executed during `run` stage)
            return {
                appSettings: provider.appSettings
            };
        };

        var provider = this;
        provider.appSettings = {
            serviceBaseUrl: "",
            systemErrorMsg: "",
            alertLifeTime: 10000,
            permissions: {
                pageView: '29cb1b57-1862-4300-b378-f3271b870148',
                pageEdit: '2da41181-be15-4ad6-a89c-3ba8b71f993b'
            },
            roles: {
                administrator: '9b461499-c49e-4398-bfed-4364a176ebbd'
            }

        };
    });
}());