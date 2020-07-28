import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { Image } from '../../domain-types/image';
import { Property } from '../../domain-types/property';
import { ImageSelectorComponent } from '../image-selector/image-selector.component';
import { ContentTypeField } from '../../domain-types/content-type-field';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  styleUrls: ['./image.component.scss']
})
export class ImageComponent implements OnInit {



  bsModalRef: BsModalRef;

  get image(): Image {
    return this._image;
  }

  @Input() set image(value: Image) {
    this._image = value
    this.init();
  }

  @Input() field: ContentTypeField;
  @Input() properties: Property[]

  imageCropSize: any

  private _image: Image;
  private _modalConfig: any = {
    ignoreBackdropClick: true
  }

  constructor(private _modalService: BsModalService) {
    this.imageCropSize = {};
  }

  ngOnInit(): void {
  }

  init() {
    if (this.properties) {
      let propWidth = this.properties.find(prop => prop.name === 'image_width'),
        propHeight = this.properties.find(prop => prop.name === 'image_height');
      this.imageCropSize.width = propWidth && propWidth.value ? propWidth.value : propWidth.defaultValue;
      this.imageCropSize.height = propHeight && propHeight.value ? propHeight.value : propHeight.defaultValue;
    }

    if (this.image.focusPoint) {
      this.setFocusPoint(this.image.focusPoint);
    }
  }

  showPopup() {
    let param: ModalOptions = JSON.parse(JSON.stringify(this._modalConfig));
    let imageSelected: EventEmitter<Image>;
    param.class = 'image-selector-modal';
    if (this.bsModalRef && this.bsModalRef.content) {
      imageSelected = this.bsModalRef.content.imageSelected as EventEmitter<any>;
      imageSelected.unsubscribe();
    }

    this.bsModalRef = this._modalService.show(ImageSelectorComponent, param), this._modalConfig;
    imageSelected = this.bsModalRef.content.imageSelected as EventEmitter<Image>;
    imageSelected.subscribe(image => this.onImageSelected(image));
    this.bsModalRef.content.closeBtnName = 'Close';


    // showImageManager().then(function (selectedImage) {
    //   vm.src = selectedImage.imageSource;
    //   vm.alt = selectedImage.imageAlt;
    //   vm.focusPoint = selectedImage.focusPoint;
    //   //setFocusPoint(vm.focusPoint);
    // }, function (response) {
    //   console.log(response);
    // });
  }
  onImageSelected(image: Image) {
    this.image = image
  }

  removeImage() {
    this.image.imageUrl = null;
    this.image.imageAltText = null;
    this.image.focusPoint = null;
  }

  showImageManager() {
    // let modalInstance = $uibModal.open({
    //   templateUrl: 'app/components/imageManagerPopup.tpl.html',
    //   controller: 'ImageManagerPopup as imVM',
    //   size: 'sm',
    //   openedClass: 'image-manager-modal',
    //   resolve: {
    //     selectedImage: function () {
    //       return {
    //         src: vm.src,
    //         alt: vm.alt,
    //         focusPoint: vm.focusPoint,
    //         imageCropSize: imageCropSize
    //       };
    //     }
    //   }
    // });
    // return modalInstance.result;
  }

  setFocusPoint(focusPoint: any) {

    // let $previewContainer = $('.focuspoint.preview');
    // let $focusPoint = $('.focuspoint');

    // $previewContainer.data('focusX', focusPoint.x);
    // $previewContainer.data('focusY', focusPoint.y);
    // $previewContainer.data('imageW', focusPoint.w);
    // $previewContainer.data('imageH', focusPoint.h);

    // $focusPoint.focusPoint();
  }

}
