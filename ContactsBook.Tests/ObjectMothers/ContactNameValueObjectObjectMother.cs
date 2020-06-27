using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.ObjectMothers
{
    public static class ContactNameValueObjectObjectMother
    {
        public static ContactNameValueObject Random()
            => new ContactNameValueObject(Faker.Name.First(), Faker.Name.Last());
    }
}
