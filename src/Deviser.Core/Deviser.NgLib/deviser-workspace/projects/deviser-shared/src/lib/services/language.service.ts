import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { ContentType } from '../domain-types/content-type';
import { catchError, tap} from 'rxjs/operators';
import { Language } from '../domain-types/language';

@Injectable({
  providedIn: 'root'
})
export class LanguageService extends BaseService {

  getSiteLanguages(): Observable<Language[]> {
    const serviceUrl: string = `${this.baseUrl}api/language/site/`;
    return this.http.get<Language[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched Site Languages')),
        catchError(this.handleError('getSiteLanguages', null))
      );
  }
}
