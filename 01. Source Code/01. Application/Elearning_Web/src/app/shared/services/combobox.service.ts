import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from '..';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ComboboxService {

  constructor(private http: HttpClient,
    private config: Configuration) { }

  getListGroupuser(): Observable<any> {
    return this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/GetListGroupuser', httpOptions);
  }

  getCategory(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-category', httpOptions);
    return tr;
  }

  getCategoryParent(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-category-parent', httpOptions);
    return tr;
  }

  getProgram(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-program', httpOptions);
    return tr;
  }

  getTopic(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-topic', httpOptions);
    return tr;
  }

  getTopicFull(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-topic-full', httpOptions);
    return tr;
  }

  getEmployeeAsync(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/employees', httpOptions);
    return tr
  }

  getHomeSpecialistAsync(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/home-specialist', httpOptions);
    return tr
  }

  getListProvince(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-province', httpOptions);
    return tr
  }

  getListNation(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-nation', httpOptions);
    return tr
  }

  getListDistrictByProvinceId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-district/'+id, httpOptions);
    return tr
  }

  getListWardByDistrictId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-ward/'+id, httpOptions);
    return tr
  }

  getUser(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-user', httpOptions);
    return tr
  }

  getLearner(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/search-learner', httpOptions);
    return tr
  }

  getListManageUnit(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'combobox/get-list-manage-unit', httpOptions);
    return tr
  }
}
