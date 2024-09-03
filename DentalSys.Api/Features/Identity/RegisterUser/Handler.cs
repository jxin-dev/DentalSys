using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Response;
using DentalSys.Api.Features.Identity.Security;
using FluentValidation;
using MediatR;

namespace DentalSys.Api.Features.Identity.RegisterUser
{
    public class Handler : IRequestHandler<Command, Result<RegisterUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<Command> _validator;

        public Handler(IUserRepository userRepository, IPasswordHasher passwordHasher, IValidator<Command> validator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task<Result<RegisterUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<RegisterUserResponse>(UserErrors.Validation(validationResult.Errors.First().ErrorMessage));
            }

            var userAlreadyExist = await _userRepository.UserExistAsync(request.Username);
            if (userAlreadyExist)
            {
                return Result.Failure<RegisterUserResponse>(UserErrors.AlreadyExist());
            }

            User newUser = new()
            {
                UserId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Username = request.Username,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = request.Role ?? "User"
            };

            var userId = await _userRepository.CreateAsync(newUser);

            return Result.Success(new RegisterUserResponse("User registered successfully", userId));


        }
    }
}
