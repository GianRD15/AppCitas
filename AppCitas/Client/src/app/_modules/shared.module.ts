import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@NgModule({
  declarations: [],
  imports: [
    CommonModule, 
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot(
      {
        timeOut: 10000,
        positionClass: 'toast-top-center',
        preventDuplicates: true,
      }
    )
  ], 
  exports:[
    BsDropdownModule,
    ToastrModule
  ]
})
export class SharedModule { }
