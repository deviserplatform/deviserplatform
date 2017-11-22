(function () {

    var app = angular.module('sd.sdlib', [
        'ngSanitize']);

    app.directive('sdEnter', sdEnterDir);

    app.directive('sdNumberOnly', sdNumberOnlyDir);

    app.directive('sdToLowerCase', sdToLowerCaseDir);

    app.directive("sdEdit", ['$compile', '$templateCache', sdContenteditable]);

    app.directive('contenteditable', ['$sce', contenteditable]); 

    app.factory('sdUtil', [sdUtil]);

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

    function sdToLowerCaseDir() {
        var returnObject = {
            require: 'ngModel',
            link: link
        }
        return returnObject;

        function link(scope, element, attrs, modelCtrl) {

            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == undefined) return '';
                var transformedInput = inputValue.toLowerCase();
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

    function sdUtil() {
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

}());


