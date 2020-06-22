using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;

namespace ContactsBook.Tests.Domain.Entities
{
    public static class ObjectMotherContact
    {
        public static Contact Random()
            => new Contact(new IdValueObject(), new ContactNameValueObject(Faker.Name.First(), Faker.Name.Last()));
    }
}
