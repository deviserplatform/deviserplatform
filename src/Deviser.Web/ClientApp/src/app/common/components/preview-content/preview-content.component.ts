import { Component, OnInit, Input, Inject } from '@angular/core';
import { PageContent } from '../../domain-types/page-content';
import { WINDOW } from '../../services/window.service';
import { PageContext } from '../../domain-types/page-context';
import { PageContentTranslation } from '../../domain-types/page-content-translation';
import { ContentFieldType } from '../../domain-types/content-field-type';
import { ContentTypeField } from '../../domain-types/content-type-field';
import { Link } from '../../domain-types/link';
import { PageService } from '../../services/page.service';
import { Page } from '../../domain-types/page';
import { Globals } from '../../config/globals';
import { Image } from '../../domain-types/image';
import { DomSanitizer } from '@angular/platform-browser';
import { LinkType } from '../../domain-types/link-type';

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
    const contentTranslation = this.pageContent.pageContentTranslation.find(pct => pct.cultureCode === this._pageContext.currentLocale);
    let content = (contentTranslation && contentTranslation.contentData) ? JSON.parse(contentTranslation.contentData) : {};
    return content;
  }

  get fields(): ContentTypeField[] {
    return this._fields;
  }


  constructor(private _sanitizer: DomSanitizer,
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
    let link: Link = content;

    if (!link || !link.linkType) return '';

    if (link.linkType === LinkType.Url) {
      return link.url;
    } else if (link.linkType === LinkType.Page) {
      let page = this.pages.find(p => p.id === link.pageId);
      let translation = page.pageTranslation.find(pt => pt.locale === this._pageContext.currentLocale);
      translation = translation ? translation : page.pageTranslation[0];
      let url = page.pageTypeId === Globals.appSettings.pageTypes.url ? translation.uRL : `${this._pageContext.siteRoot}${translation.uRL}`;
      return url;
    }
  }

  getImageUrl(image: Image) {
    return image && image.imageUrl ? `${this._baseUrl}${image.imageUrl}` : null;
  }

}
