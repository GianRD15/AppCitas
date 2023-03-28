import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, Observable, of } from 'rxjs';
import { PaginationResult } from '../_models/pagination';
@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseURL = environment.baseUrl;
  members: Member[] = [];
  paginatedResult: PaginationResult<Member[]>=new PaginationResult<Member[]>;

  constructor(private http: HttpClient) {}

  getMembers(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();
    if(page && itemsPerPage){
      params = params.append('pageNumber',page);
      params = params.append('pageSize',itemsPerPage);
    }

    return this.http.get<Member[]>(this.baseURL + 'users' , {observe:'response',params}).pipe(
      map(response => {
        if(response.body){
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if(pagination){
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
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
