import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class LessonService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchLesson(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'lessons/search', model, httpOptions);
    return tr;
  }

  searchQuestion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'lessons/search-question', model, httpOptions);
    return tr;
  }

  createLesson(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'lessons', model, httpOptions);
    return tr;
  }

  updateLesson(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'lessons/' + id, model, httpOptions);
    return tr;
  }

  updateStatus(id: string): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'lessons/status/' + id, httpOptions);
    return tr;
  }

  deleteLesson(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'lessons/' + id);
    return tr;
  }

  getLessonInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'lessons/' + id);
    return tr;
  }

  getQuestionRandom(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'lessons/get-question-random', model, httpOptions);
    return tr;
  }

  requestLesson(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'lessons/request/' + id, model, httpOptions);
    return tr;
  }

  approvalLesson(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'lessons/approval/' + id, model, httpOptions);
    return tr;
  }

  approvalHistory(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'lessons/approval-history/' + id, httpOptions);
    return tr;
  }
}
