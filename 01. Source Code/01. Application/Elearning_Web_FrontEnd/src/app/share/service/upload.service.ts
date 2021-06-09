import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Configuration } from '../config/configuration';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  // Upload 1 file
  uploadFile(file: any, folderName: string): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('folderName', folderName);
    formData.append('file', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'uploads/upload-file', formData);
    return tr
  }

  // Upload nhiều file
  uploadFiles(file: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model', JSON.stringify(model));
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'uploads/upload-files', formData);
    return tr
  }

  downloadFileZip(listfile): Observable<any> {
    var tr = this.http.post(this.config.ServerWithApiUrl + 'uploads/download-files', listfile, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      responseType: "blob"
    },);
    return tr
  }

  deleteFile(path: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'uploads/delete',path, httpOptions);
    return tr
  }

}
