using DentalSys.Api.Features.Patients.CreatePatient;
using FluentValidation;

namespace DentalSys.Api.Features.Patients
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPatientFeatures(this IServiceCollection services)
        {
            //services.AddValidatorsFromAssemblyContaining<Validator>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            return services;
        }
    }
}
