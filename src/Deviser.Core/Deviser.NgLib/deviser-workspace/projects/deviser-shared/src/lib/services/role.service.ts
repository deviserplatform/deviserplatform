import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { catchError, tap} from 'rxjs/operators';
import { Language } from '../domain-types/language';
import { Role } from '../domain-types/role';

@Injectable({
  providedIn: 'root'
})
export class RoleService  extends BaseService {

  getRoles(): Observable<Role[]> {
    const serviceUrl: string = `${this.baseUrl}api/role/`;
    return this.http.get<Role[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched roles')),
        catchError(this.handleError('getRoles', null))
      );
  }
}
