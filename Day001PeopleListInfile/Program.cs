using System;
using System.Collections.Generic;
using System.IO;

namespace Day001PeopleListInfile
{
    class Program
    {
        const string DATA_FILE = "..\\..\\people.txt";


        private static void showMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1.Add person info");
            Console.WriteLine("2.List persons info");
            Console.WriteLine("3.Find a person by name");
            Console.WriteLine("4.Find all persons younger than age");
            Console.WriteLine("0.Exit");
            Console.Write("Your choice:");

        }




        static void Main(string[] args)
        {
            while (true)
            {
                //readdata
                LoadDataFromFile();

                // showMenu
                showMenu();
                string returnVal = Console.ReadLine();
                Console.WriteLine();
                switch (returnVal)
                {
                    case "1":
                        AddPerson();
                        break;
                    case "2":
                        ListAllPerson();
                        break;
                    case "3":
                        FindPersonByName();
                        break;
                    case "4":
                        FindPersonYoungerThan();
                        break;
                    case "0":
                        Console.WriteLine("Exiting the program");
                        return;
                       
                    default:
                        Console.WriteLine("Please select a valid option from the menu.");
                        break;
                }







            }
        }

        private static void SaveDataToFile(Person[] personArray)
        {
            try
            {
                int arrayLength = personArray.Length;
                string[] strArray = new string[arrayLength];

                for (int i = 0; i < arrayLength; i++)
                {
                    strArray[i] = personArray[i].ToString();
                }

                //use append instad of write, to add instad of overwrite,  new data to the file
                File.AppendAllLines(DATA_FILE, strArray);//IOEX

            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: unable to save data from file{ex.Message}");
            }
        }


        private static Person[] LoadDataFromFile()
        {
            try
            {
                //if the file doesn't exist, create it.
                if (!File.Exists(DATA_FILE))
                {
                    File.WriteAllText(DATA_FILE, "");
                }
                string[] dataArray = File.ReadAllLines(DATA_FILE);
                int arrayLength = dataArray.Length;
                Person[] personArray = new Person[arrayLength];

                for (int i = 0; i < dataArray.Length; i++)
                {
                    string[] splitStr = dataArray[i].Split(';');
                    string name = splitStr[0];
                    int age = int.TryParse(splitStr[1], out int validAge) ? int.Parse(splitStr[1]) : -1;
                    string city = splitStr[2];
                    Person p = new Person(name, age, city);
                    personArray[i] = p;
                }

                return personArray;


            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: unable to load data from file: {ex.Message}");
                return null;
            }
        }

        private static void FindPersonYoungerThan()
        {
            Console.Write("Please input the maximum age(from 1 t0 151, inclusive): ");
            string ageStr = Console.ReadLine();

            FindMatchedPerson("age", ageStr);
        }

        private static void FindPersonByName()
        {
            Console.Write("Please input the name(case insensitive, caution for unintended space):");
            string returnVal = Console.ReadLine();
            returnVal = returnVal.Trim();
            returnVal = returnVal.ToUpper();

            FindMatchedPerson("Name", returnVal);

        }



        private static void ListAllPerson()
        {
            try
            {
                Person[] PerosnArray = LoadDataFromFile();//IOException NullPointerException
                if (PerosnArray == null)
                {
                    Console.WriteLine("No data in the file;");
                    return;
                }
                foreach (Person p in PerosnArray)
                {
                    Console.WriteLine(p.ToString());
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        private static void AddPerson()
        {
            List<Person> PersonList = new List<Person>();
            int i = 0;

            while (true)
            {

                Console.WriteLine("Entering person info:");
                Console.WriteLine("Adding new person");
                Console.Write("1.Name:");
                string name = Console.ReadLine();
                Console.Write("2.Age:");
                string ageStr = Console.ReadLine();
                int age = int.TryParse(ageStr, out int validAge) ? validAge : -1;
                Console.Write("3.City:");
                string city = Console.ReadLine();
                try
                {
                    Person p = new Person(name, age, city);//ArgumentEx


                    PersonList.Add(p);
                    i++;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Incorrect data: {ex.Message}");
                }
                Console.WriteLine("Person added");

                Console.WriteLine("Press Exit to finish or any other key to add another person");
                string returnVal = Console.ReadLine();
                if (returnVal.ToString().ToUpper().Equals("EXIT"))
                {
                    break;
                }

            }

            //Convert the list to an array as the SaveDataToFile require an array as a parameter
            Person[] personArray = PersonList.ToArray();

            SaveDataToFile(personArray);


        }


        private static void FindMatchedPerson(string searchKey, string searchValue)
        {
            int matchedRecord = 0;

            Person[] personArray = LoadDataFromFile();
            if (personArray == null)
            {
                Console.WriteLine("No data on the file");
                return;
            }

            for (int i = 0; i < personArray.Length; i++)
            {
                Person p = personArray[i];
                if (searchKey.ToUpper() == "NAME")
                {
                    if (p.Name.ToUpper().Contains(searchValue))
                    {
                        matchedRecord++;
                        Console.WriteLine(p.ToString());
                    }
                }
                else if (searchKey.ToUpper() == "AGE")
                {

                    if (!int.TryParse(searchValue, out int validAge) || validAge < 0 || validAge > 150)
                    {
                        Console.WriteLine("Please input a valid number");
                        return;
                    }



                    if (p.Age <= validAge)
                    {
                        matchedRecord++;
                        Console.WriteLine(p.ToString());
                    }

                }

            }
            Console.WriteLine();
            Console.WriteLine($"Totally found {matchedRecord} matched record(s)");
        }




    }








}


