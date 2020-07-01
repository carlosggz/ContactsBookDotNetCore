using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContactsBook.Application.Dtos
{
    public interface IModel
    {
        IEnumerable<ValidationResult> GetValidations();
    }
}
