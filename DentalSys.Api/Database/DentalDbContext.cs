using DentalSys.Api.Features.Identity;
using DentalSys.Api.Features.Patients;
using Microsoft.EntityFrameworkCore;

namespace DentalSys.Api.Database
{
    public class DentalDbContext : DbContext
    {
        public DentalDbContext(DbContextOptions<DentalDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
