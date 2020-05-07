import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';


import { MessageService } from './message.service';
import { Pagination } from '../domain-types/pagination';
import { AdminConfig } from '../domain-types/admin-config';
import { WINDOW } from './window.service';
import { DAConfig } from '../domain-types/da-config';
import { FormType } from '../domain-types/form-type';
import { FilterNode } from '../domain-types/filter-node';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl;
  private httpOptions;

  private _daConfig: DAConfig;
  private _adminConfigSubject: BehaviorSubject<AdminConfig> = new BehaviorSubject<AdminConfig>(null);
  private _adminConfig: AdminConfig;
  private _adminConfig$: Observable<AdminConfig>;
  // private _adminConfigCache: AdminConfig;

  constructor(private http: HttpClient,
    private messageService: MessageService,
    @Inject(WINDOW) private window: any) {

    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'my-auth-token'
      })
    };

    this._daConfig = window.daConfig;

    if (this._daConfig.isEmbedded) {
      this.baseUrl = `${window.location.origin}/modules`;
    }
    else {
      this.baseUrl = `${this._daConfig.debugBaseUrl}/modules`;
    }
  }

  getAdminConfig(): Observable<AdminConfig> {
    if (this._adminConfig) {
      return of(this._adminConfig);
    }
    else if (this._adminConfig$) {
      return this._adminConfig$;
    }
    else {
      const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/meta`;
      this._adminConfig$ = this.http.get<AdminConfig>(serviceUrl).pipe(
        tap(next => {
          this._adminConfig = next;
          this.log('fetched meta info');
        }),
        catchError(this.handleError<AdminConfig>('getAdminConfig'))
      );
      return this._adminConfig$;
    }
  }

  getAllRecords(pagination: Pagination = null): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}`;


    let params = new HttpParams();

    if (pagination != null) {
      params = params.append('pageNo', pagination.pageNo.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }



    return this.http.get<any>(serviceUrl, { params })
      .pipe(
        tap(_ => this.log('fetched all records')),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  filterRecords(filterNode: FilterNode, orderBy: string, pagination: Pagination = null): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/filter`;
    let params = new HttpParams();

    if (pagination != null) {
      params = params.append('pageNo', pagination.pageNo.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
      if (orderBy) {
        params = params.append('orderBy', orderBy);
      }
    }



    return this.http.post<any>(`${serviceUrl}?${params.toString()}`, filterNode, this.httpOptions)
      .pipe(
        tap(_ => this.log('fetched all records')),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  getTree(): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/tree`;

    return this.http.get<any>(serviceUrl)
      .pipe(
        tap(_ => this.log('fetched all records')),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  getRecord(id: string): Observable<any> {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/${id}`;

    return this.http.get<any>(serviceUrl)
      .pipe(
        tap(_ => this.log(`fetched a record for id: ${id}`)),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  getLookUp(formType: FormType, formName: string, fieldName: string, filterParam: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/lookup/${formType}/field/${fieldName}`;
    return this.http.put<any>(serviceUrl, filterParam, this.httpOptions)
      .pipe(
        tap(_ => this.log(`fetched lookup`)),
        catchError(this.handleError('getAllRecords', null))
      );
  }

  createRecord(record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/`;
    return this.http.post<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }

  updateRecord(record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('updated a record')),
        catchError(this.handleError('updateRecord', null))
      );
  }

  updateTree(record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/tree/`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('tree has been updated ')),
        catchError(this.handleError('updateTree', null))
      );
  }

  executeGridAction(actionName: string, record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/grid/${actionName}`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('executing grid row action')),
        catchError(this.handleError('executeGridAction', null))
      );
  }

  executeMainFormAction(actionName: string, record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/mainform/${actionName}`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('executing main form action')),
        catchError(this.handleError('executeMainFormAction', null))
      );
  }

  customFormSubmit(formName: string, record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/form/${formName}`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('executing custom form submit')),
        catchError(this.handleError('executeCustomFormAction', null))
      );
  }

  executeCustomFormAction(formName: string, actionName: string, record: any) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/form/${formName}/action/${actionName}`;
    return this.http.put<any>(serviceUrl, record, this.httpOptions)
      .pipe(
        tap(_ => this.log('executing custom form action')),
        catchError(this.handleError('executeCustomFormAction', null))
      );
  }

  deleteRecord(id: string) {
    const serviceUrl: string = this.baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/${id}`;

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
