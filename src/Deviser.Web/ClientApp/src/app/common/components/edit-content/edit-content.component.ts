import { Component, OnInit, Inject, EventEmitter } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

import { BsModalRef } from 'ngx-bootstrap/modal';
import { PageContentService } from '../../services/page-content.service';
import { LanguageService } from '../../services/language.service';
import { forkJoin } from 'rxjs';
import { PageContext } from '../../domain-types/page-context';
import { WINDOW } from '../../services/window.service';
import { Language } from '../../domain-types/language';
import { PageContentTranslation } from '../../domain-types/page-content-translation';
import { ContentType } from '../../domain-types/content-type';
import { PageContent } from '../../domain-types/page-content';
import { CoreService } from '../../services/core.service';
import { ContentTranslationService } from '../../services/content-translation.service';
import { AlertService } from '../../services/alert.service';
import { AlertType } from '../../domain-types/alert';
import { Guid } from '../../services/guid';
import { ContentTypeField } from '../../domain-types/content-type-field';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { Image } from '../../domain-types/image';
import { Link } from '../../domain-types/link';
import { PageService } from '../../services/page.service';
import { Page } from '../../domain-types/page';
import { Globals } from '../../config/globals';

@Component({
  selector: 'app-edit-content',
  templateUrl: './edit-content.component.html',
  styleUrls: ['./edit-content.component.scss']
})
export class EditContentComponent implements OnInit {

  private _pageContentId: string;
  private _fields: ContentTypeField[];
  private _fieldValues: { [fieldName: string]: any };
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
  contentSaved: EventEmitter<any> = new EventEmitter();
  viewStates = ViewState;

  Editor = ClassicEditor;



  get isList(): boolean {
    return this.pageContent && this.pageContent.contentType && this.pageContent.contentType.isList;
  }

  get content(): any {
    // const contentTranslation = this.pageContent.pageContentTranslation.find(pct => pct.cultureCode === this.pageContext.currentLocale);
    // let content = (contentTranslation && contentTranslation.contentData) ? JSON.parse(contentTranslation.contentData) : {};
    // return content;
    return this.contentTranslation.contentData;
  }

  get fields(): ContentTypeField[] {
    return this._fields;
  }

  constructor(private _alertService: AlertService,
    public bsModalRef: BsModalRef,
    private _coreService: CoreService,
    private _contentTranslationService: ContentTranslationService,
    private _pageContentService: PageContentService,
    private _langaugeService: LanguageService,
    private _pageService: PageService,
    @Inject(WINDOW) window: any) {
    this.pageContext = window.pageContext;
    this.viewState = ViewState.List;
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
  }

  ngOnInit(): void {
  }

  save() {
    if (this.contentTranslation.id) {
      this.serializeContentTranslation();
      this._contentTranslationService.updatePageContentTranslation(this.contentTranslation).subscribe(data => {
        data.contentData = JSON.parse(data.contentData);
        console.log(data);
        this.contentSaved.emit(data);
        this.bsModalRef.hide();
      }, error => this._alertService.showMessage(AlertType.Error, 'Unable to save content'));
    }
    else {
      this.contentTranslation.pageContentId = this.pageContentId;
      this.serializeContentTranslation();
      this._contentTranslationService.createPageContentTranslation(this.contentTranslation).subscribe(data => {
        data.contentData = JSON.parse(data.contentData);
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

  getLinkUrl(content: any) {
    let link: Link = content;

    if (!link || !link.linkType) return '';

    if (link.linkType === 'URL') {
      return link.url;
    } else if (link.linkType === 'PAGE') {
      let page = this.pages.find(p => p.id === link.pageId);
      let translation = page.pageTranslation.find(pt => pt.locale === this.pageContext.currentLocale);
      translation = translation ? translation : page.pageTranslation[0];
      let url = page.pageTypeId === Globals.appSettings.pageTypes.url ? translation.uRL : `${this.pageContext.siteRoot}${translation.uRL}`;
      return url;
    }
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
      //load correct translation
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
