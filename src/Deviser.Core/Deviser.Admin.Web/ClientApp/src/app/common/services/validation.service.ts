import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MessageService } from './message.service';
import { ValidationResult } from '../domain-types/validation-result';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { WINDOW } from './window.service';
import { DAConfig } from '../domain-types/da-config';
import { FormType } from '../domain-types/form-type';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  // private baseUrl = 'https://localhost:44304/deviser/admin/validator';
  // private httpOptions = {
  //   headers: new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     'Authorization': 'my-auth-token'
  //   })
  // };

  private _baseUrl;
  private _httpOptions;

  private _daConfig: DAConfig;

  constructor(private http: HttpClient,
    private messageService: MessageService,
    @Inject(WINDOW) private window: any) {
    this._daConfig = window.daConfig;
    this._httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'currentPageId': this._daConfig.currentPageId
      }),
      withCredentials: true
    };



    if (this._daConfig.isEmbedded) {
      this._baseUrl = `${window.location.origin}/modules`;
    }
    else {
      this._baseUrl = `${this._daConfig.debugBaseUrl}/modules`;
    }
  }


  validatePassword(password: string) {
    const serviceUrl: string = this._baseUrl + `/password/`;
    let userObj = { password: password };
    return this.http.put<ValidationResult>(serviceUrl, userObj, this._httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }

  validateEmailExist(email: string) {
    const serviceUrl: string = this._baseUrl + `/emailexist/`;
    let userObj = { email: email };
    return this.http.put<ValidationResult>(serviceUrl, userObj, this._httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }

  validateUserExist(userName: string) {
    const serviceUrl: string = this._baseUrl + `/userexist/`;
    let userObj = { userName: userName };
    return this.http.put<ValidationResult>(serviceUrl, userObj, this._httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }


  validateCustom(formType: FormType, formName: string, fieldName: string, fieldObject: any) {
    let serviceUrl: string;
    if (formType == FormType.MainForm) {
      serviceUrl = this._baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/validate/${formType}/fieldName/${fieldName}`;
    }
    else {
      serviceUrl = this._baseUrl + `/${this._daConfig.module}/api/${this._daConfig.model}/validate/${formType}/form/${formName}/fieldName/${fieldName}`;
    }
    // let postObj = { fieldObject: fieldObject }
    return this.http.put<ValidationResult>(serviceUrl, JSON.stringify(fieldObject), this._httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
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
