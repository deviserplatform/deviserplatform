import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { PageContent } from '../domain-types/page-content';

@Injectable({
  providedIn: 'root'
})
export class PageContentService extends BaseService {

  getPageContents(pageId: string, cultureCode: string): Observable<PageContent[]> {
    const serviceUrl: string = `${this.baseUrl}api/pagecontent/${cultureCode}/${pageId}`;
    return this.http.get<PageContent[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched pageContents')),
        catchError(this.handleError('getPageContents', null))
      );
  }

  getPageContent(pageContentId: string): Observable<PageContent> {
    const serviceUrl: string = `${this.baseUrl}api/pagecontent/${pageContentId}`;
    return this.http.get<PageContent>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched pageContents')),
        catchError(this.handleError('getPageContents', null))
      );
  }

  deletePageContent(id: string) {
    const serviceUrl: string = `${this.baseUrl}api/pagecontent/${id}`;
    return this.http.delete<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`deleted a pageContent id:${id}`)),
        catchError(this.handleError('deletePageContent', null))
      );
  }

  updatePageContent(pageContent: PageContent) {
    const serviceUrl: string = `${this.baseUrl}api/pagecontent`;
    return this.http.put<any>(serviceUrl, pageContent, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated pageContent')),
        catchError(this.handleError('updatePageContent', null))
      );
  }

  updatePageContents(pageContents: PageContent[]) {
    const serviceUrl: string = `${this.baseUrl}api/pagecontent/list`;
    return this.http.put<any>(serviceUrl, pageContents, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated pageContents')),
        catchError(this.handleError('updatePageContents', null))
      );
  }

  updatePageContentPermissions(pageContent: PageContent) {
    const serviceUrl: string = `${this.baseUrl}api/pagecontent/permission`;
    return this.http.put<any>(serviceUrl, pageContent, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated pageContentPermissions')),
        catchError(this.handleError('updatePageContentPermissions', null))
      );
  }
}
