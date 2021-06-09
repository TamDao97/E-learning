import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/share/config/configuration';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class TestService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  createTest(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams', model, httpOptions);
    return tr;
  }

  getTestInfo(id: string, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/' + id, model, httpOptions);
    return tr;
  }

  getListQuestionAnswer(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/getListQuestionAnswer', model, httpOptions);
    return tr;
  }

  getTestLessonFrameInfo(id: string, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/exam-frame/' + id, model, httpOptions);
    return tr;
  }

  finishTestLessonFrame(id: string, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/finish-lesson-frame/' + id, model, httpOptions);
    return tr;
  }
}
