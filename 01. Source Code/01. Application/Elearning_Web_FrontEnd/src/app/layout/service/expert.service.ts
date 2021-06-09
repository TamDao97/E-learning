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
export class ExpertService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchExpert(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/expert/search', httpOptions);
    return tr
  }
}
