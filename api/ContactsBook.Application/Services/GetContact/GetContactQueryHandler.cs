using ContactsBook.Application.Dtos;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Application.Services.GetContact
{
    public class GetContactQueryHandler : BaseHandler, IRequestHandler<GetContactQuery, ContactsModel>
    {
        private readonly IContactsRepository _repository;

        public GetContactQueryHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IContactsRepository repository)
            : base(unitOfWork, eventBus) => _repository = repository;

        public Task<ContactsModel> Handle(GetContactQuery request, CancellationToken cancellationToken)
        {
            var entity = _repository.GetById(new IdValueObject(request.Id)) ?? throw new EntityNotFound("Contact not found");

            return Task.FromResult(new ContactsModel()
            {
                Id = entity.Id.Value,
                FirstName = entity.Name.FirstName,
                LastName = entity.Name.LastName,
                EmailAddresses = entity.EmailAddresses.Select(x => x.Value),
                PhoneNumbers = entity.PhoneNumbers.Select(x => new PhoneNumberModel() { PhoneType = x.PhoneType, PhoneNumber = x.PhoneNumber })
            });
        }
    }
}
