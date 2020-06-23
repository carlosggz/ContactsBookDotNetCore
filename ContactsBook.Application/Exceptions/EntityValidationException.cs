using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContactsBook.Application.Exceptions
{
    public class EntityValidationException: Exception
    {
        public EntityValidationException(IEnumerable<ValidationResult> validationResults)
            => ValidationResults = validationResults;

        public IEnumerable<ValidationResult> ValidationResults { get; private set; }
    }
}
