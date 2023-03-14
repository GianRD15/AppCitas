import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, Observable, of } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseURL = environment.baseUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) {}

  getMembers(): Observable<Member[]> {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseURL + 'users').pipe(
      map((members) => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username: String): Observable<Member> {
    const member = this.members.find((x) => x.username === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseURL + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseURL + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member };
      })
    );
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseURL + 'users/delete-photo/' + photoId);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseURL + 'users/set-main-photo/' + photoId, {});
  }
}
