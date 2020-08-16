import { Component, OnInit, Output, EventEmitter, Inject, ElementRef, ViewChild } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Link } from '../../domain-types/link';
import { LinkType } from '../../domain-types/link-type';
import { PageService } from '../../services/page.service';
import { Page } from '../../domain-types/page';
import { PageLink } from '../../domain-types/page-link';
import { PageContext } from '../../domain-types/page-context';
import { WINDOW } from '../../services/window.service';
import { AlertService } from '../../services/alert.service';
import { AssetService } from '../../services/asset.service';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { FileItem } from '../../domain-types/file-item';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { AlertType } from '../../domain-types/alert-type';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-edit-link',
  templateUrl: './edit-link.component.html',
  styleUrls: ['./edit-link.component.scss']
})
export class EditLinkComponent implements OnInit {

  @Output() linkChanged: EventEmitter<Link> = new EventEmitter();

  files$: Observable<FileItem[]>;
  isReplace: boolean;
  linkType = LinkType;
  pageRoot: Page;
  pageLinks: PageLink[] = [];
  progressInfos = [];
  searchText: string;
  selectedTab: string;

  private _pageContext: PageContext;
  private _searchTerms = new BehaviorSubject<string>('');

  set link(value: Link) {
    this._link = value
    this.init();
  }

  get link(): Link {
    return this._link;
  }

  @ViewChild("fileUpload", { static: false }) fileUpload: ElementRef;

  private _link: Link;

  constructor(public bsModalRef: BsModalRef,
    private _alertService: AlertService,
    private _assetService: AssetService,
    private _pageService: PageService,
    @Inject(WINDOW) private _window: any) {
    this._pageContext = _window.pageContext;
  }

  ngOnInit(): void {
  }

  cancel() {
    this.bsModalRef.hide();
  }

  isActive(file: FileItem) {
    if (file && file.path && this.link && this.link.linkType === LinkType.File && this.link.url) {
      let path = file.path.split('?')[0];
      let filePath = this.link.url.split('?')[0];
      return filePath === path;
    }
    return false;
  }

  search(searchText: string): void {
    this._searchTerms.next(searchText);
  }

  selectLink() {
    this.linkChanged.emit(this.link);
    this.bsModalRef.hide();
  }

  selectFile(file: FileItem) {
    this.link.url = file.path;
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

  private init() {
    this._pageService.getPageTree().subscribe(pageRoot => this.onGetPageRoot(pageRoot))

    this._searchTerms.pipe(
      // wait 300ms after each keystroke before considering the term
      debounceTime(300),

      // ignore new term if same as previous term
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this._assetService.searchDocuments(term)),
    ).subscribe(files => this.onGetFiles(files));
  }

  private onGetPageRoot(pageRoot: Page) {
    this.pageRoot = pageRoot;
    this.pageLinks = [];
    this.processPage(this.pageRoot);
  }

  private onGetFiles(files: FileItem[]) {
    if (!files) return;
    files.forEach(file => {
      file.path += '?' + Math.random() * 100;
    });
    this.files$ = of(files);
    if (this.link && this.link.linkType === LinkType.File && this.link.url) {
      let selectedFile = files.find(image => image.path.split('?')[0] === this.link.url.split('?')[0]);
      selectedFile && this.selectFile(selectedFile);
    }
  }

  private processPage(page: Page, levelPrefix?: string) {
    if (!levelPrefix) {
      levelPrefix = '';
    }
    else if (levelPrefix === '') {
      levelPrefix = '--';
    }

    if (page.pageLevel != 0) {
      let translation = page.pageTranslation.find(translation => translation.locale === this._pageContext.currentLocale);
      // page.pageName = levelPrefix + englishTranslation.name;
      let pageLink: PageLink = {
        id: page.id,
        name: `${levelPrefix} ${translation.name}`
      }
      this.pageLinks.push(pageLink);
    }

    if (page.childPage && page.childPage.length > 0) {
      page.childPage.forEach(child => {
        this.processPage(child, `${levelPrefix}--`);
      });
    }
  }

  private upload(index: number, file: File) {
    this.progressInfos[index] = { value: 0, fileName: file.name };

    this._assetService.uploadDocuments(file).subscribe(
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
      this._alertService.showMessage(AlertType.Error, 'File already exists! if you want to replace this file, please check "Replace File".');
    }
    else {
      this._alertService.showMessage(AlertType.Error, 'Server error has been occured: ' + response.data.ExceptionMessage);
    }
  }


}
