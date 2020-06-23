using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;

namespace ContactsBook.Tests.Domain.Entities
{
    public static class ObjectMotherContact
    {
        public static ContactEntity Random()
            => new ContactEntity(new IdValueObject(), new ContactNameValueObject(Faker.Name.First(), Faker.Name.Last()));
    }
}
