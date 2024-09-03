using DentalSys.Api.Common.Models;

namespace DentalSys.Api.Features.Patients
{
    public static class PatientErrors
    {
        public static Error NotFound(Guid patientId)
        {
            return new Error("Patient.Notfound", $"The patient with Id = {patientId} was not found.", ErrorType.NotFound);
        }

        public static Error Validation(string message)
        {
            return new Error("Patient.Validation", message, ErrorType.BadRequest);
        }
    }
}
