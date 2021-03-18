using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day002PeopleAgain
{
    //Tech: delegate, inheritance, inherit, File, linq
    class Program
    {
        const string DATA_FILE = "..\\..\\people.txt";
        private static List<Person> _personList = new List<Person>();

          private static string[] LoadDataFromFile()
        {
            try
            {
                //if the file doesn't exist, create it.
                if (!File.Exists(DATA_FILE))
                {
                    File.WriteAllText(DATA_FILE, "");
                }
                string[] dataArray = File.ReadAllLines(DATA_FILE); //IOException
                
                return dataArray;


            }
            catch (IOException ex)
            {
                LogFailSet.Invoke(ex.Message);
                throw new InvalidParameterException ($"Error: unable to load data from file: {ex.Message}"); 
            }
        }



        // delegate type declaration
       public delegate void LogFailedSetterDelegate(string reason);
        // reference variable of delegate type
        public static LogFailedSetterDelegate LogFailSet;


        private static void LogFailToScreen(string reason) {
            Console.WriteLine($"Added an error to the screen: {reason}");
        }

        private static void LogFailToFile(string reason)
        {
            try
            {
                File.AppendAllText("..\\..\\FailLog.txt", $"Error found: {reason}");
                Console.WriteLine($"Added an error to the file: {reason}");
            }
            catch (Exception)
            {
                Console.WriteLine("Error on the LogFailToFile() function, failed to add log to the file.");
            }
        }




        private static void InstantiateData(string[] sourceDataArr)
        {
            
            List<string> errorList = new List<string>();

            if (sourceDataArr == null)
            {
                LogFailSet("No data is loaded");
                throw new InvalidParameterException("No data is loaded");
            }

            for (int i = 0; i < sourceDataArr.Length; i++)
            {
                string dataLine = sourceDataArr[i];
                string type = dataLine.Split(';')[0];
                try
                {
                    switch (type.ToUpper())
                    {

                        case "PERSON":
                            _personList.Add(new Person(dataLine));
                            break;
                        case "TEACHER":
                            _personList.Add(new Teacher(dataLine));
                            break;
                        case "STUDENT":
                            _personList.Add(new Student(dataLine));
                            break;
                        default:
                            errorList.Add($"Unsupported data type on the line {i + 1}");
                            break;
                    }

                    //Console.WriteLine($"Finished loading data from line{i+1}");
                }
                catch(Exception ex) when (ex is NullReferenceException || ex is InvalidParameterException || ex is ArgumentException) 
                {
                    errorList.Add($"{ex.Message} on the line {i + 1}");
                }

               
            }

            if (errorList.Count>0)
            {
                Console.WriteLine("Error(s) found:");
                Console.WriteLine();
                for (int j = 0; j < errorList.Count; j++)
                {
                    //Console.WriteLine(errorList[j].ToString());
                    LogFailSet(errorList[j].ToString());

                }

                Console.WriteLine();
            }


            for (int i = 0; i < _personList.Count; i++)
            {
                Console.WriteLine(_personList[i].ToString());
            }
            
        }



        private static List<Person> GetPersonByType(string typeName, int sortField)
        {
            var students = from p in _personList
                           where p.ToDataString().Split(';')[0].Equals(typeName)
                           orderby p.ToDataString().Split(';')[sortField] ascending
                           select p;

            List<Person> resultList = students.ToList<Person>();

            return resultList;
        }




        static void Main(string[] args)
        {
            //Based on user's input , assgin method(s) to delegate handler
            Console.WriteLine("Where would you like to log setters errors?");
            Console.WriteLine("1 - screen only");
            Console.WriteLine("2-screen and file");
            Console.WriteLine("3-do not log");

            string FailLogChoice = Console.ReadLine();

            switch (FailLogChoice) 
            {
                case "1":
                    LogFailSet = LogFailToScreen;
                    Console.WriteLine("Confirmed. All erorrs will log to screen");
                    break;
                case "2":
                    LogFailSet = LogFailToScreen;
                    LogFailSet += LogFailToFile;
                    Console.WriteLine("Confirmed. All erorrs will log to screen and the file");
                    break;
                case "3":
                    Console.WriteLine("Confirmed. No log for errors");
                    break;
                default:
                    Console.WriteLine("Invalid choice, no log log for errors.");
                    break;
            }


            string[] dataFile = LoadDataFromFile();
            InstantiateData(dataFile);


            List<Person> pList;

            // print perosn list
            Console.WriteLine("\nPrint out People list:\n");
            pList = GetPersonByType("Person",2);
            pList.ForEach((p) => Console.WriteLine(p.ToString()));

            // print teacher list
            Console.WriteLine("\nPrint out Teachers list:\n");
            pList = GetPersonByType("Teacher",2);
            pList.ForEach((p) => Console.WriteLine(p.ToString()));

            // print student list
            Console.WriteLine("\nPrint out Students list:\n");
            pList = GetPersonByType("Student",2);
            pList.ForEach((p) => Console.WriteLine(p.ToString()));
            Console.WriteLine();



            //get average , median  and standard deviation of GPA
            List < Student > stuList = new List<Student>();
            pList.ForEach(p => stuList.Add((Student)p));
            List<double> gpaList = new List<double>();
            stuList.ForEach(s => gpaList.Add(s.GPA));




            double avgGPA = gpaList.Average();
            double minGPA = gpaList.Min();
            double maxGPA = gpaList.Max();
            Console.WriteLine($"The average student's GPA: {avgGPA}");
            Console.WriteLine($"The median student's GPA: { (minGPA+maxGPA)/2}");

            //standard deviation ....so complicate..
            double sum = gpaList.Sum(v => (v - avgGPA) * (v - avgGPA));
            double denominator = gpaList.Count - 1;
            double sdGPA = denominator > 0.0 ? Math.Sqrt(sum / denominator) : -1;
            sdGPA = Math.Round(sdGPA,2);

            Console.WriteLine($"The standard deviation of student's GPA: { sdGPA}");

            //use linq to sort _personList by name and write to file ..\\..\\byname.txt
            Console.WriteLine();
            var SortByName = from p in _personList
                             orderby p.Name
                             select p.ToDataString();

            string[] SortByNameArray = SortByName.ToArray();

            File.WriteAllLines("..\\..\\byname.txt", SortByNameArray);

            Console.WriteLine("Sorted person by name and wrote into the file byname.txt");

        }
    }
}
