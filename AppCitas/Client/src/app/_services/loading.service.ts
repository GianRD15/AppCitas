import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  loadingRequestCount = 0;

  constructor(private spinnerServie: NgxSpinnerService) { }

  loading(){
    this.loadingRequestCount++;
    this.spinnerServie.show(undefined,{
      type: 'square-jelly-box',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    })
  }

  idle(){
    this.loadingRequestCount--;
    if(this.loadingRequestCount<=0){
      this.loadingRequestCount=0;
      this.spinnerServie.hide();
    }
  }
}
