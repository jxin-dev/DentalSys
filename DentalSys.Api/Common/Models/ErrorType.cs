namespace DentalSys.Api.Common.Models
{
    public static class ErrorType
    {
        public static readonly int BadRequest = 400;
        public static readonly int Unauthorized = 401;
        public static readonly int NotFound = 404;
        public static readonly int Conflict = 409;
        public static readonly int ServerError = 500;

    }
}
