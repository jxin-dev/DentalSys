using DentalSys.Api.Common.Models;

namespace DentalSys.Api.Features.Identity
{
    public static class UserErrors
    {
        public static Error AccessDenied()
        {
            return new Error("User.Unauthorized", "Access Denied", ErrorType.Unauthorized);
        }
        public static Error Validation(string message)
        {
            return new Error("User.Validation", message, ErrorType.BadRequest);
        }
        public static Error NotFound()
        {
            return new Error("User.NotFound", "The username was not found.", ErrorType.NotFound);
        }
        public static Error AlreadyExist()
        {
            return new Error("User.AlreadyExist", "This username is already taken. Please try another one.", ErrorType.Conflict);
        }
    }
}
