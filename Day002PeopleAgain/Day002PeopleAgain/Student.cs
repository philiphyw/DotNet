using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day002PeopleAgain
{
    


    class Student : Person
    {
        public Student(string name, int age, string program, double gpa) : base(name, age) 
        {
            Program = program;
            GPA = gpa;
        }

        public Student(string dataLine) : base()
        {
            string[] values = dataLine.Split(';');
            try
            {

                if (values[0].ToUpper().Equals("STUDENT"))
                {
                    string name = values[1];
                    int.TryParse(values[2], out int validAge);//ex
                    string subject = values[3];
                    double.TryParse(values[4], out double validgpa);//ex

                    Name = name;
                    Age = validAge;
                    Program = subject;
                    GPA = validgpa;
                }
            }
            catch (Exception)
            {
                throw new InvalidParameterException($"Invalid data for new student: {dataLine}");
            }
        }

        private string _program;
        private double _gpa;

        public string Program { get => _program; set => _program = value; }
        public double GPA { get => _gpa; set => _gpa = Math.Round(value,2); }


        public override string ToString() {
            return $"Student {Name} is {Age} studying {Program} with GPA {GPA}";
        }
        public override string ToDataString()
        {
             return string.Join(";","Student",Name,Age.ToString(),Program,GPA.ToString());
        }

        public static explicit operator Student(List<Person> v)
        {
            throw new NotImplementedException();
        }
    }


}


   
