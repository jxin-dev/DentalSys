using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Response;
using DentalSys.Api.Features.Identity.Security;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace DentalSys.Api.Features.Identity.LoginUser
{
    public class Handler : IRequestHandler<Command, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<Command> _validator;

        public Handler(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher, IValidator<Command> validator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task<Result<TokenResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<TokenResponse>(UserErrors.Validation(validationResult.Errors.First().ErrorMessage));
            }

            var username = request.Username;
            var password = request.Password;

            var user = await _userRepository.GetUsernameAsync(username);

            if(user is null)
            {
                return Result.Failure<TokenResponse>(UserErrors.NotFound());
            }

            bool verified = _passwordHasher.Verify(password, user.PasswordHash);

            if (!verified)
            {
                return Result.Failure<TokenResponse>(UserErrors.AccessDenied());
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            if(user.Role == Role.Admin)
            {
                foreach(var permission in Role.AdminRole())
                {
                    claims.Add(new Claim("Permission", permission));
                }
            }

            if (user.Role == Role.User)
            {
                foreach (var permission in Role.UserRole())
                {
                    claims.Add(new Claim("Permission", permission));
                }
            }

            var token = _tokenService.GenerateToken(username, claims);

            return Result.Success(new TokenResponse(token));

        }
    }
}
