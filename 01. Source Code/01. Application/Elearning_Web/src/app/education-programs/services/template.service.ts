import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class TemplateService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }


  search(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'file-template/search', httpOptions);
    return tr
  }

  create(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'file-template/create', model, httpOptions);
    return tr
  }

  update(model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'file-template/update', model, httpOptions);
    return tr
  }

  getById(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'file-template/getById/' + id, httpOptions);
    return tr
  }

  delete(id): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'file-template/delete/' + id, httpOptions);
    return tr
  }

  download(model: any): Observable<any> {
    var tr = this.http.post(this.config.ServerWithApiUrl + 'uploads/download-file', model, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      responseType: "blob"
    });
    return tr
  }
}
