using FluentValidation;
using PractTest.Application.DTOs;
using PractTest.Application.Interfaces.Customers;

namespace PractTest.Application.DTOValidations
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator(ICustomerRepository repository)
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
