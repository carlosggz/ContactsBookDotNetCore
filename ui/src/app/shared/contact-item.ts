import { PhoneType, ContactsModel, PhoneNumberModel } from '../services/service-proxies';
import { EmailItem } from './email-item';
import { PhoneItem } from './phone-item';
import { ValidatorsHelper } from './helper.validator';

export class ContactItem {
  constructor(
    private id: string = '',
    public firstName: string = '', 
    public lastName: string = '',
    private emails: EmailItem[] = [],
    private phones: PhoneItem[] = []
    ){      
    }

    public get Emails(): EmailItem[] {
      return [... this.emails];
    }

    public get Phones(): PhoneItem[] {
      return [... this.phones];
    }

    private getNextId = (data: EmailItem[] | PhoneItem[]): number => !data.length ? 1 : data[data.length-1].id+1;
    
    addAddress = () => this.emails.push({ id: this.getNextId(this.emails), email: ''}) ;
  
    removeAddress = (id: number) =>this.emails = this.emails.filter(x => x.id != id);
    
    addPhone = () => this.phones.push({ id: this.getNextId(this.phones), phoneType: 0, phoneNumber: ''}) ;
    
    removePhone = (id: number) => this.phones = this.phones.filter(x => x.id != id);

    public fillFromModel(model: ContactsModel): ContactItem {

      if (model == null) {
        return;
      }

      this.id = model.id,
      this.firstName = model.firstName;
      this.lastName = model.lastName;
      this.emails = [];
      this.phones = [];

      model.emailAddresses?.forEach( (x, index) => this.emails.push({id: index, email: x}));
      model.phoneNumbers?.forEach( (x, index) => this.phones.push({id: index, phoneType: x.phoneType, phoneNumber: x.phoneNumber}));
     
      return this;
    }

    public toModel(): ContactsModel {
      return new ContactsModel({
        id: this.id,
        firstName: this.firstName,
        lastName: this.lastName,
        emailAddresses: this.emails.map(x => x.email),
        phoneNumbers: this.phones.map(x => new PhoneNumberModel({ phoneType: this.convertToPhoneType(x.phoneType.toString()), phoneNumber: x.phoneNumber}))
      });
    }

    convertToPhoneType(s: string): PhoneType {

      //Due to the limitation on the nswag generator
      switch(Number.parseInt(s.toString())) {
        case 0:
          return 0;
        case 1:
          return 1;
        default:
          return 2;
      }
    }

    public validate() : string[] {

      let errors = [];

      if (ValidatorsHelper.isEmptyOrNull(this.firstName)) {
        errors.push('First name is required');
      }

      if (ValidatorsHelper.isEmptyOrNull(this.lastName)) {
        errors.push('Last name is required');
      }

      this.emails.forEach((x) => !ValidatorsHelper.isValidEmail(x.email) && errors.push(`Invalid email [${x.email}]`));
      this.phones.forEach((x) => !ValidatorsHelper.isValidPhone(x.phoneNumber) && errors.push(`Invalid phone [${x.phoneNumber}]`));

      return errors;
    }
}