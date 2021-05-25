using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SpinnySideUp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //PILOT CAN HAVE MULTIPLE FLIGHTS
        public ICollection<Flight> Flights { get; set; }
        public ICollection<Pilot> Pilots { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ssuDBConnection", throwIfV1Schema: false)
        {
        }
        public IEnumerable ApplicationUsers { get; internal set; }
        //MODELS LINKED TO BOTH FLIGHTS AND PILOTS RESPECTIVLY
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Pilot> Pilots { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}