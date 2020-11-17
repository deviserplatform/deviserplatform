import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { MessageService } from './message.service';
import { WINDOW } from './window.service';
import { PageContext } from '../domain-types/page-context';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BaseService {
  protected readonly baseUrl;
  protected httpOptions;
  protected httpHeaders;
  protected pageContext: PageContext;

  constructor(protected http: HttpClient,
    private _messageService: MessageService,
    @Inject(WINDOW) private _window: any) {
    this.pageContext = _window.pageContext;

    this.httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'currentPageId': this.pageContext.currentPageId
      // 'Authorization': 'my-auth-token'
    });
    this.httpOptions = {
      headers: this.httpHeaders, withCredentials: true
    };


    if (this.pageContext.isEmbedded) {
      this.baseUrl = this.pageContext.siteRoot;
    }
    else {
      this.baseUrl = this.pageContext.debugBaseUrl;
    }
  }

  /** Log a AdminService message with the MessageService */
  protected log(message: string) {
    this._messageService.add(`AdminService: ${message}`);
  }

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  protected handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
