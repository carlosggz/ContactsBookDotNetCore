using ContactsBook.Common.Utils;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ContactsBook.Application.Dtos
{
    public class ContactsModel: BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
        [MaxLength(36, ErrorMessage = "Id cannot have more than 36 characters")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name cannot have more than 100 characters")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Last name cannot have more than 100 characters")]
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public IEnumerable<PhoneNumberModel> PhoneNumbers { get; set; }

        protected override void OnValidation(List<ValidationResult> validations)
        {
            if (EmailAddresses != null)
            {
                foreach (var email in EmailAddresses)
                    if (!ValidationHelper.IsValidEmail(email))
                        validations.Add(new ValidationResult($"Invalid email address: {email}"));
            }

            if (PhoneNumbers != null)
            {
                var processed = new HashSet<PhoneType>();

                foreach (var phoneNumber in PhoneNumbers)
                {
                    if (phoneNumber == null)
                        validations.Add(new ValidationResult("Phone number cannot be null"));
                    else
                    {
                        if (processed.Contains(phoneNumber.PhoneType))
                            validations.Add(new ValidationResult($"Repeated phone type {phoneNumber.PhoneType}"));
                        else
                            processed.Add(phoneNumber.PhoneType);

                        validations.AddRange(phoneNumber.GetValidations());
                    }
                }
            }
        }
    }

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
