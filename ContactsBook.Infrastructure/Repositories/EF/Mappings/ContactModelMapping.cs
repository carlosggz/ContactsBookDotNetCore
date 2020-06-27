using ContactsBook.Infrastructure.Repositories.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF.Mappings
{
    class ContactModelMapping : IEFMapping
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactModel>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
