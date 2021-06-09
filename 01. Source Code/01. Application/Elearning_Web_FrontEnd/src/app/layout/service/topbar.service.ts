import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/share/config/configuration';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class TopbarService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getHomeSetting(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'home-setting/get-home-setting', httpOptions);
    return tr
  }

  createuser(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/create',model, httpOptions);
    return tr
  }

  login(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/login', model, httpOptions);
    return tr
  }

  changepass(model, learnerid): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/change-pass/'+learnerid,model, httpOptions);
    return tr
  }

  resetpassword(model, learnerid): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/reset-password/'+learnerid,model, httpOptions);
    return tr
  }

  forgotpass(email): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/forgot-pass/'+email, httpOptions);
    return tr
  }

  getUserByid(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/'+id, httpOptions);
    return tr
  }

  logout(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/logout/'+id, httpOptions);
    return tr
  }

  updateUser(id, model): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/'+id,model, httpOptions);
    return tr
  }
  
  getFacebook(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/get-facebook', model, httpOptions);
    return tr
  }

  getGoogle(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/get-google',model, httpOptions);
    return tr
  }

  getListProvince(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/search-province', httpOptions);
    return tr
  }

  getListNation(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/search-nation', httpOptions);
    return tr
  }

  getListDistrictByProvinceId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/search-district/'+id, httpOptions);
    return tr
  }

  getListWardByDistrictId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'fontend/userlogins/search-ward/'+id, httpOptions);
    return tr
  }

}
