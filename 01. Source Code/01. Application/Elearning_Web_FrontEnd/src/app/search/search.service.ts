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
export class SearchService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchCourse(learnerId, searchValue): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/course/searchCourse?learnerId='+learnerId+'&searchValue='+searchValue, httpOptions);
    return tr
  }
  getAllHomeLink(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-services/getAllHomeLink', httpOptions);
    return tr
  }
}
