using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class MagaluDbContext : IdentityDbContext<global::User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            const string databaseName = "magaluDB";
            const string databaseUser = "admin";
            const string databasePass = "magalu123";

            return $"Server=127.0.0.1;Port=3306;" +
                   $"database={databaseName};" +
                   $"Uid={databaseUser};" +
                   $"Pwd={databasePass};";
        }

        public static MagaluDbContext Create()
        {
            return new MagaluDbContext();
        }
    }
}

public class User : IdentityUser { }