import { Component, Host, HostListener, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any){
    if(this.editForm?.dirty){
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;
  
  constructor(private accountService:AccountService, private toastr:ToastrService, private memberService: MembersService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user=> this.user =user
    })
  }

  ngOnInit(): void {
    this.loadMemeber();
  }

  loadMemeber(){
    if(!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member=> this.member = member
    })
  }
  
  updateMember(){
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ =>{
        this.toastr.success('Profile updated!');
        this.editForm?.reset(this.member);
      },
      error: _ =>{
        this.toastr.error('An error has occurred, try it later');
      } 
    })
    
  }
}
