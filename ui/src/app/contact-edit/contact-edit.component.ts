import { Component, OnInit } from '@angular/core';
import {MessageService} from '../services/message.service';
import { ContactsServiceProxy, ContactsModel } from '../services/service-proxies';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-contact-edit',
  templateUrl: './contact-edit.component.html',
  styleUrls: ['./contact-edit.component.css']
})
export class ContactEditComponent implements OnInit {

  id: string = null;

  constructor(
    private route: ActivatedRoute,
    private _msgService: MessageService,
    private _contactsService: ContactsServiceProxy) { 
}

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
  }

}
