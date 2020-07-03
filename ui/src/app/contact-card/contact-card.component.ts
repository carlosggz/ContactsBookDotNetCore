import { Component, OnInit } from '@angular/core';
import {MessageService} from '../services/message.service';
import { ContactsServiceProxy, ContactsModel } from '../services/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import {PhoneTypeName} from '../shared/phonetype.enum';

@Component({
  selector: 'app-contact-card',
  templateUrl: './contact-card.component.html',
  styleUrls: ['./contact-card.component.css']
})
export class ContactCardComponent implements OnInit {

  contact: ContactsModel = new ContactsModel();
  id: string = null;
  phoneTypeDesc = PhoneTypeName;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private _msgService: MessageService,
    private _contactsService: ContactsServiceProxy) { 
}

  ngOnInit(): void {

    this.id = this.route.snapshot.params['id'];

    this
      ._contactsService
      .get(this.id)
      .subscribe(x => {
        this.contact = x;
      },
      error => {
        this._msgService.showError('Error', `There was an error: ${error}`);
        this.router.navigate(['/']);
      });
  }

}
