using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task9
{
    internal class AppicationDbContext: DbContext
    {
        public DbSet<StudentEntity> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=Task9;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
        }
    }
}
