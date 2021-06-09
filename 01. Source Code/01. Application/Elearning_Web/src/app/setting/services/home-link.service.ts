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
export class HomeLinkService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchHomeLink(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-link/search', model, httpOptions);
    return tr
  }

  // Xóa home link
  deleteHomeLink(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'home-link/' + id, httpOptions);
    return tr
  }

  // Tạo mới home link
  createHomeLink(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-link', model, httpOptions);
    return tr
  }

  // Lấy thông tin home link
  getHomeLinkInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-link/' + id, httpOptions);
    return tr
  }

  // Cập nhật home link
  updateHomeLink(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'home-link/' + id, model, httpOptionsJson);
    return tr
  }
  //cập nhập trạng thái
  updateStatusHomeLink(id: string): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'home-link/update-status/' + id, httpOptions);
    return tr
  }
}
