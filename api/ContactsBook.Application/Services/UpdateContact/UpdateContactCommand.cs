using ContactsBook.Application.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services.UpdateContact
{
    public class UpdateContactCommand: IRequest<Unit>
    {
        public UpdateContactCommand(ContactsModel model) => Model = model;

        public ContactsModel Model { get; }
    }
}
