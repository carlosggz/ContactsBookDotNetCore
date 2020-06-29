using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public enum PhoneType
    {
        [Description("Home Phone")]
        Home = 0,

        [Description("Work Phone")]
        Work,

        [Description("Mobile Phone")]
        Mobile
    }
}
