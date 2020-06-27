using ContactsBook.Infrastructure.Repositories.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsBook.Infrastructure.Repositories.EF.Mappings
{
    class ContactEmailModelMapping : IEFMapping
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactEmailModel>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Emails)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_Email_Contact");
            });
        }
    }
}
