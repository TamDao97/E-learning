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
export class ReportLearnerService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  reportLearner(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'report-learner/report-learner', model, httpOptions);
    return tr;
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'report-learner/export-excel', model, httpOptions);
    return tr;
  }

  exportPdf(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'report-learner/export-pdf', model, httpOptions);
    return tr;
  }
}
