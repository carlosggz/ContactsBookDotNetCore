using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Application.Services.AddContact
{
    public class AddContactCommandHandler : BaseHandler, IRequestHandler<AddContactCommand, Unit>
    {
        private readonly IContactsRepository _repository;

        public AddContactCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IContactsRepository repository)
            : base(unitOfWork, eventBus) => _repository = repository;
        
        public Task<Unit> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {
            ValidateEntity(request.Contact);

            var name = new ContactNameValueObject(request.Contact.FirstName, request.Contact.LastName);

            if (_repository.ExistsContactWithName(name))
                throw new DomainException("There is already a contact with that name");

            var contact = new ContactEntity(new IdValueObject(request.Contact.Id), name);
            contact.AddEmailAddresses(request.Contact.EmailAddresses);
            contact.AddPhoneNumbers(request.Contact.PhoneNumbers.Select(x => new Tuple<PhoneType, string>(x.PhoneType, x.PhoneNumber)));

            EventBus.Record(new ContactAddedDomainEvent(contact.Id.Value, contact.Name.FirstName, contact.Name.LastName));

            UoWExecute(() => _repository.Add(contact));

            return Task.FromResult(Unit.Value);
        }
    }
}
