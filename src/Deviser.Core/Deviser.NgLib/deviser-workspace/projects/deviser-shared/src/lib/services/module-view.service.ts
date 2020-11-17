import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { ContentType } from '../domain-types/content-type';
import { catchError, tap} from 'rxjs/operators';
import { ModuleView } from '../domain-types/module-view';

@Injectable({
  providedIn: 'root'
})
export class ModuleViewService extends BaseService {

  getModuleViews(): Observable<ModuleView[]> {
    const serviceUrl: string = `${this.baseUrl}api/moduleview/`;
    return this.http.get<ModuleView[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched moduleViews')),
        catchError(this.handleError('getModuleViews', null))
      );
  }

  getEditActions(id: string): Observable<ModuleView[]> {
    const serviceUrl: string = `${this.baseUrl}api/moduleview/edit/${id}`;
    return this.http.get<ModuleView[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched moduleViews')),
        catchError(this.handleError('getModuleViews', null))
      );
  }
}
