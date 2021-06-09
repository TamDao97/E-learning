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
export class MyCourseService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  getMyCourse(learerId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'my-course/getMyCourse/'+learerId, httpOptions);
    return tr
  }
}
