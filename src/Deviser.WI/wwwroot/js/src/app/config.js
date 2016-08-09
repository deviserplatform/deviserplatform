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
                pageEdit: '2da41181-be15-4ad6-a89c-3ba8b71f993b',
                moduleView: '34b46847-80be-4099-842a-b654ad550c3e',
                moduleEdit: 'cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699',
                contentView: '491b37a3-deba-4f55-9df6-a67cdd810108',
                contentEdit: '461b37d9-b801-4235-b74f-0c51f35d170f'
            },
            roles: {
                administrator: '9b461499-c49e-4398-bfed-4364a176ebbd'
            }

        };
    });
}());