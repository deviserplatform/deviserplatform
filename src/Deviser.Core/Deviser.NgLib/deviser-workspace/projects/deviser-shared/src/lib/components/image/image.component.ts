import { Component, OnInit, Input, EventEmitter, Output, Inject } from '@angular/core';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { Image } from '../../domain-types/image';
import { Property } from '../../domain-types/property';
import { ImageSelectorComponent } from '../image-selector/image-selector.component';
import { ContentTypeField } from '../../domain-types/content-type-field';
import { WINDOW } from '../../services/window.service';
import { PageContext } from '../../domain-types/page-context';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  styleUrls: ['./image.component.scss']
})
export class ImageComponent implements OnInit {

  @Input() set image(value: Image) {
    this._image = value
    this.init();
  }
  @Input() field: ContentTypeField;
  @Input() properties: Property[]

  @Output() imageSelected: EventEmitter<Image> = new EventEmitter();

  get image(): Image {
    return this._image;
  }
  get imageSource(): string {
    return this.image && this.image.imageUrl ? `${this._baseUrl}${this.image.imageUrl}` : null;
  }

  imageCropSize: any
  bsModalRef: BsModalRef;

  private readonly _baseUrl;
  private _image: Image;
  private _modalConfig: any = {
    ignoreBackdropClick: true
  }
  private _pageContext: PageContext;

  constructor(private _modalService: BsModalService,
    @Inject(WINDOW) private _window: any) {
    this.imageCropSize = {};
    this._pageContext = _window.pageContext;

    if (this._pageContext.isEmbedded) {
      this._baseUrl = this._pageContext.siteRoot;
    }
    else {
      this._baseUrl = this._pageContext.debugBaseUrl;
    }
  }

  ngOnInit(): void {
  }

  init() {
    if (this.properties && this.properties.length > 0) {
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
    param.initialState = {
      image: this.image
    }
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
    this.imageSelected.emit(this.image);
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
