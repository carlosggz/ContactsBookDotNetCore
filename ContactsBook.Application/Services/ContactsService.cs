using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace ContactsBook.Application.Services
{
    public class ContactsService: BaseService
    {
        private readonly IContactRepository _contactsRepository;

        public ContactsService(IUnitOfWork unitOfWork, IEventBus domainBus, IContactRepository contactsRepository) 
            : base(unitOfWork, domainBus)
        {
            _contactsRepository = contactsRepository;
        }

        //Add use case
        public void AddContact(ContactsModel model)
        {
            ValidateEntity(model);

            var name = new ContactNameValueObject(model.FirstName, model.LastName);

            if (_contactsRepository.ExistsContactWithName(name))
                throw new DomainException("There is already a contact with that name");

            var contact = new ContactEntity(new IdValueObject(model.Id), name);

            CopyAggregates(model, contact);

            UoWExecute(() => _contactsRepository.Add(contact));
        }

        //Update use case
        public void UpdateContact(ContactsModel model)
        {
            ValidateEntity(model);

            var id = new IdValueObject(model.Id);
            var name = new ContactNameValueObject(model.FirstName, model.LastName);

            if (_contactsRepository.ExistsContactWithName(name, id))
                throw new DomainException("There is already a contact with that name");

            var contact = GetContact(id);
            contact.Name = name;

            while (contact.EmailAddresses.Any())
                contact.RemoveEmailAddress(contact.EmailAddresses.First());

            while (contact.PhoneNumbers.Any())
                contact.RemovePhoneNumber(contact.PhoneNumbers.First());

            CopyAggregates(model, contact);

            UoWExecute(() => _contactsRepository.Update(contact));
        }

        //Delete use case
        public void DeleteContact(IdValueObject id)
        {
            var contact = GetContact(id);

            UoWExecute(() => _contactsRepository.Remove(contact));
        }

        #region Helpers
        private ContactEntity GetContact(IdValueObject id)
        {
            if (id == null)
                throw new InvalidEntityException("Invalid id");

            return _contactsRepository.GetById(id) ?? throw new InvalidEntityException("Contact not found");
        }

        private void CopyAggregates(ContactsModel model, ContactEntity contact)
        {
            if (model.EmailAddresses != null)
            {
                foreach (var email in model.EmailAddresses)
                    contact.AddEmailAddress(new EmailValueObject(email));
            }

            if (model.PhoneNumbers != null)
            {
                foreach (var phone in model.PhoneNumbers)
                    contact.AddPhoneNumber(new PhoneValueObject(phone.PhoneType, phone.PhoneNumber));
            }
        }

        #endregion
    }
}
