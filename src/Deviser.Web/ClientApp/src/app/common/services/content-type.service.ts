import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { ContentType } from '../domain-types/content-type';
import { catchError, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ContentTypeService extends BaseService {

  getContentTypes(): Observable<ContentType[]> {
    const serviceUrl: string = `${this.baseUrl}/api/contenttype/`;
    return this.http.get<ContentType[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched contentTypes')),
        catchError(this.handleError('getContentTypes', null))
      );
  }

}
