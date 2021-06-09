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

  deleteComment(id: number): Observable<any> {
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

  getLessonInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'comments/lesson/' + id);
    return tr;
  }

  createCommentFontEnd(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'comments/font-end', model, httpOptions);
    return tr;
  }
}
