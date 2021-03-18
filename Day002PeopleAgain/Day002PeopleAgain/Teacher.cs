using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day002PeopleAgain
{



    class Teacher : Person
    {
        public Teacher(string name, int age, string subject, int yearOfExperience) : base(name, age)
        {
            Subject = subject;
            YearOfExperience = yearOfExperience;
        }


		public Teacher(string dataLine) : base()
		{
			string[] values = dataLine.Split(';');
            try
            {

                if (values[0].ToUpper().Equals("TEACHER"))
                {
                    string name = values[1];
                    int.TryParse(values[2], out int validAge);//ex
                    string subject = values[3];
                    int.TryParse(values[4], out int validYOE);//ex

                    Name = name;
                    Age = validAge;
                    Subject = subject;
                    YearOfExperience = validYOE;
                }
            }
            catch (Exception)
            {
                throw new InvalidParameterException($"Invalid data for new teacher: {dataLine}");
            }
        }

		private string _subject;
        private int _yearOfExperience;

        public string Subject { get => _subject; set => _subject = value; }
        public int YearOfExperience { get => _yearOfExperience; set => _yearOfExperience = value; }


        public override string ToString()
        {
            return $"Teacher {Name} is {Age} teaching {Subject} with {YearOfExperience} years of experience";
        }

        public override string ToDataString()
        {
             return string.Join(";","Teacher",Name,Age.ToString(), Subject, YearOfExperience.ToString());
        }

    }


}


   
