import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { FileItem } from '../domain-types/file-item';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AssetService extends BaseService {

  getDocuments(): Observable<FileItem[]> {
    const serviceUrl: string = `${this.baseUrl}api/upload/documents/`;
    var obs = this.http.get<FileItem[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true });
    return obs;
  }

  getImages(): Observable<FileItem[]> {
    const serviceUrl: string = `${this.baseUrl}api/upload/images/`;
    return this.http.get<FileItem[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched images')),
        catchError(this.handleError('getImages', null))
      );
  }

  searchDocuments(term: string) {
    const serviceUrl: string = `${this.baseUrl}api/upload/documents?searchTerm=${term}`;
    return this.http.get<FileItem[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched images')),
        catchError(this.handleError('searchImages', null))
      );
  }

  searchImages(term: string) {
    const serviceUrl: string = `${this.baseUrl}api/upload/images?searchTerm=${term}`;
    return this.http.get<FileItem[]>(serviceUrl, { headers: this.httpHeaders, withCredentials: true })
      .pipe(
        tap(_ => this.log('fetched images')),
        catchError(this.handleError('searchImages', null))
      );
  }

  uploadDocuments(file: File) {
    const serviceUrl: string = `${this.baseUrl}api/upload/documents/`;
    const formData: FormData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(serviceUrl, formData, {
      reportProgress: true,
      observe: 'events'
    })
      .pipe(
        tap(_ => this.log('documents uploaded')),
        catchError(this.handleError('uploadDocuments', null))
      );
  }

  uploadImages(file: File) {
    const serviceUrl: string = `${this.baseUrl}api/upload/images/`;
    const formData: FormData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(serviceUrl, formData, {
      reportProgress: true,
      observe: 'events'
    })
      .pipe(
        tap(_ => this.log('images uploaded')),
        catchError(this.handleError('uploadImages', null))
      );
  }
}
