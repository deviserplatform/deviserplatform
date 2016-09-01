(function () {
    var app = angular.module('modules.app.imageManager', [
        'ui.router',
        'ui.bootstrap',
        'ui.sortable',
        'ui.select',
        'deviser.services',
        'ngFileUpload',
        'angular-img-cropper'
    ]);

    app.directive("sdImageManager", ['$compile', '$templateCache', 'assetService', sdImageManagerDir]);

    app.controller('ImageManagerPopup', ['$scope', '$uibModalInstance', '$timeout', 'Upload', 'assetService', 'selectedImage', imageManagerPopup]);

    function sdImageManagerDir($compile, $templateCache, assetService) {
        var returnObject = {
            restrict: "A",
            controller: ['$scope', '$uibModal', ctrl],
            controllerAs: 'locVM',
            bindToController: true,
            link: link,
            scope: {
                src: '=',
                alt: '=',
                focusPoint: '=',
                properties: '&',
                label: '@'
            }
        };

        return returnObject;
        /////////////////////////////////////////////
        /*Function declarations only*/
        function link(scope, element, attrs) {
            template = $templateCache.get("app/components/imageManager.tpl.html");
            element.html(template);
            $compile(element.contents())(scope);
        }

        function ctrl($scope, $uibModal) {
            var vm = this;
            var imageCropSize = {};
            vm.showPopup = showPopup;
            vm.removeImage = removeImage;

            init();

            /////////////////////////////////////////////
            /*Function declarations only*/
            function init() {
                var properties = vm.properties();
                imageCropSize.width = _.find(properties, { name: 'imagewidth' }).value;
                imageCropSize.height = _.find(properties, { name: 'imageheight' }).value;
                if (vm.focusPoint) {
                    setFocusPoint(vm.focusPoint);
                }
            }

            function showPopup() {
                showImageManager().then(function (selectedImage) {
                    vm.src = selectedImage.imageSource;
                    vm.alt = selectedImage.imageAlt;
                    vm.focusPoint = selectedImage.focusPoint;
                    //setFocusPoint(vm.focusPoint);
                }, function (response) {
                    console.log(response);
                });
            }

            function removeImage() {
                vm.src = null;
                vm.alt = null;
                vm.focusPoint = null;
            }

            function showImageManager() {
                var modalInstance = $uibModal.open({
                    templateUrl: 'app/components/imageManagerPopup.tpl.html',
                    controller: 'ImageManagerPopup as imVM',
                    size: 'sm',
                    openedClass: 'image-manager-modal',
                    resolve: {
                        selectedImage: function () {
                            return {
                                src: vm.src,
                                alt: vm.alt,
                                focusPoint: vm.focusPoint,
                                imageCropSize: imageCropSize
                            };
                        }
                    }
                });
                return modalInstance.result;
            }

            $scope.$watch(function () {
                return vm.focusPoint;
            }, function () {
                if (vm.focusPoint) {
                    setFocusPoint(vm.focusPoint);
                }
            });

        }
    }

    function imageManagerPopup($scope, $uibModalInstance, $timeout, Upload, assetService, selectedImage) {
        var vm = this;
        var imageCropSize = selectedImage.imageCropSize;
        vm.imageSource = selectedImage.src;
        vm.imageAlt = selectedImage.alt;
        vm.focusPoint = selectedImage.focusPoint;

        vm.focusPointAttr = {};

        /////////////////////////////////////////////
        /*Function binding*/
        vm.selectImage = selectImage;
        vm.yes = yes;
        vm.no = no;
        vm.isActive = isActive;
        vm.onFileSelect = onFileSelect;
        vm.cropImage = cropImage;
        vm.focusImage = focusImage;

        init();

        /////////////////////////////////////////////
        /*Function declarations only*/
        function init() {
            getImages();
            vm.focusPointAttr = {
                x: 0,
                y: 0,
                w: 0,
                h: 0
            };
            vm.selectedTab = 'PREVIEW';
            if (vm.imageSource) {
                setImage(vm.imageSource);
                vm.imageSource = vm.imageSource.split("?")[0];
                vm.selectedImage = {
                    name: vm.imageSource.split('/').pop()
                }
            }
            getCropSize();
        }

        function getCropSize() {
            //if (ModuleSettings.TabModuleSettings.ImageCropSize) {
            //    imageCropSize = JSON.parse(ModuleSettings.TabModuleSettings.ImageCropSize);
            //}

            vm.cropWidth = (imageCropSize && imageCropSize.width) || 300;
            vm.cropHeight = (imageCropSize && imageCropSize.height) || 200;

        }

        function selectImage(file) {
            vm.selectedImage = file;
            vm.imageSource = file.path;
            vm.selectedTab = 'PREVIEW';
            setImage(vm.imageSource);
        }

        function yes() {
            $uibModalInstance.close({
                imageSource: vm.imageSource,
                imageAlt: vm.imageAlt,
                focusPoint: vm.focusPoint
            });
        }

        function no() {
            $uibModalInstance.dismiss('cancel');
        }

        function isActive(file) {
            if (file && file.path) {
                var path = file.path.split('?')[0];
                var imageSource = vm.imageSource.split('?')[0];
                return imageSource === path;
            }
            return false;
        }

        function getImages() {
            assetService.get().then(function (images) {
                _.forEach(images, function (image) {
                    image.path += '?' + Math.random() * 100;
                })

                vm.images = images;
            });
        }

        function uploadSuccess(data, status, headers, config) {
            // file is uploaded successfully
            init();
            console.log(data);
            vm.imageSource = data.data[0];
            vm.imageSource += '?' + Math.random() * 100;
            showMessage("success", "File uploaded successfully!");
        }

        function uploadError(response) {
            console.log(response);
            if (response.status == 409) {
                //File already exists!
                init();
                showMessage("error", "File already exists! if you want to replace this file, please check 'Replace Image'.");
                vm.imageSource = response.data.filePath;
            }
            else {
                showMessage("error", "Server error has been occured: " + response.data.ExceptionMessage);
            }
        }

        function uploadProgress(evt) {
            console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total, 10));
        }

        function onFileSelect($files) {
            //$files: an array of files selected, each file has name, size, and type.
            if ($files) {
                //for (var i = 0; i < $files.length; i++) {
                //    var file = $files[i];

                //}

                var uploadObj = Upload.upload({
                    url: '/api/upload/images', //upload.php script, node.js route, or servlet url
                    method: 'POST',// or 'PUT',
                    data: {
                        files: $files
                    },
                    fileFormDataName: 'files'
                    //headers: { 'IsReplace': vm.isReplace },
                    //withCredentials: true,
                    //file: file // or list of files ($files) for html5 only
                    //fileName: 'doc.jpg' or ['1.jpg', '2.jpg', ...] // to modify the name of the file(s)
                    // customize file formData name ('Content-Disposition'), server side file variable name. 
                    //fileFormDataName: myFile, //or a list of names for multiple files (html5). Default is 'file' 
                    // customize how data is added to formData. See #40#issuecomment-28612000 for sample code
                    //formDataAppender: function(formData, key, val){}
                });

                //$scope.upload.then(success, error, progress);
                uploadObj.then(uploadSuccess, uploadError, uploadProgress);
            }
        }

        function cropImage() {
            var blobToUpload = Upload.dataUrltoBlob(vm.croppedImage);
            blobToUpload.name = vm.selectedImage.name;
            var uploadObj = Upload.upload({
                url: '/api/upload/images', //upload.php script, node.js route, or servlet url
                method: 'POST',// or 'PUT',

                //withCredentials: true,
                file: blobToUpload // or list of files ($files) for html5 only
                //fileName: 'doc.jpg' or ['1.jpg', '2.jpg', ...] // to modify the name of the file(s)
                // customize file formData name ('Content-Disposition'), server side file variable name. 
                //fileFormDataName: myFile, //or a list of names for multiple files (html5). Default is 'file' 
                // customize how data is added to formData. See #40#issuecomment-28612000 for sample code
                //formDataAppender: function(formData, key, val){}
            });

            //$scope.upload.then(success, error, progress);
            uploadObj.then(uploadSuccess, uploadError, uploadProgress);
        }

        function focusImage(event) {
            //console.log(event);
            var $target = $(event.target);

            var imageW = $target.width();
            var imageH = $target.height();

            //Calculate FocusPoint coordinates
            var offsetX = event.pageX - $target.offset().left;
            var offsetY = event.pageY - $target.offset().top;
            var focusX = (offsetX / imageW - .5) * 2;
            var focusY = (offsetY / imageH - .5) * -2;
            vm.focusPointAttr.x = focusX.toFixed(2);
            vm.focusPointAttr.y = focusY.toFixed(2);

            vm.focusPoint = vm.focusPointAttr;//'data-focus-x="' + vm.focusPointAttr.x + '" data-focus-y="' + vm.focusPointAttr.y +
            //'" data-focus-w="' + vm.focusPointAttr.w + '" data-focus-h="' + vm.focusPointAttr.h + '"';

            //Calculate CSS Percentages
            var percentageX = (offsetX / imageW) * 100;
            var percentageY = (offsetY / imageH) * 100;
            //var backgroundPosition = percentageX.toFixed(0) + '% ' + percentageY.toFixed(0) + '%';
            //var backgroundPositionCSS = 'background-position: ' + backgroundPosition + ';';

            setFocusPoint(vm.focusPoint);


            //Leave a sweet target reticle at the focus point.
            $('.reticle').css({
                'top': percentageY + '%',
                'left': percentageX + '%'
            });
        }

        function setImage(imgURL) {
            //Get the dimensions of the image by referencing an image stored in memory
            $("<img/>")
				.attr("src", imgURL)
				.load(function () {
				    vm.focusPointAttr.w = this.width;
				    vm.focusPointAttr.h = this.height;
				});
        }

        function showMessage(messageType, messageContent) {
            vm.message = {
                messageType: messageType,
                content: messageContent
            }

            $timeout(function () {
                vm.message = {};
            }, 5000)

        }
    }

    function setFocusPoint(focusPoint) {

        var $previewContainer = $('.focuspoint.preview');
        var $focusPoint = $('.focuspoint');


        //if (ictContentModule.TabModuleSettings.ImageCropSize) {
        //    var imgCropSize = JSON.parse(ictContentModule.TabModuleSettings.ImageCropSize);
        //    if (imgCropSize) {
        //        $previewContainer.css('height', imgCropSize.Height - 20);
        //        $previewContainer.css('width', imgCropSize.Width - 20);

        //    }
        //}

        $previewContainer.data('focusX', focusPoint.x);
        $previewContainer.data('focusY', focusPoint.y);
        $previewContainer.data('imageW', focusPoint.w);
        $previewContainer.data('imageH', focusPoint.h);

        $focusPoint.focusPoint();
    }

})();