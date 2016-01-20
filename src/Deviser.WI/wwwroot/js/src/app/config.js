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
            alertLifeTime: 5000,
        };
    });
}());