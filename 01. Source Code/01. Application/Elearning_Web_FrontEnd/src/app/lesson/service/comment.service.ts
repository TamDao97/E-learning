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
export class CommentService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchComment(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/search', model, httpOptions);
    return tr;
  }

  createComment(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments', model, httpOptions);
    return tr;
  }

  updateComment(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'comments/' + id, model, httpOptions);
    return tr;
  }

  deleteComment(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'comments/' + id);
    return tr;
  }

  getCommentInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'comments/' + id);
    return tr;
  }

  approvedComment(model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'comments/approved', model, httpOptions);
    return tr;
  }

  unapprovedComment(model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'comments/unapproved', model, httpOptions);
    return tr;
  }

  getCommentTotalNew(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'comments/get-total-new');
    return tr;
  }

  searchCommentFontEnd(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/search-font-end', model, httpOptions);
    return tr;
  }

  searchCommentFontEndCourse(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/search-comment-course', model, httpOptions);
    return tr;
  }

  getLessonCourse(slug: string, userId: string, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/lesson-course/' + slug + "?userId=" + userId, model, httpOptions);
    return tr;
  }

  savetemp(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/save-temp', model, httpOptions);
    return tr;
  }

  finishtest(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'exams/finish-test', model, httpOptions);
    return tr;
  }

  getLessonInfo(id: string, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/lesson/' + id, model, httpOptions);
    return tr;
  }

  createCommentFontEnd(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/font-end', model, httpOptions);
    return tr;
  }

  getLessonIdCourseSlug(slug: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'exams/get-lesson-id-by-slug?slug=' + slug);
    return tr;
  }

  getLessonFrameInfo(id: string, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/lesson-frame/' + id, model, httpOptions);
    return tr;
  }
}
