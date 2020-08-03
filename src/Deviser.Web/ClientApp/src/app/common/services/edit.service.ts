import { Injectable, Inject } from '@angular/core';
import { Page } from '../domain-types/page';
import { PageService } from './page.service';
import { Link } from '../domain-types/link';
import { LinkType } from '../domain-types/link-type';
import { Globals } from '../config/globals';
import { PageContext } from '../domain-types/page-context';
import { WINDOW } from './window.service';

@Injectable({
  providedIn: 'root'
})
export class EditService {

  pages: Page[];
  private _pageContext: PageContext;

  constructor(private _pageService: PageService,
    @Inject(WINDOW) _window: any) {
    this._pageContext = _window.pageContext;
    _pageService.getPages().subscribe(pages => this.pages = pages)
  }


  getLinkUrl(link: Link) {
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
}
