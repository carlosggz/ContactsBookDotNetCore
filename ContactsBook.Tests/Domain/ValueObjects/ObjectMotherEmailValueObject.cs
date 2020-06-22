using ContactsBook.Domain.Contacts;

namespace ContactsBook.Tests.Domain.ValueObjects
{
    public static class ObjectMotherEmailValueObject
    {
        public static EmailValueObject Random()
            => new EmailValueObject(Faker.Internet.Email());
    }
}
