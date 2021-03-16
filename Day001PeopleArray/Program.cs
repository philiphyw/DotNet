using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Tech:c# basic,  output parameter
namespace Day001PeopleArray
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] nameArray = new string[4];
            int[] ageArray = new int[4];

            //get input names
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Enter name #{0}:", i + 1);
                string name = Console.ReadLine();
                nameArray[i] = name;


                Console.WriteLine("Enter age#{0}: ",i+1);
                string ageStr = Console.ReadLine();
                //Java way: using trycatch to parse string to int
                //try
                //{
                //    int age = int.Parse(ageStr);//FormatException
                //    ageArray[i] = age;
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Invalid number format in age");
                //    Environment.Exit(1);
                //}


                //C# way:parsing using output parameter
                if (!int.TryParse(ageStr, out int age)) {
                    Console.WriteLine("Invalid input. Exiting...");
                    return;
                }

                ageArray[i] = age;

            }

            //print input names

            for (int i = 0; i < nameArray.Length; i++)
            {
                Console.Write("{0}{1} is {2} years old:",i==0?"":", ", nameArray[i],ageArray[i]);
            }
            Console.WriteLine();


            //Use Ctrl + F5 to run the code in debug mode, or need below line to stop console window to close after finishing above codes. 
            //Console.WriteLine("Press any key to end");
            //Console.ReadKey();
        }
    }
}
