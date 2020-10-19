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

namespace ContactsBook.Application.Services.UpdateContact
{
    public class UpdateContactCommandHandler : BaseHandler, IRequestHandler<UpdateContactCommand, Unit>
    {
        private readonly IContactsRepository _repository;

        public UpdateContactCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IContactsRepository repository)
            : base(unitOfWork, eventBus) => _repository = repository;

        public Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            ValidateEntity(request.Model);

            var id = new IdValueObject(request.Model.Id);
            var name = new ContactNameValueObject(request.Model.FirstName, request.Model.LastName);

            if (_repository.ExistsContactWithName(name, id))
                throw new DomainException("There is already a contact with that name");

            var contact = _repository.GetById(id) ?? throw new EntityNotFound("Contact not found");
            contact.Name = name;
            contact.RemoveAllEmailAddress();
            contact.RemoveAllPhoneNumbers();
            contact.AddEmailAddresses(request.Model.EmailAddresses);
            contact.AddPhoneNumbers(request.Model.PhoneNumbers.Select(x => new Tuple<PhoneType, string>(x.PhoneType, x.PhoneNumber)));

            EventBus.Record(new ContactUpdatedDomainEvent(contact.Id.Value, contact.Name.FirstName, contact.Name.LastName));

            UoWExecute(() => _repository.Update(contact));
            return Task.FromResult(Unit.Value);
        }
    }
}
