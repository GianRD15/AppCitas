import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit{
[x: string]: any;
  members: Member[] | undefined;
  pagination:Pagination | undefined;
  userParams: UserParams | undefined;
  genderList =[{value:'male', display:'Males'},{value:'female', display:'Females'}];

  constructor(private memeberService:MembersService, private accountService: AccountService) {
    this.userParams = this.memeberService.getUserParams();
   }

  ngOnInit(){
   this.loadMembers();
  }

  loadMembers(){
    if(!this.userParams) return;
    this.memeberService.setUserParams(this.userParams);
    this.memeberService.getMembers(this.userParams).subscribe({
      next: response =>{
        if(response.result && response.pagination){
          this.members = response.result;
          this.pagination = response.pagination;
        }
      }
    });
  }

  resetFilters(){
    this.userParams = this.memeberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    if(this.userParams && this.userParams?.pageNumber != event.page){
      this.userParams.pageNumber = event.page;
      this.memeberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
}
