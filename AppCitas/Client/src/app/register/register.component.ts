import { Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model:any ={};
  
  constructor() {}
  ngOnInit(): void{}

  register(): void{
    console.log(this.model);
  }
  cancel(): void {
    this.cancelRegister.emit(false);
  }
}
