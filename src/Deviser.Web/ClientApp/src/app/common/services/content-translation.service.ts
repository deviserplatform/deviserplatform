import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { BaseService } from './base.service';
import { PageLayout } from '../domain-types/page-layout';
import { PageContentTranslation } from '../domain-types/page-content-translation';

@Injectable({
  providedIn: 'root'
})
export class ContentTranslationService extends BaseService {

  createPageContentTranslation(pageContentTranslation: PageContentTranslation) {
    const serviceUrl: string = `${this.baseUrl}/api/contenttranslation/`;
    return this.http.post<any>(serviceUrl, pageContentTranslation, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a pageContentTranslation')),
        catchError(this.handleError('createPageContentTranslation', null))
      );
  }

  updatePageContentTranslation(pageContentTranslation: PageContentTranslation) {
    const serviceUrl: string = `${this.baseUrl}/api/contenttranslation/`;
    return this.http.put<PageLayout>(serviceUrl, pageContentTranslation, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated a pageContentTranslation')),
        catchError(this.handleError('updatePageContentTranslation', null))
      );
  }

}
