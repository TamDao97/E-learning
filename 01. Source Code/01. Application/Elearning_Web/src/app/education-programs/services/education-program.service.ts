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
export class EducationProgramService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  // Tìm kiếm chương trình đào tạo
  searchProgram(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'programs/search', model, httpOptions);
    return tr
  }

  // Xóa chương trình đào tạo
  deleteProgram(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'programs/' + id, httpOptions);
    return tr
  }

  // Tạo mới chương trình đào tạo
  createProgram(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'programs', model, httpOptions);
    return tr
  }

  // Lấy thông tin chương trình đào tạo
  getProgramInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'programs/' + id, httpOptions);
    return tr
  }

  // Cập nhật chương trình đào tạo
  updateProgram(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'programs/' + id, model, httpOptionsJson);
    return tr
  }
  //cập nhập trạng thái
  updateStatusProgram(id: string): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'programs/update-status/' + id, httpOptions);
    return tr
  }


}
