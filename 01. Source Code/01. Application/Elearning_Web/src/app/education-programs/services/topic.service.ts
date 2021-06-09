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
export class TopicService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

   // Tìm kiếm chủ đề
   searchtopic(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'topics/search', model, httpOptions);
    return tr
  }

  // Xóa chủ đề
  deletetopic(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'topics/' + id, httpOptions);
    return tr
  }

  // Tạo mới chủ đề
  createtopic(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'topics', model, httpOptions);
    return tr
  }

  // Lấy thông tin chủ đề
  gettopicInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'topics/' + id, httpOptions);
    return tr
  }

  // Cập nhật chủ đề
  updatetopic(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'topics/' + id, model, httpOptionsJson);
    return tr
  }
}
