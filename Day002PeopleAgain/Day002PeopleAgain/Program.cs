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

        private static List<Student> GetPersonByLinq(string typeName) {
            var list = _personList.Where(p => p is Student).Cast<Student>().ToList();
            return list;
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
            List<Student> stuList = _personList.Where(p => p is Student).Cast<Student>().ToList();
            double gpaSum = stuList.Sum(s => s.GPA);
            double gpaAvg = stuList.Average(s => s.GPA);
            double gpaMax = stuList.Max(s => s.GPA);
            double gpaMin = stuList.Min(s => s.GPA);
            //GPA Average
            Console.WriteLine($"The average of student's GPA: {gpaAvg:0.#}");//show 1 decimal places.

            //GPA Median, TODO: review the definition/calcuation of median 
            //Sort the list before identify which value is the median
            stuList = stuList.OrderBy(s => s.GPA).ToList();
            
            stuList.ForEach(s => Console.WriteLine(s.ToDataString()));
            double median;
            int middleIndex = stuList.Count / 2;
            if (stuList.Count() % 2 == 1)//odd number of student
            {
                median = stuList[middleIndex].GPA;
            } else //even number
            {
                median = (stuList[middleIndex -1].GPA + stuList[middleIndex].GPA)/2;
            }

            Console.WriteLine($"The median of student's GPA: {median:0.##}");//show 2 decimal places.

            //standard deviation ....so complicate..
            double gpaSumOfSqrt = stuList.Sum(s => (s.GPA - gpaAvg) * (s.GPA - gpaAvg));
            double sdGPA = Math.Sqrt(gpaSumOfSqrt / stuList.Count);

            Console.WriteLine($"The standard deviation of student's GPA: {sdGPA:0.###}");//show 3 decimal places.

            //use linq to sort _personList by name and write to file..\\..\\byname.txt
            Console.WriteLine();
            var SortByName = from p in _personList
                             orderby p.Name
                             select p.ToDataString();

            string[] SortByNameArray = SortByName.ToArray();

            File.WriteAllLines("..\\..\\byname.txt", SortByNameArray);

            Console.WriteLine("Sorted person by name and wrote into the file byname.txt");



            //use linq to make it simpler
            Console.WriteLine("\nuse linq to make selection simpler\n");
            var list = _personList.Where(p => p is Student).Cast<Student>().ToList();
            List<Student> sortedStudent = list.OrderBy(s => s.GPA).ToList();
            sortedStudent.ForEach(s => Console.WriteLine(s.ToDataString()));
        }
    }
}
