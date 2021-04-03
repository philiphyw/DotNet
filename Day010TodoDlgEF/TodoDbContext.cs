using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Day010TodoDlgEF
{
    public class TodoDbContext : DbContext
    {
        // Your context has been configured to use a 'TodoDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Day010TodoDlgEF.TodoDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TodoDbContext' 
        // connection string in the application configuration file.
        const string DbName = "Database.mdf";

		static string DbPath = Path.Combine(Environment.CurrentDirectory, DbName); // database will be put into the big\debug\

		//Below line will create the database on the project folder, but it will cause error while trying to add new view in Server Explorer.Data Connections.
        //static string DbPath = Path.Combine(Environment.CurrentDirectory + @"\..\..\", DbName);

        public TodoDbContext()
            : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30") //base("name=TodoDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Todo> Todos { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}