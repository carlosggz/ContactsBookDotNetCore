using ContactsBook.Application.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services.AddContact
{
    public class AddContactCommand: IRequest<Unit>
    {
        public AddContactCommand(ContactsModel contact)
            => Contact = contact;

        public ContactsModel Contact { get; }
    }
}
