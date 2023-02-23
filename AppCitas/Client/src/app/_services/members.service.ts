import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseURL = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getMembers(): Observable<Member[]>{
    return this.http.get<Member[]>(this.baseURL + 'users',this.getHttpOptions());
  }

  getMember(username: String): Observable<Member>{
    return this.http.get<Member>(this.baseURL+'users/'+username, this.getHttpOptions())
  }

  getHttpOptions(): {headers: HttpHeaders;} | undefined {
    const userString = localStorage.getItem('user');
    if(!userString) return;

    const user = JSON.parse(userString);

    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer '+user.token
      })
    }
  }
}
