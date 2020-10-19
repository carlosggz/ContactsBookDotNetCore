using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Application.Services.GetContacts
{
    public class GetContactsQueryHandler : BaseHandler, IRequestHandler<GetContactsQuery, SearchResults<ContactDto>>
    {
        private readonly IContactsRepository _repository;

        public GetContactsQueryHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IContactsRepository repository)
            : base(unitOfWork, eventBus) => _repository = repository;
        public Task<SearchResults<ContactDto>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.Size < 1)
                throw new InvalidParametersException("Invalid search parameters");

            return Task.FromResult(_repository.SearchByCriteria(new ContactSearchCriteria(request.Page, request.Size, request.Text)));
        }
    }
}
