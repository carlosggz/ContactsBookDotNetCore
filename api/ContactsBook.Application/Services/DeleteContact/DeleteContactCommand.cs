using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services.DeleteContact
{
    public class DeleteContactCommand: IRequest<Unit>
    {
        public DeleteContactCommand(string id) 
            => Id = id;

        public string Id { get; }
    }
}
