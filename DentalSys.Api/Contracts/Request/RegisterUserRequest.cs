namespace DentalSys.Api.Contracts.Request
{
    public record RegisterUserRequest(
        string FirstName,
        string LastName,
        string Username,
        string Password,
        string Role,
        string Email,
        string PhoneNumber);
}
