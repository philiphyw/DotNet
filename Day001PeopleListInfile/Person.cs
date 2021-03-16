using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day001PeopleListInfile
{
    class Person
    {
        
         //Build a constructor like in JAVA. Or just use an initializer:  Person p = new Person{Name="Jim", Age=24, City= "Montreal"}, but it may NOT initialize all the fields..
        public Person(string name, int age, string city)
        {
            // don't use _name = name; otherwise it will skip the setter and apply the value to the field(sotrage) directly
            Name = name;
            Age = age;
            City = city;
        }
    

        private string _name;//this is a field(storage)
        private int _age;
        private string _city;


        //C# way encapsulation 
        public string Name //this is a property
        {

            get
            {
                return _name;
            }
            set
            {
                if (value.Length<2 || value.Length>100 || value.Contains(";"))
                {
                    throw new ArgumentException("Name must be 2-100 characters long, no semicolons");
                }
                _name = value;
            }
        }

        public int Age
        {

            get
            {
                return _age;
            }
            set
            {
                if (value<0||value>150)
                {
                    throw new ArgumentException("Age must be 0-150");
                }
                _age = value;
            }
        }

        public string City
        {

            get
            {
                return _city;
            }
            set
            {
                if (value.Length < 2 || value.Length > 100 || value.Contains(";"))
                {
                    throw new ArgumentException("City  must be 2-100 characters long, no semicolons");
                }
                _city = value;
            }
        }


        public override string ToString() => ($"{Name};{Age};{City}"); 
        

    }
}
            




