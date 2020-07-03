import { Component, OnInit } from '@angular/core';
import { MessageService } from '../services/message.service';
import { ContactsServiceProxy, ApiContactResultModel } from '../services/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { PhoneTypeName } from '../shared/phonetype.enum';
import { ContactItem } from '../shared/contact-item';
import { ThrowStmt } from '@angular/compiler';
import { ContactsListComponent } from '../contacts-list/contacts-list.component';

@Component({
  selector: 'app-contact-edit',
  templateUrl: './contact-edit.component.html',
  styleUrls: ['./contact-edit.component.css']
})
export class ContactEditComponent implements OnInit {

  id: string = null;
  phoneTypeDesc = PhoneTypeName;  
  contact: ContactItem = new ContactItem();
  validations: string[] = [];
  
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private _msgService: MessageService,
    private _contactsService: ContactsServiceProxy) { 
}

  ngOnInit(): void {
  
    this.id = this.route.snapshot.params['id']; 

    if (this.id) {
      this.loadContact();     
    }
  }

  loadContact() {
    this._contactsService.get(this.id).subscribe(
      x => this.contact.fillFromModel(x),
      error => {
        this._msgService.showError('Error', `There was an error: ${error}`);
        this.backToHome();
      }); 
  }

  save() {
    this.validations = this.contact.validate();

    if (this.validations.length) {
      return;
    }

    if (!this.id) {
      this.add();
    }
    else {
      this.update();
    }
  }

  add() {
    console.log(this.contact.toModel());
    this._contactsService.add(this.contact.toModel()).subscribe(
      (x) => this.checkResult(x, 'Your contact was successfully added'),
      (error) => (error) => this.checkError(error)
    );
  }

  update() {
    console.log(this.contact.toModel());
    this._contactsService.update(this.contact.toModel()).subscribe(
      (x) => this.checkResult(x, 'Your contact was successfully updated'),
      (error) => this.checkError(error)
    );    
  }

  checkResult(apiResult: ApiContactResultModel, message: string) {
    if (apiResult.success) {
      this._msgService.showSuccess('Success', message);
      this.backToHome();
    } else if (apiResult.errors?.length) {          
      this._msgService.showError('Error', 'The operation failed, please check the errors');
      this.validations = apiResult.errors;
    }
  }

  checkError(error) {
    console.log(error);
    this._msgService.showError('Error', `There was an error: ${error}`)
  }

  backToHome() {
    this.router.navigate(['/']);
  }
}
