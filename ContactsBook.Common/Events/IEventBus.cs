using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactsBook.Common.Events
{
    public interface IEventBus
    {
        void Record(IDomainEvent domainEvent);
        Task PublishAsync();
    }

}
