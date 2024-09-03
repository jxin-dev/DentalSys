namespace DentalSys.Api.Features.Identity
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static ICollection<string> AdminRole()
        {
            var permissions = new List<string>();
            permissions.Add(Permission.PatientView);
            permissions.Add(Permission.PatientCreate);

            return permissions;
        }

        public static ICollection<string> UserRole()
        {
            var permissions = new List<string>();
            permissions.Add(Permission.PatientView);

            return permissions;
        }

   
    }
}
