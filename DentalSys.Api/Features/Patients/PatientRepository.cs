using DentalSys.Api.Common.Interfaces;
using DentalSys.Api.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalSys.Api.Features.Patients
{
    public interface IPatientRepository : IGenericRepository<Patient, Guid>
    {

    }

    public class PatientRepository(DentalDbContext context) : IPatientRepository
    {
        public async Task<Guid> CreateAsync(Patient entity)
        {
            await context.Patients.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.PatientId;
        }

        public async Task<int> DeleteAsync(Patient entity)
        {
            context.Patients.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Patient>?> GetAllAsync(Expression<Func<Patient, bool>>? predicate = null)
        {
            if (predicate is null)
            {
                return await context.Patients.ToListAsync();
            }
            return await context.Patients.Where(predicate).ToListAsync();
        }

        public async Task<Patient?> GetAsync(Expression<Func<Patient, bool>> predicate)
        {
            return await context.Patients.SingleOrDefaultAsync(predicate);
        }

        public async Task<int> UpdateAsync(Patient entity)
        {
            context.Update(entity);
            return await context.SaveChangesAsync();
        }
    }
}
