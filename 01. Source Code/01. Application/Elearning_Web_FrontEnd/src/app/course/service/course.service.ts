import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentFontEndComponent } from 'src/app/lesson/comment-font-end/comment-font-end.component';
import { Configuration } from 'src/app/share/config/configuration';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})

export class CourseService {
  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  tripDetailsarray:any = []

  data(item, i){
    item.index = i;
   this.tripDetailsarray =[];
   this.tripDetailsarray.push(item);
  }

  getCourseById(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'fontend/course',model,httpOptions
    );
    return tr;
  }
}
