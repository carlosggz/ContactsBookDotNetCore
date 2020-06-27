using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF.Models
{
    public class ContactModel
    {
        public ContactModel()
        {
            Emails = new HashSet<ContactEmailModel>();
            Phones = new HashSet<ContactPhoneModel>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<ContactEmailModel> Emails { get; set; }
        public virtual ICollection<ContactPhoneModel> Phones { get; set; }

        public ContactEntity ToEntity()
        {
            var entity = new ContactEntity(new IdValueObject(Id), new ContactNameValueObject(FirstName, LastName));
            entity.AddEmailAddresses(Emails.Select(x => x.Email));
            entity.AddPhoneNumbers(Phones.Select(x => new Tuple<PhoneType, string>(x.PhoneType, x.PhoneNumber)));
            return entity;
        }

        public void FillFromEntity(ContactEntity entity)
        {
            FirstName = entity.Name.FirstName;
            LastName = entity.Name.LastName;
            Emails.Clear();
            Phones.Clear();

            foreach (var email in entity.EmailAddresses)
                Emails.Add(new ContactEmailModel() { Email = email.Value });

            foreach (var phone in entity.PhoneNumbers)
                Phones.Add(new ContactPhoneModel() { PhoneType = phone.PhoneType, PhoneNumber = phone.PhoneNumber });
        }

        public ContactDto ToContactDto() 
            => new ContactDto() { ContactId = Id, FirstName = FirstName, LastName = LastName, EmailsCount = Emails.Count, PhonesCount = Phones.Count };
    }
}
