using ContactsBook.Domain.Contacts;

namespace ContactsBook.Tests.ObjectMothers
{
    public static class EmailValueObjectObjectMother
    {
        public static EmailValueObject Random()
            => new EmailValueObject(Faker.Internet.Email());
    }
}
