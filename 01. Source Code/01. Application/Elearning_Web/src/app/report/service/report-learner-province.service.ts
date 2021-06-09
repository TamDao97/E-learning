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
export class ReportLearnerProvinceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  reportLearnerProvince(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'report-learner-province/report-learner-province',model, httpOptions);
    return tr;
  }

  exportFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'report-learner-province/export-file', model, httpOptions);
    return tr;
  }
}
