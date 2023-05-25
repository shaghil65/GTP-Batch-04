using Microsoft.EntityFrameworkCore;
using System;
using Task7_UpdateEF.Models;

namespace Task7_UpdateEF.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Passing all the configuration to base class which DbContext using options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
    }
}
