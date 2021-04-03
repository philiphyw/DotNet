using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day010EntityFrameworkConsole
{
    class Program
    {
        /*
        private static string GenRandomName()
        {
            Random random = new Random();

            int NameLength = random.Next(20);

            char[] NameArrChar = new char[NameLength];
            for (int i = 0; i < NameLength; i++)
            {
                NameArrChar[i] = Char.Parse(random.Next(100).ToString());
            }

            return NameArrChar.ToString();
        }
        */
        static void Main(string[] args)
        {
            try
            {
                SocietyDbContext ctx = new SocietyDbContext();
                Random random = new Random();

                Person p1 = new Person { Name = "Amanda" + random.Next(100), Age = random.Next(100) };
                ctx.People.Add(p1);//scheduled an insert operation, but not executive yet.
                ctx.SaveChanges();//synchronize objects in memory with database, all crub will be executived.
                Console.WriteLine("Record Added");


                //SELECT
                Person p2 = (from p in ctx.People where p.Id == 2 select p).FirstOrDefault<Person>();
                if (p2 != null)
                {
                    Console.WriteLine(p2.Name);
                    p2.Name = "Peter Griffin" + random.Next(100000);
                    ctx.SaveChanges();//need this line otherwise no change will apply to the database.
                    Console.WriteLine(p2.Name);
                    Console.WriteLine("Record updated");

                }
                else
                {
                    Console.WriteLine("Record to update not found");
                }

                List<Person> peopleList = (from p in ctx.People select p).ToList<Person>();
                foreach (Person p in peopleList)
                {
                    Console.WriteLine($"{p.Id};{p.Name};{p.Age}");
                }


                Person p3 = (from p in ctx.People where p.Name.Contains("Aman") select p).FirstOrDefault<Person>();
                Console.WriteLine($"{p3.Id};{p3.Name};{p3.Age} is about to be deleted");
                ctx.People.Remove(p3);
                ctx.SaveChanges();


                peopleList = (from p in ctx.People select p).ToList<Person>();
                foreach (Person p in peopleList)
                {
                    Console.WriteLine($"{p.Id};{p.Name};{p.Age}");
                }
            }
            catch (SystemException ex) //catch-all for EF, SQL and many othe exceptions
            { Console.WriteLine("Batabase operation failed: "+ ex.Message); }
            finally
            {
                Console.WriteLine("Press any key");
                Console.ReadKey();
            }
        }
    }
}
