import { Injectable } from '@angular/core';
import swal, { SweetAlertResult } from'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor() { }

  doQuestion(title: string, question: string): Promise<SweetAlertResult<any>>{

    return swal.fire({
      title: title,
      text: question,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes'
    });   
  }

  showError(title: string, description: string) {
    swal.fire({
      icon: 'error',
      title: title,
      text: description
    });    
  }

  showSuccess(title: string, description: string) {
    swal.fire({
      icon: 'success',
      title: title,
      text: description
    });    
  }  
}
