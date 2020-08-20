import { Component, OnInit, EventEmitter, ViewChild, ElementRef, Inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AssetService } from '../../services/asset.service';
import { AlertService } from '../../services/alert.service';
import { AlertType } from '../../domain-types/alert-type';
import { map, catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { HttpEventType, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { of, Observable, Subject, BehaviorSubject } from 'rxjs';
import { Image } from '../../domain-types/image';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { FileItem } from '../../domain-types/file-item';
import { PageContext } from '../../domain-types/page-context';
import { WINDOW } from '../../services/window.service';

@Component({
  selector: 'app-image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.scss']
})
export class ImageSelectorComponent implements OnInit {

  croppedImage: any = '';
  cropHeight: any;
  cropWidth: any;
  focusPointAttr: { x: number; y: number; w: number; h: number; };
  files = [];
  files$: Observable<FileItem[]>;
  imageAltText: string = '';
  imageCropSize: any;
  imageSelected: EventEmitter<Image> = new EventEmitter();
  imageSource: string = '';
  isReplace: boolean;
  progressInfos = [];
  searchText: string;
  selectedImage: FileItem;
  selectedTab: string;


  set image(value: Image) {
    this._image = value
    this.init();
  }

  @ViewChild("fileUpload", { static: false }) fileUpload: ElementRef;

  private _searchTerms = new BehaviorSubject<string>('');
  private _pageContext: PageContext;
  private _image: Image;
  private readonly _baseUrl;

  constructor(public bsModalRef: BsModalRef,
    private _alertService: AlertService,
    private _assetService: AssetService,
    @Inject(WINDOW) private _window: any) {
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

  cancel() {
    this.bsModalRef.hide();
  }

  cropImage() {
    let block = this.croppedImage.split(";");
    // Get the content type of the image
    let contentType = block[0].split(":")[1];// In this case "image/gif"
    // get the real base64 content of the file
    let realData = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

    // Convert it to a blob to upload
    let blob = this.b64toBlob(realData, contentType);
    let file = new File([blob], this.selectedImage.name);
    this.upload(0, file);
  }

  selectImage() {
    const image = {
      imageUrl: this.selectedImage.path,
      imageAltText: this.imageAltText,
      focusPoint: {}
    }
    this.imageSelected.emit(image);
    this.bsModalRef.hide();
  }

  search(searchText: string): void {
    this._searchTerms.next(searchText);
  }

  isActive(file) {
    if (file && file.path && this.selectedImage && this.selectedImage.path) {
      let path = file.path.split('?')[0];
      let imageSource = this.selectedImage.path.split('?')[0];
      return imageSource === path;
    }
    return false;
  }

  selectFile(file) {
    this.selectedImage = file;
    this.imageSource = `${this._baseUrl}${this.selectedImage.path}`;
    this.selectedTab = 'PREVIEW';
    this.setImage(this.imageSource);
  }

  onFileDropped(fileList: FileList) {
    console.log(fileList);
    for (let i = 0; i < fileList.length; i++) {
      this.upload(i, fileList[i]);
    }
  }

  onUploadClick() {
    const fileUpload = this.fileUpload.nativeElement; fileUpload.onchange = () => {
      for (let index = 0; index < fileUpload.files.length; index++) {
        const file: File = fileUpload.files[index];
        // this.files.push({ data: file, inProgress: false, progress: 0 });
        this.upload(index, file);
      }
    };
    fileUpload.click();
  }

  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
  }
  imageLoaded() {
    // show cropper
  }
  cropperReady() {
    // cropper ready
  }
  loadImageFailed() {
    // show message
  }

  private init() {
    this.focusPointAttr = {
      x: 0,
      y: 0,
      w: 0,
      h: 0
    };
    this.selectedTab = 'PREVIEW';
    if (this.imageSource) {
      this.setImage(this.imageSource);
      this.imageSource = this.imageSource.split('?')[0];
      this.selectedImage = {
        name: this.imageSource.split('/').pop()
      }
    }
    this.getCropSize();
    this._searchTerms.pipe(
      // wait 300ms after each keystroke before considering the term
      debounceTime(300),

      // ignore new term if same as previous term
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this._assetService.searchImages(term)),
    ).subscribe(files => this.onGetImages(files));
  }

  private getCropSize() {
    //if (ModuleSettings.TabModuleSettings.ImageCropSize) {
    //    imageCropSize = JSON.parse(ModuleSettings.TabModuleSettings.ImageCropSize);
    //}
    this.cropWidth = (this.imageCropSize && this.imageCropSize.width) || 300;
    this.cropHeight = (this.imageCropSize && this.imageCropSize.height) || 200;

  }

  private onGetImages(files: FileItem[]) {
    files.forEach(image => {
      image.path += '?' + Math.random() * 100;
    });
    this.files$ = of(files);
    if (this._image && this._image.imageUrl) {
      let selectedFile = files.find(image => image && image.path && image.path.split('?')[0] === this._image.imageUrl.split('?')[0]);
      selectedFile && this.selectFile(selectedFile);
    }
  }


  private uploadSuccess(filePath) {
    // file is uploaded successfully    
    // this.imageSource = `${this._baseUrl}${filePath}`;
    // this.imageSource += '?' + Math.random() * 100;
    this.init();
    this.progressInfos = [];
    this._alertService.showMessage(AlertType.Info, 'File uploaded successfully!');
  }

  private uploadError(response) {
    console.log(response);
    if (response.status == 409) {
      //File already exists!
      this.init();
      this._alertService.showMessage(AlertType.Error, 'File already exists! if you want to replace this file, please check "Replace Image".');
      this.imageSource = response.data.filePath;
    }
    else {
      this._alertService.showMessage(AlertType.Error, 'Server error has been occured: ' + response.data.ExceptionMessage);
    }
  }

  private upload(index: number, file: File) {
    this.progressInfos[index] = { value: 0, fileName: file.name };

    this._assetService.uploadImages(file).subscribe(
      event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progressInfos[index].value = Math.round(100 * event.loaded / event.total);
        } else if (event instanceof HttpResponse) {
          this.uploadSuccess(event.body[0])
        }
      },
      err => {
        this.uploadError(err);
        // this.progressInfos[index].value = 0;
        // this._alertService.showMessage(AlertType.Error, 'Could not upload the file:' + file.name);
      });
  }

  private focusImage(event) {
    // //console.log(event);
    // let $target = $(event.target);

    // let imageW = $target.width();
    // let imageH = $target.height();

    // //Calculate FocusPoint coordinates
    // let offsetX = event.pageX - $target.offset().left;
    // let offsetY = event.pageY - $target.offset().top;
    // let focusX = (offsetX / imageW - .5) * 2;
    // let focusY = (offsetY / imageH - .5) * -2;
    // this.focusPointAttr.x = focusX.toFixed(2);
    // this.focusPointAttr.y = focusY.toFixed(2);

    // this.focusPoint = this.focusPointAttr;//'data-focus-x='' + this.focusPointAttr.x + '' data-focus-y='' + this.focusPointAttr.y +
    // //'' data-focus-w='' + this.focusPointAttr.w + '' data-focus-h='' + this.focusPointAttr.h + ''';

    // //Calculate CSS Percentages
    // let percentageX = (offsetX / imageW) * 100;
    // let percentageY = (offsetY / imageH) * 100;
    // //let backgroundPosition = percentageX.toFixed(0) + '% ' + percentageY.toFixed(0) + '%';
    // //let backgroundPositionCSS = 'background-position: ' + backgroundPosition + ';';

    // this.setFocusPoint(this.focusPoint);


    // //Leave a sweet target reticle at the focus point.
    // $('.reticle').css({
    //   'top': percentageY + '%',
    //   'left': percentageX + '%'
    // });
  }

  private setImage(imgURL) {
    // //Get the dimensions of the image by referencing an image stored in memory
    // $('<img/>')
    //   .attr('src', imgURL)
    //   .load(private() {
    //     this.focusPointAttr.w = this.width;
    //     this.focusPointAttr.h = this.height;
    //   });
  }

  private b64toBlob(b64Data, contentType, sliceSize = null) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;

    let byteCharacters = atob(b64Data);
    let byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      let slice = byteCharacters.slice(offset, offset + sliceSize);

      let byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      let byteArray = new Uint8Array(byteNumbers);

      byteArrays.push(byteArray);
    }

    let blob = new Blob(byteArrays, { type: contentType });
    return blob;
  }
}
