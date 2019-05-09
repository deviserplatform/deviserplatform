import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';


import { MessageService } from './message.service';
import { Pagination } from '../domain-types/pagination';
import { AdminConfig } from '../domain-types/admin-config';
import { WINDOW } from './window.service';
import { DAConfig } from '../domain-types/da-config';

@Injectable({
  providedIn: 'root'
})
export class AdminService {


  private baseUrl;
  private httpOptions;

  private daConfig: DAConfig;

  constructor(private http: HttpClient,
    private messageService: MessageService,
    @Inject(WINDOW) private window: any) {

    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'my-auth-token'
      })
    };

    this.daConfig = window.daConfig;

    if (this.daConfig.isEmbedded) {
      this.baseUrl = `${window.location.origin}/modules`;
    }
    else {
      this.baseUrl = `${this.daConfig.debugBaseUrl}/modules`;
    }
  }

  getAdminConfig(): Observable<AdminConfig> {
    const serviceUrl: string = this.baseUrl + `/${this.daConfig.module}/api/${this.daConfig.entity}/meta`;
    return this.http.get<AdminConfig>(serviceUrl).pipe(
      tap(_ => this.log('fetched meta info')),
      catchError(this.handleError<AdminConfig>('getAdminConfig'))
    );
  }

  getAllRecords(pagination: Pagination = null): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${this.daConfig.module}/api/${this.daConfig.entity}`;


    let params = new HttpParams();

    if (pagination != null) {
      params = params.append('pageNo', pagination.pageNo.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }



    return this.http.get<any>(serviceUrl, { params: params })
      .pipe(
        tap(_ => this.log('fetched all records')),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  getRecord(id: string): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${this.daConfig.module}/api/${this.daConfig.entity}/${id}`;

    return this.http.get<any>(serviceUrl)
      .pipe(
        tap(_ => this.log(`fetched a record for id: ${id}`)),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  createRecord(record: any) {
    const serviceUrl: string = this.baseUrl + `/${this.daConfig.module}/api/${this.daConfig.entity}/`;
    return this.http.post<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }

  updateRecord(record: any) {
    const serviceUrl: string = this.baseUrl + `/${this.daConfig.module}/api/${this.daConfig.entity}/`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated a record')),
        catchError(this.handleError('updateRecord', null))
      );
  }

  deleteRecord(id: string) {
    const serviceUrl: string = this.baseUrl + `/${this.daConfig.module}/api/${this.daConfig.entity}/${id}`;

    return this.http.delete<any>(serviceUrl)
      .pipe(
        tap(_ => this.log(`deleted a record id:${id}`)),
        catchError(this.handleError('deleteRecord', null))
      );
  }



  /** Log a AdminService message with the MessageService */
  private log(message: string) {
    this.messageService.add(`AdminService: ${message}`);
  }

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  private handleError<T>(operation = 'operation', result?: T) {
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
