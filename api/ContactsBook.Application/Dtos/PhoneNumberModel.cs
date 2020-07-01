using ContactsBook.Common.Utils;
using ContactsBook.Domain.Contacts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContactsBook.Application.Dtos
{
    public class PhoneNumberModel: BaseModel
    {
        public PhoneType PhoneType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required")]
        [MaxLength(50, ErrorMessage = "Phone number cannot have more than 50 characters")]
        public string PhoneNumber { get; set; }

        protected override void OnValidation(List<ValidationResult> validations)
        {
            if (!validations.Any() && !ValidationHelper.IsValidPhoneNumber(PhoneNumber))
                validations.Add(new ValidationResult($"Invalid phone number: {PhoneNumber}"));
        }
    }
}
