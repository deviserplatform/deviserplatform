import { Component, OnInit, Inject, EventEmitter } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { BsModalRef, ModalOptions, BsModalService } from 'ngx-bootstrap/modal';
import {
  AlertService, CoreService, ContentTranslationService, EditService,
  Guid, LanguageService, PageService, PageContentService, EditLinkComponent, ImageSelectorComponent, LinkType
} from 'deviser-shared';
import { forkJoin } from 'rxjs';
import {
  AlertType, ContentType, ContentTypeField, Image, Language, Link, 
  Page, PageContent, PageContentTranslation, PageContext, WINDOW
} from 'deviser-shared';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

var editContentComponent: EditContentComponent;

@Component({
  selector: 'app-edit-content',
  templateUrl: './edit-content.component.html',
  styleUrls: ['./edit-content.component.scss']
})
export class EditContentComponent implements OnInit {

  private _pageContentId: string;
  private _fields: ContentTypeField[];
  private _fieldValues: { [fieldName: string]: any };
  private _pageContext: PageContext;
  private _baseUrl: string;
  private _modalConfig: any = {
    ignoreBackdropClick: true
  }
  get pageContentId(): string {
    return this._pageContentId;
  }

  set pageContentId(value: string) {
    this._pageContentId = value;
    this.init();
  }

  viewState: ViewState
  pageContext: PageContext;
  languages: Language[];
  selectedLocale: Language;
  contentTranslations: PageContentTranslation[];
  contentType: ContentType;
  pages: Page[];
  pageContent: PageContent;
  contentTranslation: PageContentTranslation;
  selectedItem: any = {};
  tempItem: any;
  isChanged: boolean;
  contentSaved: EventEmitter<PageContentTranslation> = new EventEmitter();
  viewStates = ViewState;
  modules: any = {
    toolbar: {
      container: [
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
        ['bold', 'italic', 'underline', 'strike'],
        ['blockquote', 'code-block'],
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'indent': '-1' }, { 'indent': '+1' }],
        [{ 'align': [] }],
        ['clean'],
        ['link', 'image']
      ],
      handlers: {
        'link': function (value) {
          editContentComponent.onEditorLinkClicked(this.quill, value);
        },
        'image': function (callback) {
          editContentComponent.showImagePopup(this.quill);
        }
      }
    }
  }

  get isList(): boolean {
    return this.pageContent && this.pageContent.contentType && this.pageContent.contentType.isList;
  }

  get content(): any {
    // const contentTranslation = this.pageContent.pageContentTranslation.find(pct => pct.cultureCode === this.pageContext.currentLocale);
    // let content = (contentTranslation && contentTranslation.contentData) ? JSON.parse(contentTranslation.contentData) : {};
    // return content;
    return this.contentTranslation.contentData;
  }

  get contentItems(): any[] {
    if (!this.content || !this.content.items) return;

    let contentItems: any[] = this.content.items;
    let sortedItems = contentItems.sort((a, b) => a.sortOrder > b.sortOrder ? 1 : -1);
    return sortedItems;
  }

  get fields(): ContentTypeField[] {
    return this._fields;
  }

  constructor(private _alertService: AlertService,
    public bsModalRef: BsModalRef,
    public bsImageModalRef: BsModalRef,
    public bsLinkModalRef: BsModalRef,
    private _coreService: CoreService,
    private _contentTranslationService: ContentTranslationService,
    private _editService: EditService,
    private _pageContentService: PageContentService,
    private _langaugeService: LanguageService,
    private _modalService: BsModalService,
    private _pageService: PageService,
    @Inject(WINDOW) _window: any) {
    this.pageContext = _window.pageContext;
    this.viewState = ViewState.List;
    this._pageContext = _window.pageContext;
    if (this._pageContext.isEmbedded) {
      this._baseUrl = this._pageContext.siteRoot;
    }
    else {
      this._baseUrl = this._pageContext.debugBaseUrl;
    }
    _pageService.getPages().subscribe(pages => this.pages = pages)
    this._fieldValues = {
      Link: {
        linkType: '',
        linkText: '',
        isNewWindow: false,
        url: '',
        pageId: ''
      },
      Attachment: {
        fieldName: '',
        fileType: '',
        filePath: ''
      },
      Image: {
        imageUrl: '',
        imageAltText: '',
        focusPoint: {}
      }
    }
    editContentComponent = this;
  }

  ngOnInit(): void {
  }

  save() {
    if (this.contentTranslation.id) {
      this.serializeContentTranslation();
      this._contentTranslationService.updatePageContentTranslation(this.contentTranslation).subscribe(data => {
        // data.contentData = JSON.parse(data.contentData);
        console.log(data);
        this.contentSaved.emit(data);
        this.bsModalRef.hide();
      }, error => this._alertService.showMessage(AlertType.Error, 'Unable to save content'));
    }
    else {
      this.contentTranslation.pageContentId = this.pageContentId;
      this.serializeContentTranslation();
      this._contentTranslationService.createPageContentTranslation(this.contentTranslation).subscribe(data => {
        // data.contentData = JSON.parse(data.contentData);
        console.log(data);
        this.contentSaved.emit(data);
        this.bsModalRef.hide();
      }, error => this._alertService.showMessage(AlertType.Error, 'Unable to save content'));
    }
  }

  newItem() {
    this.selectedItem = {};
    this.fields.forEach(field => {
      const filedVal = this._fieldValues[field.contentFieldType.name] ? this._fieldValues[field.contentFieldType.name] : '';
      this.selectedItem[field.fieldName] = JSON.parse(JSON.stringify(filedVal));
    });
    this.viewState = ViewState.New;
  }

  cancel() {
    this.bsModalRef.hide();
  }

  get bsConfig(): any {
    return {
      dateInputFormat: 'DD.MM.YYYY'
    };
  }

  updateItem() {
    if (!this.selectedItem.id) {
      this.selectedItem.id = Guid.newGuid();
      this.selectedItem.viewOrder = this.contentTranslation.contentData.items.length + 1;
      this.contentTranslation.contentData.items.push(this.selectedItem);
    }
    this.viewState = ViewState.List;
  }

  editItem(item) {
    this.selectedItem = item;
    this.tempItem = JSON.parse(JSON.stringify(item));
    this.viewState = ViewState.Edit;
  }

  removeItem(item) {
    var index = this.contentTranslation.contentData.items.indexOf(item);
    this.contentTranslation.contentData.items.splice(index, 1);
    this.isChanged = true;
  }

  cancelDetailView() {
    if (this.viewState === ViewState.New) {
      this.selectedItem = {};
    }
    else if (this.viewState === ViewState.Edit) {
      var index = this.contentTranslation.contentData.items.findIndex(item => item.id === this.selectedItem.id);
      this.selectedItem = this.tempItem;
      // Replace item at index using native splice
      this.contentTranslation.contentData.items.splice(index, 1, this.tempItem);
    }
    this.viewState = ViewState.List;
  }

  onImageSelected($event: Image, content: any, field: ContentTypeField) {
    content[field.fieldName] = $event;
  }

  onLinkChanged($event: Link, content: any, field: ContentTypeField) {
    content[field.fieldName] = $event;
  }

  getLinkUrl(content: any) {
    // let link: Link = content;

    // if (!link || !link.linkType) return '';

    // if (link.linkType === LinkType.Url) {
    //   return link.url;
    // } else if (link.linkType === LinkType.Page) {
    //   let page = this.pages.find(p => p.id === link.pageId);
    //   let translation = page.pageTranslation.find(pt => pt.locale === this.pageContext.currentLocale);
    //   translation = translation ? translation : page.pageTranslation[0];
    //   let url = page.pageTypeId === Globals.appSettings.pageTypes.url ? translation.uRL : `${this.pageContext.siteRoot}${translation.uRL}`;
    //   return url;
    // }
    return this._editService.getLinkUrl(content);
  }

  getImageUrl(image: Image) {
    return image && image.imageUrl ? `${this._baseUrl}${image.imageUrl}` : null;
  }

  drop(event: CdkDragDrop<any[]>) {
    let gridItems = event.container.data;
    moveItemInArray(gridItems, event.previousIndex, event.currentIndex);
    gridItems.forEach((item, index) => item.sortOrder = index + 1);
  }

  onEditorLinkClicked(quill: any, value: any) {
    console.log('from angular component!');
    if (value) {
      // let href = prompt('Enter the URL');
      // quill.format('link', href);
      this.showLinkPopup(quill);
    } else {
      quill.format('link', false);
    }
  }

  showLinkPopup(quill: any) {
    let param: ModalOptions = JSON.parse(JSON.stringify(this._modalConfig));
    let linkChanged: EventEmitter<Link>;
    param.class = 'link-selector-modal';
    param.initialState = {
      link: {}
    }
    if (this.bsLinkModalRef && this.bsLinkModalRef.content && this.bsLinkModalRef.content.linkChanged) {
      linkChanged = this.bsLinkModalRef.content.linkChanged as EventEmitter<Link>;
      linkChanged.unsubscribe();
    }

    this.bsLinkModalRef = this._modalService.show(EditLinkComponent, param), this._modalConfig;
    linkChanged = this.bsLinkModalRef.content.linkChanged as EventEmitter<Link>;
    linkChanged.subscribe(link => this.onQuillLinkChanged(quill, link));
    this.bsLinkModalRef.content.closeBtnName = 'Close';
  }

  showImagePopup(quill: any) {
    let param: ModalOptions = JSON.parse(JSON.stringify(this._modalConfig));
    let imageSelected: EventEmitter<Image>;
    param.class = 'image-selector-modal';
    param.initialState = {
      image: {}
    }
    if (this.bsImageModalRef && this.bsImageModalRef.content && this.bsImageModalRef.content.imageSelected) {
      imageSelected = this.bsImageModalRef.content.imageSelected as EventEmitter<any>;
      imageSelected.unsubscribe();
    }

    this.bsImageModalRef = this._modalService.show(ImageSelectorComponent, param), this._modalConfig;
    imageSelected = this.bsImageModalRef.content.imageSelected as EventEmitter<Image>;
    imageSelected.subscribe(image => this.onQuillImageSelected(image, quill));
    this.bsImageModalRef.content.closeBtnName = 'Close';


    // showImageManager().then(function (selectedImage) {
    //   vm.src = selectedImage.imageSource;
    //   vm.alt = selectedImage.imageAlt;
    //   vm.focusPoint = selectedImage.focusPoint;
    //   //setFocusPoint(vm.focusPoint);
    // }, function (response) {
    //   console.log(response);
    // });
  }

  onQuillLinkChanged(quill: any, link: Link) {
    if (link.linkType === LinkType.File || link.linkType === LinkType.Url) {
      quill.format('link', link.url);
    } else {
      this._pageService.getPage(link.pageId).subscribe((page: Page) => {
        let translation = page.pageTranslation.find(pt => pt.locale === this.pageContext.currentLocale);
        let url = translation.redirectUrl || `${this.pageContext.siteRoot}${translation.url}`;
        quill.format('link', url);
      });
    }
  }

  onQuillImageSelected(image: Image, quill: any) {
    const index = quill.getSelection() || {index: 0}
    let imageUrl = `${this.pageContext.siteRoot}${image.imageUrl}`
    quill.insertEmbed(index, 'image', imageUrl);
  }

  private init() {
    const pageContent$ = this._pageContentService.getPageContent(this.pageContentId);
    const siteLanguage$ = this._langaugeService.getSiteLanguages();
    forkJoin([pageContent$, siteLanguage$]).subscribe(results => {
      this.pageContent = results[0];
      this.contentTranslations = this.pageContent.pageContentTranslation;
      this.contentType = this.pageContent.contentType;
      this.languages = results[1];
      this._fields = this.pageContent.contentType.contentTypeFields.sort((a, b) => a.sortOrder > b.sortOrder ? 1 : -1);

      const currentCultureCode = this.pageContext.currentLocale;
      this.selectedLocale = this.languages.find(langauge => langauge.cultureCode === currentCultureCode);
      //load the current translation
      let translation = this.getTranslationForLocale(this.selectedLocale.cultureCode);
      this.contentTranslation = translation;
      this.deserializeContentTranslation();
    })
  }

  private getTranslationForLocale(locale) {
    let translation = this.contentTranslations.find(translation => translation.cultureCode === locale);
    if (!translation) {
      translation = {
        cultureCode: locale,
        contentData: {
          items: []
        },
        pageContentId: this.pageContent.id
      };
    }
    return translation;
  }

  private serializeContentTranslation() {
    //if (this.contentType.dataType && (this.contentType.dataType === 'array' || this.contentType.dataType === 'object')) {
    let contentData = this._coreService.prepareRequest(this.contentTranslation.contentData);
    this.contentTranslation.contentData = JSON.stringify(contentData);
    //}
  }

  private deserializeContentTranslation() {
    if ( /*this.contentType.dataType && (this.contentType.dataType === 'array' || this.contentType.dataType === 'object') &&*/
      typeof (this.contentTranslation.contentData) === 'string') {
      let contentData = JSON.parse(this.contentTranslation.contentData);
      this.contentTranslation.contentData = this._coreService.parseResponse(contentData); //if any date string is found, it parses back to date object
    }
  }

}

export enum ViewState {
  List = 'List',
  New = 'New',
  Edit = 'Edit'
}
