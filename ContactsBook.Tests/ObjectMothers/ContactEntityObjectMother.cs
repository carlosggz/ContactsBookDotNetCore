using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;

namespace ContactsBook.Tests.ObjectMothers
{
    public static class ContactEntityObjectMother
    {
        public static ContactEntity Random()
            => new ContactEntity(new IdValueObject(), ContactNameValueObjectObjectMother.Random());
    }
}
