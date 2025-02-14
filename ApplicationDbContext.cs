using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engitask
{
   public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer("Server=.;Database=Engitask;Integrated Security=True;TrustServerCertificate=True;");

        public DbSet<User> Users { get; set; }
    }

    

}
