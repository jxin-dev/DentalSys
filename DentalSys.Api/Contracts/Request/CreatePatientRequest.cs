namespace DentalSys.Api.Contracts.Request
{
   
    public record CreatePatientRequest(
           string FirstName,
           string LastName,
           string MiddleName,
           DateTime DateOfBirth,
           string Gender,
           string Address,
           string PhoneNumber,
           string Email);

}
