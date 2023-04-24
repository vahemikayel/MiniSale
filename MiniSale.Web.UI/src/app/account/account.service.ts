import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUser } from './../shared/models/IUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl: string = environment.apiUrl;
  baseApiVersion: string = environment.apiVersion;
  baseUrlWithVersion = this.baseUrl + this.baseApiVersion;
  baseAccountUrl = this.baseUrlWithVersion + 'account/';

  private currentUserSource = new BehaviorSubject<boolean>(false);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {

    const token = localStorage.getItem('token') || '';
    const isAuthorized = token && token.length > 0 ? true : false;
    this.currentUserSource.next(isAuthorized);

  }


  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource?.next(null as any);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', 'Bearer ' + token);
    return this.http.get<IUser>(this.baseAccountUrl, { 'headers': headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource?.next(true);
        } else {
          this.currentUserSource?.next(false);
        }
      }));
  }

  login(values: any) {
    return this.http.post<IUser>(this.baseAccountUrl + 'login', values)
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource?.next(true);
          }
        }
        )
      );
  }

  register(values: any) {
    return this.http.post<IUser>(this.baseAccountUrl + 'register', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource?.next(true);
        } else {
          this.currentUserSource?.next(false);
        }
      }));
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(false);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseAccountUrl + 'emailexists?email=' + email);
  }
}
