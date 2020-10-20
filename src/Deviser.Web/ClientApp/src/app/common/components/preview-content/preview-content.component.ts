import { Component, OnInit, Input, Inject } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ContentTypeField, Image, Page, PageContent, PageContext, WINDOW } from 'deviser-shared';
import { EditService, PageService } from 'deviser-shared';

@Component({
  selector: 'app-preview-content',
  templateUrl: './preview-content.component.html',
  styleUrls: ['./preview-content.component.scss']
})
export class PreviewContentComponent implements OnInit {


  _pageContext: PageContext;
  pages: Page[];

  private _baseUrl: string;
  private _fields: ContentTypeField[];
  private _pageContent: PageContent

  get pageContent(): PageContent {
    return this._pageContent;
  }

  @Input() set pageContent(value: PageContent) {
    this._pageContent = value;
    this._fields = this.pageContent.contentType.contentTypeFields.sort((a, b) => a.sortOrder > b.sortOrder ? 1 : -1);
  }

  get isList(): boolean {
    return this.pageContent.contentType && this.pageContent.contentType.isList;
  }

  get content(): any {
    const contentTranslation = this.pageContent.pageContentTranslation.find(pct => pct && pct.cultureCode === this._pageContext.siteLanguage);
    let content = (contentTranslation && contentTranslation.contentData) ? JSON.parse(contentTranslation.contentData) : {};
    return content;
  }

  get fields(): ContentTypeField[] {
    return this._fields;
  }


  constructor(private _editService: EditService,
    private _sanitizer: DomSanitizer,
    private _pageService: PageService,
    @Inject(WINDOW) private _window: any) {
    this._pageContext = _window.pageContext;
    if (this._pageContext.isEmbedded) {
      this._baseUrl = this._pageContext.siteRoot;
    }
    else {
      this._baseUrl = this._pageContext.debugBaseUrl;
    }

    _pageService.getPages().subscribe(pages => this.pages = pages)
  }

  ngOnInit(): void {
  }

  getLinkUrl(content: any) {
    return this._editService.getLinkUrl(content);
  }

  getImageUrl(image: Image) {
    return image && image.imageUrl ? `${this._baseUrl}${image.imageUrl}` : null;
  }

}
