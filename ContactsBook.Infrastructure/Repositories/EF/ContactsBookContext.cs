using ContactsBook.Infrastructure.Repositories.EF.Mappings;
using ContactsBook.Infrastructure.Repositories.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF
{
    public class ContactsBookContext: DbContext
    {
        public ContactsBookContext(DbContextOptions<ContactsBookContext> options)
            :base(options)
        {}

        public virtual DbSet<ContactModel> Contacts { get; set; }
        public virtual DbSet<ContactEmailModel> ContactEmails { get; set; }
        public virtual DbSet<ContactPhoneModel> ContactPhones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new ContactModelMapping().Map(modelBuilder);
            new ContactEmailModelMapping().Map(modelBuilder);
            new ContactPhoneModelMapping().Map(modelBuilder);
        }
    }
}
