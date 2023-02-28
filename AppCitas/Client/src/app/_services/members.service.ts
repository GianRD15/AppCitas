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
    return this.http.get<Member[]>(this.baseURL + 'users');
  }

  getMember(username: String): Observable<Member>{
    return this.http.get<Member>(this.baseURL+'users/'+username);
  }
}
