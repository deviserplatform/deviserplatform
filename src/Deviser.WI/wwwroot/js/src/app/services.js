(function () {
    var app = angular.module('deviserLayout.services', ['deviser.config']);
    app.factory('layoutService', layoutService);
    app.factory('pageService', pageService);
    app.factory('contentTypeService', contentTypeService);
    app.factory('layoutTypeService', layoutTypeService);
    app.factory('pageContentService', pageContentService);
    app.factory('moduleService', moduleService);
    app.factory('pageModuleService', pageModuleService);
    app.factory('skinService', skinService);
    app.factory('userService', userService);
    app.factory('userExistService', userExistService);
    app.factory('passwordResetService', passwordResetService);
    app.factory('roleService', roleService);
    app.factory('userRoleService', userRoleService);
    

    ////////////////////////////////
    /*Function declarations only*/

    function layoutService($http, $q, globals) {
        return baseService($http, $q, globals, '/layout');
    }

    function pageService($http, $q, globals) {
        return baseService($http, $q, globals, '/page');
    }

    function skinService($http, $q, globals) {
        return baseService($http, $q, globals, '/skin');
    }

    function contentTypeService($http, $q, globals) {
        return baseService($http, $q, globals, '/contenttype');
    }

    function layoutTypeService($http, $q, globals) {
        return baseService($http, $q, globals, '/layouttype');
    }
    
    function pageContentService($http, $q, globals) {
        var serviceUrl = "/pagecontent";
        var service = baseService($http, $q, globals, serviceUrl);
        var url = globals.appSettings.serviceBaseUrl + serviceUrl;
        service.get = get;
        service.putContents = putContents;
        return service;

        ////////////////////////////////
        /*Function declarations only*/

        function get() {
            var getUrl = url;

            if (arguments[0] && arguments[1]) {
                var cultureCode = arguments[1],
                    pageId = arguments[0];
                getUrl = url + '/' + cultureCode + '/' + pageId;
            }
            else if (arguments[0]) {
                var pageContentId = arguments[0];
                getUrl = url + '/' + pageContentId;
            }

            var request = $http({
                method: 'GET',
                url: getUrl
            });
            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }

        function putContents(data) {
            var putUrl = url + '/list';
            var request = $http({
                method: 'PUT',
                url: putUrl,
                data: angular.toJson(data)
            });

            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }
    }

    function moduleService($http, $q, globals) {
        var service = baseService($http, $q, globals, '/module');
        service.put = null;
        service.post = null;
        service.remove = null;
        return service;
    }

    function pageModuleService($http, $q, globals) {
        var serviceUrl = "/pagemodule";
        var url = globals.appSettings.serviceBaseUrl + serviceUrl;
        var service = baseService($http, $q, globals, serviceUrl);
        service.get = null;
        service.put = null;
        service.putModules = putModules;
        return service;

        function putModules(data) {
            var putUrl = url + '/list';
            var request = $http({
                method: 'PUT',
                url: putUrl,
                data: angular.toJson(data)
            });

            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }
    }

    function userService($http, $q, globals) {
        var service = baseService($http, $q, globals, '/user');
        return service;
    }

    function userExistService($http, $q, globals) {
        return baseService($http, $q, globals, '/user/isexist');
    }

    function passwordResetService($http, $q, globals) {
        return baseService($http, $q, globals, '/user/passwordreset');
    }

    function roleService($http, $q, globals) {
        return baseService($http, $q, globals, '/role');
    }

    function userRoleService($http, $q, globals) {
        var serviceUrl = '/user/role';
        var service = baseService($http, $q, globals, serviceUrl);
        var url = globals.appSettings.serviceBaseUrl + serviceUrl;
        service.remove = remove;


        return service;

        function remove(userId, roleName) {
            if (typeof (id))
                var request = $http({
                    method: 'DELETE',
                    url: url + '/' + userId + '/' + roleName
                });
            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }
    }
    
    function baseService($http, $q, globals, serviceUrl) {
        var service = {
            get: get,
            put: put,
            post: post,
            remove: remove
            //remove: deletePost,
            //posts: posts
        };

        var url = globals.appSettings.serviceBaseUrl + serviceUrl;

        return service;

        ////////////////////////////////
        /*Function declarations only*/
        function get(id) {
            var getUrl = url;
            if (id) {
                getUrl = url + '/' + id;
            }

            var request = $http({
                method: 'GET',
                url: getUrl
            });
            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }

        function post(post) {
            var request = $http({
                method: 'POST',
                url: url,
                data: angular.toJson(post)
            });
            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }

        function put(data, id) {
            var putUrl = url;
            if (id) {
                putUrl = url + '/' + id;
            }

            var request = $http({
                method: 'PUT',
                url: putUrl /*+ '/' + data.id*/,
                data: angular.toJson(data)
            });

            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }

        function remove(id) {
            if (typeof (id))
            var request = $http({
                method: 'DELETE',
                url: url + '/' + id
            });
            return request.then(handleSuccess, function (response) {
                return handleError(handleError, $q)
            });
        }
    }

    function handleError(response, $q) {
        // The API response from the server should be returned in a
        // nomralized format. However, if the request was not handled by the
        // server (or what not handles properly - ex. server error), then we
        // may have to normalize it on our end, as best we can.
        if (
            !angular.isObject(response.data) ||
            !response.data.message
            ) {
            return ($q.reject("An unknown error occurred."));
        }
        // Otherwise, use expected error message.
        return ($q.reject(response.data.message));
    }


    // I transform the successful response, unwrapping the application data
    // from the API response payload.
    function handleSuccess(response) {
        return (response.data);
    }

}());