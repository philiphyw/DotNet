using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DayMidtermMulti
{
    class Airport
    {


        //		Define the following class Airport.



        //- Add ToString implementation to display data in user-friendly manner.
        //- Add ToDataString implementation to save data to file as a single line semicolon-separated.

        //- Add constructor that takes 5 parameters to initialize the 5 fields.
        public Airport(string code, string city, double latitude, double longitude, int elevationMeters)
        {
            Code = code;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
            ElevationMeters = elevationMeters;
        }

        //- Add constructor that takes 1 string parameter which is a line in file that it will parse and use to create Airport object.
        public Airport(string dataString)
        {
            string[] data = dataString.Split(';');
            if (data.Length != 5)
            {
                Logger.LogException?.Invoke("Data string must have 5 fields");
                throw new InvalidDataException("Data string must have 5 fields");
            }
            string code = data[0];
            string city = data[1];
            if (!double.TryParse(data[2], out double latitude))//Format Ex
            {
                Logger.LogException?.Invoke("Latitude must be an double type number");
                throw new InvalidDataException("Latitude must be an double type number");
            }
            if (!double.TryParse(data[3], out double longitude))//Format Ex
            {
                Logger.LogException?.Invoke("Longitude must be an double type number");
                throw new InvalidDataException("Longitude must be an double type number");
            }
            if (!int.TryParse(data[4], out int elevationMeters))//Format Ex
            {
                Logger.LogException?.Invoke("Elevation meters must be an integer type number");
                throw new InvalidDataException("Elevation meters must be an integer type number");
            }

            Code = code;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
            ElevationMeters = elevationMeters;

        }

        public Airport() { }


        private string _code;
        private string _city;
        private double _latitude, _longitude;
        private int _elevationMeters;


        //- Add getters and setters with verification that throw InvalidDataException on failure.

        // exactly 3 uppercase letters
        public string Code
        {
            get => _code; set
            {
                if (!Regex.IsMatch(value, @"^[A-Z][A-Z][A-Z]$"))
                {
                    Logger.LogException?.Invoke("Code must be exactly 3 uppercase letters");
                    throw new InvalidDataException("Code must be exactly 3 uppercase letters");
                }
                _code = value;
            }
        }


        // 1-50 characters, no semicolons
        public string City
        {
            get => _city; set
            {
                if (!Regex.IsMatch(value, @"^[a-zA-Z0-9][^;]{1,50}$"))
                {
                    Logger.LogException?.Invoke("City must be 1-50 characters, no semicolons");
                    throw new InvalidDataException("City must be 1-50 characters, no semicolons");
                }
                _city = value;
            }
        }

        //-90 to 90
        public double Latitude
        {
            get => _latitude; set
            {
                if (value < -90 || value > 90)
                {
                    Logger.LogException?.Invoke("Latitude must be -90 to 90");
                    throw new InvalidDataException("Latitude must be -90 to 90");
                }
                _latitude = value;
            }
        }

        //  -180 to 180
        public double Longitude
        {
            get => _longitude; set
            {
                if (value < -180 || value > 180)
                {
                    Logger.LogException?.Invoke("Longitude must be -180 to 180");
                    throw new InvalidDataException("Longitude must be -180 to 180");
                }
                _longitude = value;
            }
        }

        //-1000 to 10000
        public int ElevationMeters
        {
            get => _elevationMeters; set
            {
                if (value < -1000 || value > 10000)
                {
                    Logger.LogException?.Invoke("Elevation eeters must be -1000 to 10000");
                    throw new InvalidDataException("Elevation eeters must be -1000 to 10000");
                }
                _elevationMeters = value;
            }
        }

        public override string ToString()
        {
            return $"{Code} in {City} at {Latitude} lat / {Longitude} lng at {ElevationMeters}m elevation";
        }

        public virtual string ToDataString()
        {
            return $"{Code};{City};{Latitude};{Longitude};{ElevationMeters}";
        }
    }
}
