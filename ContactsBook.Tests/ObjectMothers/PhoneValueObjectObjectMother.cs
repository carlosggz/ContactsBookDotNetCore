using ContactsBook.Domain.Contacts;
using System;

namespace ContactsBook.Tests.ObjectMothers
{
    public static class PhoneValueObjectObjectMother
    {
        public static readonly string[] PhoneNumbers =
        {
                "3087774825",
                "(281)388-0388",
                "(281)388-0300",
                "(979) 778-0978",
                "(281)934-2479",
                "(281)934-2447",
                "(979)826-3273",
                "(979)826-3255",
                "1334714149",
                "(281)356-2530",
                "(281)356-5264",
                "(936)825-2081",
                "(832)595-9500",
                "(832)595-9501",
                "281-342-2452",
                "1334431660"
        };

        public static PhoneType GetRandomPhoneType()
            => (PhoneType)new Random().Next((int)PhoneType.Home, (int)PhoneType.Mobile);

        public static string GetRandomPhoneNumber()
            => PhoneNumbers[new Random().Next(0, PhoneNumbers.Length - 1)];

        public static PhoneValueObject Random()
            => new PhoneValueObject(GetRandomPhoneType(), GetRandomPhoneNumber());
    }
}
