using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContactsBook.Application.Dtos
{
    public abstract class BaseModel: IModel
    {
        public IEnumerable<ValidationResult> GetValidations()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, validationContext, validationResults, true);

            return validationResults;
        }

        protected abstract void OnValidation(List<ValidationResult> validations);
    }
}
