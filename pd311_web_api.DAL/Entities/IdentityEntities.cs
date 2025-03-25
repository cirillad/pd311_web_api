using Microsoft.AspNetCore.Identity;

namespace pd311_web_api.DAL.Entities
{
    public class IdentityEntities
    {
        public class AppUser : IdentityUser, IBaseEntity<string>
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Image { get; set; }

            // Implement IBaseEntity<string>
            public string Id { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

            public virtual ICollection<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();
            public virtual ICollection<AppUserLogin> Logins { get; set; } = new List<AppUserLogin>();
            public virtual ICollection<AppUserToken> Tokens { get; set; } = new List<AppUserToken>();
            public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
        }

        public class AppRole : IdentityRole
        {
            public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
            public virtual ICollection<AppRoleClaim> RoleClaims { get; set; } = [];
        }

        public class AppUserRole : IdentityUserRole<string>
        {
            public virtual AppUser? User { get; set; }
            public virtual AppRole? Role { get; set; }
        }

        public class AppUserClaim : IdentityUserClaim<string>
        {
            public virtual AppUser? User { get; set; }
        }

        public class AppUserLogin : IdentityUserLogin<string>
        {
            public virtual AppUser? User { get; set; }
        }

        public class AppRoleClaim : IdentityRoleClaim<string>
        {
            public virtual AppRole? Role { get; set; }
        }

        public class AppUserToken : IdentityUserToken<string>
        {
            public virtual AppUser? User { get; set; }
        }
    }
}
