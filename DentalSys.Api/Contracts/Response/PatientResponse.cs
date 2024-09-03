namespace DentalSys.Api.Contracts.Response
{
    public record PatientResponse(
        Guid PatientId,
        string FirstName,
        string LastName,
        string MiddleName,
        DateTime DateOfBirth,
        string Gender,
        string Address,
        string PhoneNumber,
        string Email);
}
