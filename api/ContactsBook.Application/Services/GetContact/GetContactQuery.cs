﻿using ContactsBook.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services.GetContact
{
    public class GetContactQuery: IRequest<ContactsModel>
    {
        public GetContactQuery(string id) => Id = id;

        public string Id { get; }
    }
}
