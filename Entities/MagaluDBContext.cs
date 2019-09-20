using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class MagaluDbContext : DbContext
    {
        public MagaluDbContext(DbContextOptions<MagaluDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
