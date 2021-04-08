using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Day012MidTerm.DataLayer
{
    public class PersonPassportDbContext : DbContext
    {
        const string DbName = "Database.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, DbName);
        public PersonPassportDbContext() : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30") { }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Passport> Passports { get; set; }

        /*
         * TODO: how to use ModelBuilder in Fluent APi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passport>()
                .HasIndex(p => p.PassportNo)
                .IsUnique();
        }
        */
    }

  
}