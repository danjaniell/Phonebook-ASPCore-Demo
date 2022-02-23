using System.Text.RegularExpressions;
using FluentValidation;
using Phonebook.Models;

namespace Phonebook.Validators
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().NotNull().MaximumLength(55).WithMessage("Please specify first name");
            RuleFor(x => x.LastName).NotEmpty().NotNull().MaximumLength(55).WithMessage("Please specify last name");
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().WithMessage("Not a valid email address");
            RuleFor(x => x.Mobile).Must(BeAValidMobileNum).WithMessage("Not a valid mobile number");
        }

        private bool BeAValidMobileNum(string mobileNum)
        {
            if (string.IsNullOrWhiteSpace(mobileNum)) { return false; }

            Regex r = new(@"^(09|\+639)\d{9}$");
            return r.Match(mobileNum).Success;
        }
    }
}