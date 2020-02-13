import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MessageService } from './message.service';
import { ValidationResult } from '../domain-types/validation-result';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  private baseUrl = 'https://localhost:44304/deviser/admin/validator';
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'my-auth-token'
    })
  };

  constructor(private http: HttpClient,
    private messageService: MessageService) { }


  validatePassword(password: string) {
    const serviceUrl: string = this.baseUrl + `/password/`;
    let userObj = { password: password };
    return this.http.post<ValidationResult>(serviceUrl, userObj, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }

  validateEmailExist(email: string) {
    const serviceUrl: string = this.baseUrl + `/emailexist/`;
    let userObj = { email: email };
    return this.http.post<ValidationResult>(serviceUrl, userObj, this.httpOptions)
      .pipe(
        tap(_ => this.log('created a record')),
        catchError(this.handleError('createRecord', null))
      );
  }

  validateUserExist(userName: string) {
    const serviceUrl: string = this.baseUrl + `/userexist/`;
    let userObj = { userName: userName };
    return this.http.post<ValidationResult>(serviceUrl, userObj, this.httpOptions)
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
