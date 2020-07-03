import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ContactsListComponent } from './contacts-list/contacts-list.component';
import { ContactEditComponent } from './contact-edit/contact-edit.component';
import { ContactCardComponent } from './contact-card/contact-card.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {MessageService} from './services/message.service';

import {ContactsServiceProxy} from './services/service-proxies';
import {API_BASE_URL} from './services/service-proxies';
import { environment } from 'src/environments/environment';
import { HttpClientModule } from '@angular/common/http';
import { BackToListComponent } from './back-to-list/back-to-list.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    ContactsListComponent,
    ContactEditComponent,
    ContactCardComponent,
    BackToListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    { provide: API_BASE_URL, useFactory: () => environment.apiUrl },
    MessageService,
    ContactsServiceProxy
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
