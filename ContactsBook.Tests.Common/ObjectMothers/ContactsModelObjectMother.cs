using ContactsBook.Application.Dtos;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Common.ObjectMothers
{
    public static class ContactsModelObjectMother
    {
        public static ContactsModel Random()
            => new ContactsModel()
            {
                Id = new IdValueObject().Value,
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                EmailAddresses = new List<string>
                {
                    Faker.Internet.Email()
                },
                PhoneNumbers = new List<PhoneNumberModel>
                {
                    new PhoneNumberModel()
                    {
                        PhoneType = PhoneValueObjectObjectMother.GetRandomPhoneType(),
                        PhoneNumber = PhoneValueObjectObjectMother.GetRandomPhoneNumber()
                    }
                }
            };

        public static ContactsModel FromEntity(ContactEntity contact)
            => new ContactsModel()
            {
                Id = contact.Id.Value,
                FirstName = contact.Name.FirstName,
                LastName = contact.Name.LastName,
                EmailAddresses = contact.EmailAddresses.Select(x => x.Value).ToList(),
                PhoneNumbers = contact.PhoneNumbers.Select(x => new PhoneNumberModel() { PhoneType = x.PhoneType, PhoneNumber = x.PhoneNumber })
            };
    }
}
