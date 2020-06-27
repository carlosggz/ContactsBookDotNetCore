using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Infrastructure.Repositories.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF
{
    public class ContactsRepository : BaseEFRepository, IContactsRepository
    {
        public ContactsRepository(ContactsBookContext context)
            : base(context)
        { }
        
        #region IContactRepository
        public void Add(ContactEntity entity)
        {
            if (entity == null)
                throw new InvalidEntityException("Invalid contact entity");

            var model = new ContactModel() { Id = entity.Id.Value };
            model.FillFromEntity(entity);
            Context.Contacts.Add(model);
        }

        public void Delete(IdValueObject id)
        {
            if (id == null)
                throw new InvalidParametersException("Invalid contact id");

            var contact = Context.Contacts.SingleOrDefault(x => x.Id == id.Value);

            if (contact == null)
                throw new InvalidEntityException("Invalid entity");

            Context.Contacts.Remove(contact);
        }

        public bool ExistsContactWithName(ContactNameValueObject name, IdValueObject ignoredId = null)
            => Context
                .Contacts
                .Any(x => x.FirstName == name.FirstName && x.LastName == name.LastName && (ignoredId == null || x.Id == ignoredId.Value));

        public ContactEntity GetById(IdValueObject id)
        {
            if (id == null)
                throw new InvalidParametersException("Invalid contact id");

            var contact = Context.Contacts.SingleOrDefault(x => x.Id == id.Value);

            return contact?.ToEntity();
        }

        public SearchsResults<ContactDto> SearchByCriteria(ContactSearchCriteria criteria)
        {
            var query = Context
                .Contacts
                .Where(x => string.IsNullOrWhiteSpace(criteria.Text) || x.FirstName.Contains(criteria.Text) || x.LastName.Contains(criteria.Text));

            var count = query.Count();

            var items = query
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(x => x.ToContactDto())
                .ToList();           

            return new SearchsResults<ContactDto>(count, items);
        }

        public void Update(ContactEntity entity)
        {
            if (entity == null)
                throw new InvalidEntityException("Invalid entity");

            var model = Context.Contacts.SingleOrDefault(x => x.Id == entity.Id.Value);

            if (model == null)
                throw new InvalidEntityException("Contact not found");

            model.FillFromEntity(entity);
            Context.Contacts.Update(model);
        }

        #endregion
    }
}
