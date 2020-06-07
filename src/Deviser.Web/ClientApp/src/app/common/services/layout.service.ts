import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { MessageService } from './message.service';
import { WINDOW } from './window.service';
import { PageContext } from '../domain-types/page-context';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { BaseService } from './base.service';
import { PageLayout } from '../domain-types/page-layout';

@Injectable({
  providedIn: 'root'
})
export class LayoutService extends BaseService {

  getLayouts(): Observable<PageLayout[]> {
    const serviceUrl: string = `${this.baseUrl}/api/layout/`;
    return this.http.get<PageLayout[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched layouts')),
        catchError(this.handleError('getLayouts', null))
      );
  }

  getLayout(id: string): Observable<PageLayout> {
    const serviceUrl: string = `${this.baseUrl}/api/layout/${id}`;
    return this.http.get<PageLayout>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched layouts')),
        catchError(this.handleError('getLayout', null))
      );
  }

  createLayout(pageLayout: PageLayout) {
    const serviceUrl: string = `${this.baseUrl}/api/layout/`;
    return this.http.post<PageLayout>(serviceUrl, pageLayout, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createLayout', null))
      );
  }

  updateLayout(pageLayout: PageLayout) {
    const serviceUrl: string = `${this.baseUrl}/api/layout/`;
    return this.http.put<PageLayout>(serviceUrl, pageLayout, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('updateLayout', null))
      );
  }


  deleteLayout(id: string) {
    const serviceUrl: string = `${this.baseUrl}/api/layout/${id}`;
    return this.http.delete<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`deleted a record id:${id}`)),
        catchError(this.handleError('deleteLayout', null))
      );
  }
}
