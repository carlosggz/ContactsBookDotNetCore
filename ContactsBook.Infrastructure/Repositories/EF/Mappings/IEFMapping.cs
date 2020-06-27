using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF.Mappings
{
    interface IEFMapping
    {
        void Map(ModelBuilder modelBuilder);
    }
}
