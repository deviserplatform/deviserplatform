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
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-preview-content',
  templateUrl: './preview-content.component.html',
  styleUrls: ['./preview-content.component.scss']
})
export class PreviewContentComponent implements OnInit {

  private _pageContent: PageContent
  pageContext: PageContext;
  pages: Page[];
  private _fields: ContentTypeField[];

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
    const contentTranslation = this.pageContent.pageContentTranslation.find(pct => pct.cultureCode === this.pageContext.currentLocale);
    let content = (contentTranslation && contentTranslation.contentData) ? JSON.parse(contentTranslation.contentData) : {};
    return content;
  }

  get fields(): ContentTypeField[] {
    return this._fields;
  }


  constructor(private _sanitizer: DomSanitizer,
    private _pageService: PageService,
    @Inject(WINDOW) private _window: any) {
    this.pageContext = _window.pageContext;

    _pageService.getPages().subscribe(pages => this.pages = pages)
  }

  ngOnInit(): void {
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

}
