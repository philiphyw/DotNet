using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day007TravelList
{
    class Trip
    {
        public Trip(string destination, string name, string passport, DateTime departure, DateTime @return)
        {
            Destination = destination;
            Name = name;
            Passport = passport;
            SetDepartureReturnDate(departure, @return);
        }

        public Trip(string dataString)
        {
            string[] Fields = dataString.Split(';');
            List<string> errList = new List<string>();


            if (Fields.Length!=5)
            {
                throw new InvalidDataException("Data String didn't match the required fields");
            }

            try { Name = Fields[0]; } catch (InvalidDataException ex)  { errList.Add(ex.Message); }
            try { Passport = Fields[1]; } catch (InvalidDataException ex) { errList.Add(ex.Message); }
            try { Destination = Fields[2]; } catch (InvalidDataException ex) { errList.Add(ex.Message); }

            //handle date exceptions
            try { SetDepartureReturnDate(DateTime.Parse(Fields[3]), DateTime.Parse(Fields[4])); } catch (Exception ex) when(ex is InvalidDataException || ex is FormatException) { errList.Add(ex.Message); }
           

            if (errList.Count>0)
            {
                string errStr = "";

                foreach (string err in errList)
                {
                    errStr += err + "\n";
                }

                throw new InvalidDataException(errStr);
            }

        }
        public Trip()
        {
        }


        private string _destination;
        private string _name;
        private string _passport;
        private DateTime _departure;
        private DateTime _return;



        public string Destination { get => _destination; set 
            {
                if (!Regex.IsMatch(value, @"^[\w\d\s\.\-]{2,30}$"))
                {
                    throw new InvalidDataException("Invalid destination: must be 2-30 characters ");
                }

                _destination = value; 
            } 
        }
        public string Name { get => _name; set
            {
                if (!Regex.IsMatch(value, @"^[\w\d\s\.\-]{2,30}$"))
                {
                    throw new InvalidDataException("Invalid name: must be 2-30 characters ");
                }

                _name = value;
            }
        }
        public string Passport { get => _passport; set
            {
                if (!Regex.IsMatch(value, @"^[A-Z]{2}[\d]{6}$"))
                {
                    throw new InvalidDataException("Invalid passport: must be 2 uppercase letters plus 6 number");
                }

                _passport = value;
            }

        }
        // need to combine and validate Departure and return together
        public DateTime Departure { get => _departure;  }
        public DateTime Return { get => _return;  }

        private void SetDepartureReturnDate(DateTime departureDate, DateTime returnDate)
        {
            if (departureDate == null || returnDate == null)
            {
                throw new InvalidDataException("Departure/Return date must NOT be null");
            }
            else if (departureDate.CompareTo(returnDate) <=0)
            {
                _departure = departureDate;
                _return = returnDate;
            }
            else 
            {
                throw new InvalidDataException("Return date must be same as or later than departure date");
            }

        }



        public override string ToString()
        {
            return $"{Name}, {Passport} to {Destination} on {Departure:d} and return on {Return:d}";
        }

        public  string ToDataString()
        {
            return $"{Name};{Passport};{Destination};{Departure:yyyy-MM-dd};{Return:yyyy-MM-dd}";
        }
    }
}
