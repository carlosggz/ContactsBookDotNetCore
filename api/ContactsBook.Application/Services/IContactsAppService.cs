﻿using ContactsBook.Application.Dtos;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;

namespace ContactsBook.Application.Services
{
    public interface IContactsAppService
    {
        void AddContact(ContactsModel model);
        void DeleteContact(string id);
        ContactsModel GetContact(string id);
        SearchResults<ContactDto> GetContactsByName(int page, int size, string text);
        void UpdateContact(ContactsModel model);
    }
}