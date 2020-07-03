import { Component, OnInit } from '@angular/core';
import {MessageService} from '../services/message.service';
import { ContactsServiceProxy, ContactsSearchCriteriaModel, ContactDto } from '../services/service-proxies';

@Component({
  selector: 'app-contacts-list',
  templateUrl: './contacts-list.component.html',
  styleUrls: ['./contacts-list.component.css']
})
export class ContactsListComponent implements OnInit {

  contacts: ContactDto[] = [];
  pageNumber = 1;
  pageSize = 3;
  text = '';
  total = 0;  
  filteredText = '';

  constructor(
      private _msgService: MessageService,
      private _contactsService: ContactsServiceProxy) { 
  }

  ngOnInit(): void {
    this.search();
  }

  remove(id: string) {
    this
      ._msgService
      .doQuestion('Are you sure?', "You won't be able to revert this!")
      .then(x => {
        if (!x.value) {
          return;
        }
        this
          ._contactsService
          .delete(id)
          .subscribe(x => {
            if (x.success) {
              this.search();
              this._msgService.showSuccess('Deleted', 'Your contact has been deleted');
            }
          },
          error => {
            this._msgService.showError('Error', `There was an error: ${error}`);
          });
        });      
  }

  search() {
    const criteria = new ContactsSearchCriteriaModel({ 
      pageNumber: this.pageNumber, 
      pageSize: this.pageSize, 
      text: this.text
    });

    this
      ._contactsService
      .search(criteria)
      .subscribe(x => {
        this.contacts = x.results;
        this.total = x.total;
      });
  }

  gotoPage(newPage: number) {
    if (!newPage) {
      return;
    }

    this.pageNumber = newPage;
    this.search();
  }

  filter() {
    this.text = this.filteredText.trim();
    console.log(this.text)
    this.search();
  }
}
