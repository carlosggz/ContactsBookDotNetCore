using ContactsBook.Common.Entities;
using ContactsBook.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public interface IRepository<TEntity, TId, TCriteria, TSearchResultsDto>
        where TEntity : IAggregateRoot
        where TId : IValueObject
        where TCriteria: ISearchCriteria
        where TSearchResultsDto: IDto
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        TEntity GetById(TId id);
        ISearchsResults<TSearchResultsDto> SearchByCriteria(TCriteria criteria);
    }
}
