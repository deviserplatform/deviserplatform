import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';


import { MessageService } from './message.service';
import { Pagination } from '../domain-types/pagination';
import { AdminConfig } from '../domain-types/admin-config';

@Injectable({
  providedIn: 'root'
})
export class AdminService {


  private baseUrl = 'https://localhost:44304/modules';
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      'Authorization': 'my-auth-token'
    })
  };

  constructor(private http: HttpClient,
    private messageService: MessageService) { }

  getAdminConfig(moduleName: string, entityName: string): Observable<AdminConfig> {
    const serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/meta`;
    return this.http.get<AdminConfig>(serviceUrl).pipe(
      tap(_ => this.log('fetched meta info')),
      catchError(this.handleError<AdminConfig>('getAdminConfig'))
    );
  }

  getAllRecords(moduleName: string, entityName: string, pagination: Pagination = null): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}`;


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

  getRecord(moduleName: string, entityName: string, id: string): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/${id}`;

    return this.http.get<any>(serviceUrl)
      .pipe(
        tap(_ => this.log(`fetched a record for id: ${id}`)),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  createRecord(moduleName: string, entityName: string, record: any) {
    const serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/`;
    return this.http.post<any>(serviceUrl, record, this.httpOptions)
    .pipe(
      tap(_ => this.log('created a record')),
      catchError(this.handleError('createRecord', null))
    );
  }

  updateRecord(moduleName: string, entityName: string, record: any) {
    const serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
    .pipe(
      tap(_ => this.log('updated a record')),
      catchError(this.handleError('updateRecord', null))
    );
  }

  deleteRecord(moduleName: string, entityName: string, id: string) {
    const serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/${id}`;

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
