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
}
