using System;
using System.Data.Entity;
using System.Linq;

namespace Day010EntityFrameworkConsole
{
    public class SocietyDbContext : DbContext
    {
        // Your context has been configured to use a 'SocietyDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Day010EntityFrameworkConsole.SocietyDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SocietyDbContext' 
        // connection string in the application configuration file.
        public SocietyDbContext()
            : base("name=SocietyDbContext")
        {

        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<Person> People { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}