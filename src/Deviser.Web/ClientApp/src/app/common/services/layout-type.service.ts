import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { LayoutType } from '../domain-types/layout-type';

@Injectable({
  providedIn: 'root'
})
export class LayoutTypeService extends BaseService {

  getLayoutTypes(): Observable<LayoutType[]> {
    const serviceUrl: string = `${this.baseUrl}/api/layouttype/`;
    return this.http.get<LayoutType[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched layouts')),
        catchError(this.handleError('getLaouts', null))
      );
  }
}
