using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day002PeopleAgain
{
    public class Person
    {

        private string _name;
        private int _age;

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }


        public Person() { }

        public Person(string dataLine)
        {
            string[] values = dataLine.Split(';');

            try
            {

                if (values[0].ToUpper().Equals("PERSON"))
                {
                    string name = values[1];
                    int.TryParse(values[2], out int validAge);//ex
                    Name = name;
                    Age = validAge;
                }
            }
            catch (Exception)
            {
                throw new InvalidParameterException($"Invalid data for new person: {dataLine}");
            }

        }




        public string Name { get => _name; 
            
            set{
                if (!Regex.IsMatch(value, @"^[^;]{2,100}$"))
                {
                    throw new ArgumentException("Name must be 2-100 characters long, no semicolons");
                }
                _name = value;
            } }
        public int Age
        {
            get => _age; set
            {
                if (value <0 || value >150)
                {
                    throw new ArgumentException("Age must be 0-150");
                }
                _age = value;
            } }

        public override string ToString()
        {
            return $"Person {Name} is {Age}";
        }

        public virtual string ToDataString() 
        {
            return $"Person;{Name};{Age}";
        }
    }
}
