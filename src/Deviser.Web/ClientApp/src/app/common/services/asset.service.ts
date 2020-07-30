import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { FileItem } from '../domain-types/file-item';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AssetService extends BaseService {

  getImages(): Observable<FileItem[]> {
    const serviceUrl: string = `${this.baseUrl}api/upload/images/`;
    return this.http.get<FileItem[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched contentTypes')),
        catchError(this.handleError('getContentTypes', null))
      );
  }

  searchImages(term: string) {
    const serviceUrl: string = `${this.baseUrl}api/upload/images?search=${term}`;
    return this.http.get<FileItem[]>(serviceUrl, { headers: this.httpHeaders })
      .pipe(
        tap(_ => this.log('fetched contentTypes')),
        catchError(this.handleError('getContentTypes', null))
      );
  }

  upload(file: File) {
    const serviceUrl: string = `${this.baseUrl}api/upload/images/`;
    const formData: FormData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(serviceUrl, formData, {
      reportProgress: true,
      observe: 'events'
    })
      .pipe(
        tap(_ => this.log('fetched contentTypes')),
        catchError(this.handleError('getContentTypes', null))
      );
  }
}
