import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../config/configuration';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class DownloadService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  downloadFile(model): Observable<any> {
    let apiPath = this.config.ServerWithApiUrl + 'uploads/download-file';
    var tr = this.http.post(apiPath, model, {
      responseType: "blob"
    });
    return tr
  }

  downloadFiles(model): Observable<any> {
    let apiPath = this.config.ServerWithApiUrl + 'uploads/download-files';
    var tr = this.http.post(apiPath, model, {
      responseType: "blob"
    });
    return tr
  }

}
