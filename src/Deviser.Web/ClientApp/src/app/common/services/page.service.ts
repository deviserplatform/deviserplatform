import { Injectable, Inject } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { WINDOW } from './window.service';

@Injectable({
  providedIn: 'root'
})
export class PageService extends BaseService {

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
    return this.http.get<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`current page has been changed to draft state, pageId: ${id}`)),
        catchError(this.handleError('draftPage', null))
      );
  }

  publishPage(id: string): Observable<any> {
    const serviceUrl: string = `${this.baseUrl}/api/page/publish/${id}`;
    return this.http.get<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`current page has been published, pageId: ${id}`)),
        catchError(this.handleError('publishPage', null))
      );
  }
}
