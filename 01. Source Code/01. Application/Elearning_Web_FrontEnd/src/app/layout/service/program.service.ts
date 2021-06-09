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
export class ProgramService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchCourse(learnerId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/program/search?learnerId='+learnerId,httpOptions);
    return tr
  }
  getTop2Course(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/program/gettop2Course', httpOptions);
    return tr
  }
  createLearnerCourse(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/program/create-learner-course',model, httpOptions);
    return tr
  }
}
