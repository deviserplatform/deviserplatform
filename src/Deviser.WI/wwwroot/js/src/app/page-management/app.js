(function () {

    var app = angular.module('deviser.pageMangement', [
    'ui.router',
    'ui.bootstrap',
    'ui.tree',
    'sd.sdlib',
    'deviserLayout.services',
    'deviser.config'
    ]);

    app.controller('PageManagementCtrl', ['$scope', '$timeout', '$filter', '$q', 'globals', 'pageService', 'skinService', editCtrl]);

    app.filter('selectLanguage', function () {

        return function (input, language) {
            if (input && language) {
                //console.log(input);
                //console.log(language);
                return _.filter(input, function (translation) {
                    return translation.locale === language;
                });
            }
            else {
                return input;
            }
        }

    });

    ////////////////////////////////
    /*Function declarations only*/
    function editCtrl($scope, $timeout, $filter, $q, globals, pageService, skinService) {
        var vm = this;
        SYS_ERROR_MSG = globals.appSettings.systemErrorMsg;
        vm.alerts = [];
        vm.pages = [];
        vm.selectedItem = {};
        vm.liveTill = {};
        vm.liveFrom = {};
        vm.delete = remove;
        vm.toggle = toggle;
        vm.selectCurrent = selectCurrent;
        vm.newSubPage = newSubPage;
        vm.save = save;
        vm.options = {
            accept: accept,
            dropped: dropped
        }
        vm.dateFormat = "dd.MM.yyyy";
        vm.openDatePicker = openDatePicker;
        vm.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        vm.disabledDatePicker = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

        $scope.$watch(function () {
            return vm.pages;
        }, function () {
            vm.isChanged = true;
        }, true);

        vm.currentLocale = appContext.currentCulture; //to be replaced as language feature in future
        getPages();
        getSkins();

        //////////////////////////////////
        ///*Function declarations only*/
        function getPages() {
            pageService.get().then(function (data) {
                vm.pages = [];
                vm.pages.push(data);
                sortPages(vm.pages[0]);
            }, function (error) {
                showMessage("error", "Cannot load pages, please contact administrator");
            });

        }

        function getSkins() {
            skinService.get().then(function (data) {
                vm.skins = data;
            }, function (error) {
                showMessage("error", "Cannot load skin, please contact administrator");
            });
        }

        function openDatePicker($event, dateObj) {
            $event.preventDefault();
            $event.stopPropagation();
            dateObj.opened = true;
        }

        function accept(sourceNodeScope, destNodesScope, destIndex) {
            var canDrop = (destNodesScope) && destNodesScope.depth() > 0;
            return canDrop;
        }

        function dropped(event) {
            var sourcePage = event.source.nodeScope.page;
            var destPage = event.dest.nodesScope.page;

            if (sourcePage && destPage) {
                sourcePage.parentId = destPage.id;
                sourcePage.pageLevel = event.dest.nodesScope.depth();
                reSortPages(vm.pages[0]);
                savePageTree();
            }
        }

        function remove(page) {
            if (page) {
                var pageId = page.id;
                pageService.remove(pageId).then(function (data) {
                    page.isDeleted = true;
                    showMessage("success", "Page has been removed");
                }, function (error) {
                    showMessage("error", "'Cannot remove page, please contact administrator");
                });
            }
        }

        function toggle(scope) {
            scope.toggle();
        }

        function selectCurrent(selected) {
            vm.selectedItem = selected;
        }

        function newSubPage(parentPage) {
            var newPage = {
                parentId: parentPage.id,
                childPages: [],
                pageTranslations: [
                      {
                          locale: vm.currentLocale,
                          name: "",
                          url: "",
                          title: null,
                          description: null,
                          keywords: null
                      }
                ]
            };

            pageService.post(newPage).then(function (data) {
                parentPage.childPages.push(data);
                showMessage("success", "Page created successfully");
            }, function (error) {
                showMessage("error", "'Cannot update page, please contact administrator");
            });
        }

        function save() {
            if (vm.selectedItem) {
                savePage(vm.selectedItem);
            }
        }

        function savePage(page) {
            pageService.put(page, page.id)
                    .then(function (data) {
                        showMessage("success", "Page updated successfully");
                    }, function (error) {
                        showMessage("error", "'Cannot update page, please contact administrator");
                    });
        }

        function savePageTree() {
            pageService.put(vm.pages[0])
                   .then(function (data) {
                       showMessage("success", "All pages updated successfully");
                   }, function (error) {
                       showMessage("error", "'Cannot update all pages, please contact administrator");
                   });
        }

        function reSortPages(page) {
            if (page && page.childPages) {
                _.each(page.childPages, function (child, index) {
                    child.pageOrder = index + 1;
                    reSortPages(child);
                });
            }
        }

        function sortPages(page) {

            if (page && page.childPages) {

                page.childPages = _.sortBy(page.childPages, function (page) {
                    return page.pageOrder;
                });

                _.each(page.childPages, function (child, index) {
                    sortPages(child);
                });
            }
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