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
export class QuestionsService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  // Tìm kiếm câu hỏi
  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'questions/search', model, httpOptions);
    return tr;
  }

  // Lấy danh sách chủ đề
  getTopicFull(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-topic-full', httpOptions);
    return tr
  }

  //Lấy chi tiết câu hỏi
  getQuestionById(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'questions/' + id, httpOptions);
    return tr
  }

  // Tạo mới câu hỏi
  create(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'questions', model, httpOptions);
    return tr
  }

  // Cập nhật câu hỏi
  update(model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'questions', model, httpOptionsJson);
    return tr
  }

  //Xóa câu hỏi
  delete(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'questions/' + id);
    return tr;
  }

  // Tạo chủ đề
  createtopic(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'topics', model, httpOptions);
    return tr
  }

  updateStatusQuestion(id: string): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'questions/update-status/' + id, httpOptions);
    return tr
  }

  requestQuestion(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'questions/request/' + id, model, httpOptions);
    return tr;
  }

  approvalQuestion(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'questions/approval/' + id, model, httpOptions);
    return tr;
  }

  approvalHistory(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'questions/approval-history/' + id, httpOptions);
    return tr;
  }
}
