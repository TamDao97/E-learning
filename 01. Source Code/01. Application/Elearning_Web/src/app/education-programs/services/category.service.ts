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
export class CategoryService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchCategory(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'categorys/search', model, httpOptions);
    return tr;
  }

  createCategory(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'categorys', model, httpOptions);
    return tr;
  }

  updateCategory(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'categorys/' + id, model, httpOptions);
    return tr;
  }

  deleteCategory(id: string): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'categorys/' + id);
    return tr;
  }

  getCategoryInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'categorys/' + id);
    return tr;
  }
}
