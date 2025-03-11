using FluentValidation;
using ParkingHere.Application.ApplicationUsers.Commands;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        private readonly IUserRepository _userRepository;
        
        public RegisterUserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom(async (value, context) =>
                {
                    var emailInUse = await _userRepository.GetByEmailAsync(value) != null;
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
