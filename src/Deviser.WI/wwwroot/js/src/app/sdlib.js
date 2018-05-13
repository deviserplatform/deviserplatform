(function () {

    var app = angular.module('dev.sdlib', [
        'ngSanitize']);

    app.directive('devEnter', sdEnterDir);

    app.directive('devNumberOnly', sdNumberOnlyDir);

    app.directive('devAlphabetsOnly', sdAlphabetsOnlyDir);

    app.directive("devEdit", ['$compile', '$templateCache', sdContenteditable]);

    app.directive('contenteditable', ['$sce', contenteditable]); 

    app.factory('devUtil', [devUtil]);

    app.factory('editLayoutUtil', [editLayoutUtil])

    app.factory('dateConverter', ['dateFilter', dateConverter]);

    app.filter('notInArray', ['$filter', notInArray]);
    /////////////////////////////////////////////
    /*Function declarations only*/
    function sdEnterDir() {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ictEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    }

    function sdNumberOnlyDir() {
        var returnObject = {
            require: 'ngModel',
            link: link
        }
        return returnObject;

        function link(scope, element, attrs, modelCtrl) {

            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == undefined) return ''
                var transformedInput = inputValue.replace(/[^0-9]/g, '');
                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }

                return transformedInput;
            });
        }
    }

    function sdAlphabetsOnlyDir() {
        var returnObject = {
            require: 'ngModel',
            link: link
        }
        return returnObject;

        function link(scope, element, attrs, modelCtrl) {

            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == undefined) return '';
               
                var transformedInput;
                transformedInput = inputValue.replace(/\s+/g, '_');
                transformedInput = transformedInput.replace(/[^_a-zA-Z]/g, '');
                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }
                return transformedInput;
            });
        }
    }

    function sdContenteditable($compile, $templateCache) {
        var returnObject = {
            restrict: "A",
            controller: ctrl,
            controllerAs: 'sdVM',
            bindToController: true,
            link: link,
            scope: {
                contentId: '@',
                containerId: '@',
                contentType: '@',
                ngModel: '=',
                onSave: '&'
            }
        };

        return returnObject;

        /////////////////////////////////////////////
        /*Function declarations only*/
        function link(scope, element, attrs, onSave, contentId, containerId) {

            //$compile(element.contents())(scope.$new());
            var templateUrl = "edit_" + attrs.contentType;
            template = $templateCache.get(templateUrl);
            element.html(template);
            $compile(element.contents())(scope);

            //element.find(".content-item-edit ") bind("blur keyup change", function () {
            //    //scope.$apply(read);
            //    console.log(element);
            //});
        }

        function ctrl($scope, $element) {
            var vm = this;
            var scope = $scope;


            if (!vm.content) {
                vm.content = "Please enter the content";
            }


            vm.edit = function () {
                vm.isEditMode = !vm.isEditMode
            }
            vm.save = function () {
                //console.log(vm.content);
                if (vm.content && scope.sdVM.contentId && scope.sdVM.containerId) {
                    scope.sdVM.onSave()(vm.content, scope.sdVM.contentId, scope.sdVM.containerId);
                }
                vm.cancel();
            }
            vm.cancel = function () {
                vm.isEditMode = !vm.isEditMode
            }

            $scope.$watch(function () {
                return scope.sdVM.ngModel;
            }, function (newValue) {
                console.log("Changed to " + newValue);
                setContent();
            });

            /////////////////////////////////////////////
            /*Function declarations only*/
            function setContent() {
                if (scope.sdVM.ngModel && scope.sdVM.contentId) {
                    var content = _.find(scope.sdVM.ngModel, function (content) {
                        return content.id === scope.sdVM.contentId;
                    });
                    if (content)
                        vm.content = content;
                }
            }
        }
    }

    function contenteditable($sce) {
        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function (scope, element, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // Specify how UI should be updated
                ngModel.$render = function () {
                    element.html($sce.getTrustedHtml(ngModel.$viewValue || ''));
                };

                // Listen for change events to enable binding
                element.on('blur keyup change', function () {
                    scope.$evalAsync(read);
                });
                read(); // initialize

                // Write data to the model
                function read() {
                    var html = element.html();
                    // When we clear the content editable the browser leaves a <br> behind
                    // If strip-br attribute is provided then we strip this out
                    if (attrs.stripBr && html == '<br>') {
                        html = '';
                    }
                    ngModel.$setViewValue(html);
                }
            }
        };
    }   

    function devUtil() {
        var service = {
            getGuid: getGuid,
            isGuid: isGuid
        };

        return service;

        function getGuid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        }

        function isGuid(guid) {
            return /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(guid);
        }

    }

    function editLayoutUtil() {
        var defaultWidth = 'col-md-4';
        var service = {
            const: {
                defaultWidth: defaultWidth
            },
            setColumnWidth: setColumnWidth,
            getColumnWidthProperty: getColumnWidthProperty
        };

        return service;

        function setColumnWidth(properties) {
            var columnWidth;

            var columnWidthProp = getColumnWidthProperty(properties);
            if (columnWidthProp && columnWidthProp.value) {
                var width = _.find(columnWidthProp.optionList.list, { id: columnWidthProp.value });
                columnWidth = width.name;
            }
            else {
                columnWidth = defaultWidth;
            }

            return columnWidth;
        }

        function getColumnWidthProperty(properties) {
            return _.find(properties, { name: 'column_width' });
        }
    }

    function notInArray($filter){
        return function (list, arrayFilter, element) {
            if (arrayFilter) {
                return $filter("filter")(list, function (listItem) {
                    //return arrayFilter.indexOf(listItem[element]) !== -1;
                    for (var i = 0; i < arrayFilter.length; i++) {
                        if (arrayFilter[i][element] == listItem[element])
                            return false;
                    }
                    return true;
                });
            }
            else {
                return list;
            }
        };
    }

    function dateConverter(dateFilter) {
        var dateConverter = {
            parseResponse: parseResponse,
            prepareRequest: prepareRequest
        };

        return dateConverter;

        function parseResponse(response) {

            //for (prop in response) {
            //    if (response.hasOwnProperty(prop)) {
            //        if (moment(response[prop], moment.ISO_8601, true).isValid()) {
            //            response[prop] = new Date(response[prop]);
            //        }
            //    }
            //}
            recursiveFunction(response);

            return response;

            function recursiveFunction(response) {
                _.forEach(response, function (value, prop) {

                    if (response.hasOwnProperty(prop)) {

                        if (moment(response[prop], moment.ISO_8601, true).isValid()) {
                            response[prop] = moment(response[prop]).toDate();//new Date(response[prop]);
                        }

                        if (angular.isArray(response[prop]) && response[prop].length > 0) {
                            _.forEach(response[prop], function (item) {
                                recursiveFunction(item);
                            });
                        }
                    }
                });
            };
        };

        function prepareRequest(obj) {
            //for (prop in item) {
            //    if (item.hasOwnProperty(prop)) {
            //        if (angular.isDate(item[prop])) {
            //            item[prop] = dateFilter(item[prop], "yyyy-MM-ddTHH:mm:ssZ")
            //        }
            //    }
            //}
            var clonedObject;

            clonedObject = angular.copy(obj);

            recursiveFunction(clonedObject);

            return clonedObject;

            function recursiveFunction(parent) {
                _.forEach(parent, function (value, prop) {

                    if (parent.hasOwnProperty(prop)) {

                        if (angular.isDate(parent[prop])) {
                            //parent[prop] = dateFilter(parent[prop], "yyyy-MM-ddTHH:mm:ssZ")
                            parent[prop] = moment(parent[prop]).format()
                        }

                        if (angular.isArray(parent[prop]) && parent[prop].length > 0) {
                            _.forEach(parent[prop], function (item) {
                                recursiveFunction(item);
                            });
                        }
                    }
                });
            };


        };

        
    }

}());


