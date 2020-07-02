import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactsListComponent } from './contacts-list/contacts-list.component';
import { ContactEditComponent } from './contact-edit/contact-edit.component';
import { ContactCardComponent } from './contact-card/contact-card.component';


export const routes: Routes = [
  { path: '',  component: ContactsListComponent },
  { path: 'edit/:id',  component: ContactEditComponent },
  { path: 'add',   component: ContactEditComponent},
  { path: 'card/:id',   component: ContactCardComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
