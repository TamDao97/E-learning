import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class SystemConfigService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getListEducationProgram() {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-course/get-listeducationprogram', httpOptions);
    return tr;
  }

  save(listCourse: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'home-course', listCourse, httpOptions);
    return tr;
  }
}
