using ContactsBook.Domain.Contacts;

namespace ContactsBook.Tests.Common.ObjectMothers
{
    public static class EmailValueObjectObjectMother
    {
        public static EmailValueObject Random()
            => new EmailValueObject(Faker.Internet.Email());
    }
}
