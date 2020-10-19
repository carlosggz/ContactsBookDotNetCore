using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services.GetContacts
{
    public class GetContactsQuery : IRequest<SearchResults<ContactDto>>
    {
        public GetContactsQuery(int page, int size, string text)
        {
            Page = page;
            Size = size;
            Text = text;
        }

        public int Page { get; }
        public int Size { get; }
        public string Text { get; }
    }
}
