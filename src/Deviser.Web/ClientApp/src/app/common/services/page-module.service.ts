import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { PageModule } from '../domain-types/page-module';

@Injectable({
  providedIn: 'root'
})
export class PageModuleService extends BaseService {

  getPageModules(pageId: string): Observable<PageModule[]> {
    const serviceUrl: string = `${this.baseUrl}/api/pagemodule/page/${pageId}`;
    return this.http.get<PageModule[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched pageModules')),
        catchError(this.handleError('getPageModules', null))
      );
  }

  getPageModule(pageModuleId: string): Observable<PageModule> {
    const serviceUrl: string = `${this.baseUrl}/api/pagemodule/${pageModuleId}`;
    return this.http.get<PageModule>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched pageModule')),
        catchError(this.handleError('getPageModule', null))
      );
  }

  deletePageModule(id: string) {
    const serviceUrl: string = `${this.baseUrl}/api/pagemodule/${id}`;
    return this.http.delete<any>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log(`deleted a pageModule id:${id}`)),
        catchError(this.handleError('deletePageModule', null))
      );
  }


  // updatePageContents
  updatePageModule(pageModule: PageModule) {
    const serviceUrl: string = `${this.baseUrl}/api/pagemodule/`;
    return this.http.put<any>(serviceUrl, pageModule, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated pageModule')),
        catchError(this.handleError('updatePageModule', null))
      );
  }

  updatePageModules(pageModules: PageModule[]) {
    const serviceUrl: string = `${this.baseUrl}/api/pagemodule/list`;
    return this.http.put<any>(serviceUrl, pageModules, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated pageModules')),
        catchError(this.handleError('updatePageModules', null))
      );
  }

  updatePageModulePermissions(pageModule: PageModule) {
    const serviceUrl: string = `${this.baseUrl}/api/pagemodule/permission`;
    return this.http.put<any>(serviceUrl, pageModule, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated pageModulePermissions')),
        catchError(this.handleError('updatePageModulePermissions', null))
      );
  }
}
