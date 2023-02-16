import { Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @Output() cancelRegister = new EventEmitter();
  model:any ={};
  
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void{}

  register(): void{
    this.accountService.register(this.model).subscribe({
      next: _ => this.cancel(),
      error: error => {
        this.toastr.error(error.error);
        console.error(error);
      } 
    })
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }
}
