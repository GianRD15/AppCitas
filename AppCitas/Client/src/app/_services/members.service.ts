import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, Observable, of } from 'rxjs';
import { PaginationResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseURL = environment.baseUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) {}

  getMembers(userParams: UserParams) {
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    
    params = params.append('minAge',userParams.minAge);
    params = params.append('maxAge',userParams.maxAge);
    params = params.append('gender',userParams.gender);
    params = params.append('orderBy',userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseURL+'users' ,params);
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    
    const paginatedResult: PaginationResult<T>=new PaginationResult<T>;

    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    
    return params;
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
