using ContactsManagement.Core.Models.Validation;
using FluentValidation.Results;

namespace ContactsManagement.Core.Models
{
    public class ContactsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ValidationResult Validate()
        {
            var validator = new ContactsModelValidator();
            var result = validator.Validate(this);
            return result;
        }
    }
}
