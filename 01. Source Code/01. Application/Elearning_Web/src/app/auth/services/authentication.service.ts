import { Inject, Injectable, Optional, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { REQUEST } from '@nguniversal/express-engine/tokens';
import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { Configuration } from '../../shared';
import { isPlatformServer } from '@angular/common';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private http: HttpClient, private config: Configuration) {

  }

  login(loginData: any): Observable<any> {
    return this.http.post<any>(this.config.ServerWithApiUrl + 'authen/login', loginData, httpOptionsJson);
  }

  ChangePassword(model): Observable<any> {
    return this.http.put<any>(this.config.ServerWithApiUrl + 'users/changepass', model, httpOptionsJson);
  }

  logout() {
    // remove user from local storage to log user out
    return this.http.put<any>(this.config.ServerWithApiUrl + 'authen/logout', httpOptionsJson);
  }

  /**
    * Handle Http operation that failed.
    * Let the app continue.
    * @param operation - name of the operation that failed
    * @param result - optional value to return as the observable result
  */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead


      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

}