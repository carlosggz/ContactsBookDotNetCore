using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContactsBook.Common.Utils
{
    public static class ValidationHelper
    {
        private const string EmailRegularExpression = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private const string PhoneNumberExpression = @"^(\+[0-9]{9})$";

        private static bool Matches(string value, string pattern)
            => !string.IsNullOrWhiteSpace(value) && Regex.Match(value, pattern).Success;
        public static bool IsValidEmail(string emailAddress)
            => Matches(emailAddress, EmailRegularExpression);

        public static bool IsValidPhoneNumber(string phoneNumber)
            => Matches(phoneNumber, PhoneNumberExpression);

        public static bool IsValidId(string value) 
            => Guid.TryParse(value, out Guid _);
    }
}
