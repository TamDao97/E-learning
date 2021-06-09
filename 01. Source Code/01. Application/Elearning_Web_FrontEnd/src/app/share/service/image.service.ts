import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ImageService {

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
  uploadFiles(files: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('folderName', model);
    files.forEach(file => {
      formData.append('files', file);
    });
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
}
