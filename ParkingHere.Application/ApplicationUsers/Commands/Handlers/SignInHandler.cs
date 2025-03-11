using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Application.Security;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Commands.Handlers
{
    public class SignInHandler : ICommandHandler<SignIn>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticator _authenticator;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenStorage _tokenStorage;

        public SignInHandler(IUserRepository userRepository, IAuthenticator authenticator, IPasswordManager passwordManager,
            ITokenStorage tokenStorage)
        {
            _userRepository = userRepository;
            _authenticator = authenticator;
            _passwordManager = passwordManager;
            _tokenStorage = tokenStorage;
        }

        public async Task HandleAsync(SignIn command)
        {
            var user = await _userRepository.GetByEmailAsync(command.Email);
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            if (!_passwordManager.Validate(command.Password, user.PasswordHash))
            {
                throw new InvalidCredentialsException();
            }

            var jwt = _authenticator.CreateToken(user.Id, user.Role.Name);
            _tokenStorage.Set(jwt);
        }
    }
}
