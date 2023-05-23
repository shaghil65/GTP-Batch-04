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

        //Overrriding this method to seed some data to our entity/table using entity framework

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Bilal", Age = 17 },
                new Person { Id = 2, Name = "Shaghil", Age = 22 },
                new Person { Id = 3, Name = "Ali", Age = 23 }
                );
        }
    }
}
