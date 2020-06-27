using ContactsBook.Infrastructure.Repositories.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsBook.Infrastructure.Repositories.EF.Mappings
{
    class ContactPhoneModelMapping : IEFMapping
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactPhoneModel>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                 entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Phones)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_Phone_Contact");
            });
        }
    }
}
