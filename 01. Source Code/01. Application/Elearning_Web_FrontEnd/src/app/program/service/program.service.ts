import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  getProgramById(id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/program/searchProgram', id, httpOptions);
    return tr;
  }

  createLearnerCourse(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/program/create-learner-course',model, httpOptions);
    return tr
  }

  getListProgram(id: string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/program/search/'+id, httpOptions);
    return tr;
  }

}
