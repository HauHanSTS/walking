using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole> 
            {
                new IdentityRole
                {
                    Id = "a5430ca2-946c-4220-8b97-cf9fc5acef22",
                    ConcurrencyStamp = "a5430ca2-946c-4220-8b97-cf9fc5acef22",
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = "f9f4c19d-22d2-4d75-ab57-4e9c83bfdbd0",
                    ConcurrencyStamp = "f9f4c19d-22d2-4d75-ab57-4e9c83bfdbd0",
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
