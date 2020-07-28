import { Component, OnInit, EventEmitter, ViewChild, ElementRef, Inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AssetService } from '../../services/asset.service';
import { AlertService } from '../../services/alert.service';
import { AlertType } from '../../domain-types/alert';
import { map, catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { HttpEventType, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { of, Observable, Subject } from 'rxjs';
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
  images$: Observable<FileItem[]>;
  imageAltText: string = '';
  imageCropSize: any;
  imageSelected: EventEmitter<Image> = new EventEmitter();
  imageSource: string = '';
  isReplace: boolean;
  progressInfos = [];
  searchText: string;
  selectedImage: FileItem;
  selectedTab: string;

  @ViewChild("fileUpload", { static: false }) fileUpload: ElementRef;

  private _searchTerms = new Subject<string>();
  private _pageContext: PageContext;
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

    this.init();
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

  select() {
    const image = {
      imageUrl: `${this._baseUrl}${this.selectedImage.path}`,
      imageAltText: this.imageAltText,
      focusPoint: {}
    }
    this.imageSelected.emit(image);
    this.bsModalRef.hide();
  }

  search(term: string): void {
    this._searchTerms.next(term);
  }

  isActive(file) {
    if (file && file.path) {
      let path = file.path.split('?')[0];
      let imageSource = this.imageSource.split('?')[0];
      return imageSource === path;
    }
    return false;
  }

  selectImage(file) {
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
    this.getImages();
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
    this.images$ = this._searchTerms.pipe(
      // wait 300ms after each keystroke before considering the term
      debounceTime(300),

      // ignore new term if same as previous term
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this._assetService.searchImages(term)),
    );
  }

  private getCropSize() {
    //if (ModuleSettings.TabModuleSettings.ImageCropSize) {
    //    imageCropSize = JSON.parse(ModuleSettings.TabModuleSettings.ImageCropSize);
    //}

    this.cropWidth = (this.imageCropSize && this.imageCropSize.width) || 300;
    this.cropHeight = (this.imageCropSize && this.imageCropSize.height) || 200;

  }

  private getImages() {
    this._assetService.getImages().subscribe(images => {
      images.forEach(image => {
        image.path += '?' + Math.random() * 100;
      });
      this.images$ = of(images);
    });
  }

  private uploadSuccess() {
    // file is uploaded successfully
    this.init();
    // this.imageSource = data.data[0];
    // this.imageSource += '?' + Math.random() * 100;
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
    // //$files: an array of files selected, each file has name, size, and type.
    // if ($files) {
    //   //for (let i = 0; i < $files.length; i++) {
    //   //    let file = $files[i];

    //   //}

    //   let uploadObj = this._assetService.upload({
    //     url: '/api/upload/images', //upload.php script, node.js route, or servlet url
    //     method: 'POST',// or 'PUT',
    //     data: {
    //       files: $files
    //     },
    //     fileFormDataName: 'files'
    //     //headers: { 'IsReplace': this.isReplace },
    //     //withCredentials: true,
    //     //file: file // or list of files ($files) for html5 only
    //     //fileName: 'doc.jpg' or ['1.jpg', '2.jpg', ...] // to modify the name of the file(s)
    //     // customize file formData name ('Content-Disposition'), server side file letiable name. 
    //     //fileFormDataName: myFile, //or a list of names for multiple files (html5). Default is 'file' 
    //     // customize how data is added to formData. See #40#issuecomment-28612000 for sample code
    //     //formDataAppender: private(formData, key, val){}
    //   });

    //   //$scope.upload.then(success, error, progress);
    //   uploadObj.then(uploadSuccess, uploadError, uploadProgress);
    // }
    this.progressInfos[index] = { value: 0, fileName: file.name };

    this._assetService.upload(file).subscribe(
      event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progressInfos[index].value = Math.round(100 * event.loaded / event.total);
        } else if (event instanceof HttpResponse) {
          this.uploadSuccess()
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
