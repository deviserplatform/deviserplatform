import { Injectable, Inject } from '@angular/core';
import { BaseService } from './base.service';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { WINDOW } from './window.service';
import { Page } from '../domain-types/page';

@Injectable({
  providedIn: 'root'
})
export class PageService extends BaseService {


  private _pages: Page[];
  private _pages$: Observable<Page[]>;

  getPages(): Observable<Page[]> {
    if (this._pages) {
      return of(this._pages);
    }
    else if (this._pages$) {
      return this._pages$;
    }
    else {
      const serviceUrl: string = this.baseUrl + `/api/page/list`;
      this._pages$ = this.http.get<Page[]>(serviceUrl, { headers: this.httpHeaders }).pipe(
        tap(next => {
          this._pages = next;
          this.log('fetched pages');
        }),
        catchError(this.handleError<Page[]>('getPages'))
      );
      return this._pages$;
    }
  }

  getPage(id: string): Observable<any> {
    const serviceUrl: string = `${this.baseUrl}/api/page/${id}`;

    return this.http.get<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`fetched a page for id: ${id}`)),
        catchError(this.handleError('getPage', null))
      );
  }

  draftPage(id: string): Observable<any> {
    const serviceUrl: string = `${this.baseUrl}/api/page/draft/${id}`;
    return this.http.put<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`current page has been changed to draft state, pageId: ${id}`)),
        catchError(this.handleError('draftPage', null))
      );
  }

  publishPage(id: string): Observable<any> {
    const serviceUrl: string = `${this.baseUrl}/api/page/publish/${id}`;
    return this.http.put<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`current page has been published, pageId: ${id}`)),
        catchError(this.handleError('publishPage', null))
      );
  }
}
