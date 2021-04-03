using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Day011CarOwner
{
    public class CarOwnerDbContext : DbContext
    {
        // Your context has been configured to use a 'CarOwnerDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Day011CarOwner.CarOwnerDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CarOwnerDbContext' 
        // connection string in the application configuration file.
        const string DbName = "Database.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, DbName); // database will be put into the big\debug\
        //static string DbPath = Path.Combine(Environment.CurrentDirectory + @"\..\..\", DbName);

        public CarOwnerDbContext()
            : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30") //base("name=TodoDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}