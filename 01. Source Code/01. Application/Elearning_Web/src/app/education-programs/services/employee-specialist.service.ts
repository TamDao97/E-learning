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
export class EmployeeSpecialistService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchEmployeeSpecialist(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'employee-specialists/search', httpOptions);
    return tr;
  }

  createEmployeeSpecialist(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'employee-specialists', model, httpOptions);
    return tr;
  }

  updateEmployeeSpecialist(id: number, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'employee-specialists/' + id, model, httpOptions);
    return tr;
  }

  deleteEmployeeSpecialist(id: number): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'employee-specialists/' + id);
    return tr;
  }

  getByIdEmployeeSpecialist(id: number): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'employee-specialists/' + id);
    return tr;
  }

  searchEmployee(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'employee-specialists/employee-search',model, httpOptions);
    return tr;
  }
}
