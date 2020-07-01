using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ContactsBook.Application.Services
{
    public class ContactsAppService : BaseAppService, IContactsAppService
    {
        public ContactsAppService(IContactsBookUnitOfWork unitOfWork, IEventBus domainBus)
            : base(unitOfWork, domainBus)
        { }

        //Add use case
        public void AddContact(ContactsModel model)
        {
            ValidateEntity(model);

            var name = new ContactNameValueObject(model.FirstName, model.LastName);

            if (UnitOfWork.ContactsRepository.ExistsContactWithName(name))
                throw new DomainException("There is already a contact with that name");

            var contact = new ContactEntity(new IdValueObject(model.Id), name);
            contact.AddEmailAddresses(model.EmailAddresses);
            contact.AddPhoneNumbers(model.PhoneNumbers.Select(x => new Tuple<PhoneType, string>(x.PhoneType, x.PhoneNumber)));

            EventBus.Record(new ContactAddedDomainEvent(contact.Id.Value, contact.Name.FirstName, contact.Name.LastName));

            UoWExecute(() => UnitOfWork.ContactsRepository.Add(contact));
        }

        //Update use case
        public void UpdateContact(ContactsModel model)
        {
            ValidateEntity(model);

            var id = new IdValueObject(model.Id);
            var name = new ContactNameValueObject(model.FirstName, model.LastName);

            if (UnitOfWork.ContactsRepository.ExistsContactWithName(name, id))
                throw new DomainException("There is already a contact with that name");

            var contact = GetContact(id);
            contact.Name = name;
            contact.RemoveAllEmailAddress();
            contact.RemoveAllPhoneNumbers();
            contact.AddEmailAddresses(model.EmailAddresses);
            contact.AddPhoneNumbers(model.PhoneNumbers.Select(x => new Tuple<PhoneType, string>(x.PhoneType, x.PhoneNumber)));

            EventBus.Record(new ContactUpdatedDomainEvent(contact.Id.Value, contact.Name.FirstName, contact.Name.LastName));

            UoWExecute(() => UnitOfWork.ContactsRepository.Update(contact));
        }

        //Delete use case
        public void DeleteContact(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new InvalidEntityException("Invalid id");

            var contact = GetContact(new IdValueObject(id));

            EventBus.Record(new ContactDeletedDomainEvent(contact.Id.Value, contact.Name.FirstName, contact.Name.LastName));

            UoWExecute(() => UnitOfWork.ContactsRepository.Delete(contact.Id));
        }

        //Search use case
        public SearchResults<ContactDto> GetContactsByName(int page, int size, string text)
        {
            if (page < 1 || size < 1)
                throw new InvalidParametersException("Invalid search parameters");

            return UnitOfWork.ContactsRepository.SearchByCriteria(new ContactSearchCriteria(page, size, text));
        }

        //Get contact by id
        public ContactsModel GetContact(string id)
        {
            var entity = GetContact(new IdValueObject(id));

            return new ContactsModel()
            {
                Id = entity.Id.Value,
                FirstName = entity.Name.FirstName,
                LastName = entity.Name.LastName,
                EmailAddresses = entity.EmailAddresses.Select(x => x.Value),
                PhoneNumbers = entity.PhoneNumbers.Select(x => new PhoneNumberModel() { PhoneType = x.PhoneType, PhoneNumber = x.PhoneNumber })
            };
        }
        #region Helpers
        private ContactEntity GetContact(IdValueObject id)
        {
            if (id == null)
                throw new InvalidEntityException("Invalid id");

            return UnitOfWork.ContactsRepository.GetById(id) ?? throw new EntityNotFound("Contact not found");
        }

        #endregion
    }
}
