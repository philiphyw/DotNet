using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Day012EagerLoadingPractice.DataLayer
{
    public class CarsOwnersDbContext : DbContext
    {
        const string DbName = "Database.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, DbName);
        public CarsOwnersDbContext() : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30") { }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}