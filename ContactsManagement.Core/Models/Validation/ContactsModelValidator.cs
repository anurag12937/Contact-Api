using FluentValidation;
using FluentValidation.Validators;

namespace ContactsManagement.Core.Models.Validation
{
    public class ContactsModelValidator: AbstractValidator<ContactsModel>
    {
        public ContactsModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please fill first name.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please fill Last name.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Please fill valid email.");
        }
    }
}
