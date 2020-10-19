using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Application.Services.DeleteContact
{
    public class DeleteContactCommandHandler : BaseHandler, IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly IContactsRepository _repository;
        public DeleteContactCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IContactsRepository repository)
            : base(unitOfWork, eventBus) => _repository = repository;

        public Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                throw new InvalidEntityException("Invalid id");

            var contact = _repository.GetById(new IdValueObject(request.Id)) ?? throw new EntityNotFound("Contact not found");

            EventBus.Record(new ContactDeletedDomainEvent(contact.Id.Value, contact.Name.FirstName, contact.Name.LastName));

            UoWExecute(() => _repository.Delete(contact.Id));

            return Task.FromResult(Unit.Value);
        }
    }
}
