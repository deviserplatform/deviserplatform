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


  private baseUrl = 'https://localhost:44304/modules'




  constructor(private http: HttpClient,
    private messageService: MessageService) { }

  getMetaInfo(moduleName: string, entityName: string): Observable<AdminConfig> {
    let serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/meta`;
    return this.http.get<AdminConfig>(serviceUrl).pipe(
        tap(_ => this.log('fetched meta info')),
        catchError(this.handleError<AdminConfig>('getMetaInfo'))
      );
  }

  getAllRecords(moduleName: string, entityName: string, pagination: Pagination = null): Observable<any> {
    let serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}`;


    let params = new HttpParams();

    if (pagination != null) {
      params = params.append('pageNo', pagination.pageNo.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }



    return this.http.get<any>(serviceUrl, { params: params })
      .pipe(
        tap(_ => this.log('fetched all records')),
        catchError(this.handleError('getAllRecords', []))
      );
  }

  getRecord(moduleName: string, entityName: string, id: string): Observable<any> {
    let serviceUrl: string = this.baseUrl + `/${moduleName}/api/${entityName}/${id}`;

    return this.http.get<any>(serviceUrl)
      .pipe(
        tap(_ => this.log('fetched all records')),
        catchError(this.handleError('getAllRecords', []))
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