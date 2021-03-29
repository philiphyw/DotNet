using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Day010FinalFlights
{
    public class Flight
    {

        public Flight() { }
        public Flight(DateTime onDate, String fromCode, String toCode, FType type, int passengers)
        {
           OnDate = onDate;
            FromCode = fromCode;
            ToCode = toCode;
            FType = type;
            Passengers = passengers;
        }


        public Flight(string dataString)
        {
            string[] Fields = dataString.Split(';');
            List<string> errList = new List<string>();


            if (Fields.Length != 5)
            {
                throw new InvalidDataException("Data String didn't match the required fields");
            }

            //validate OnDate
            try 
            { 
                OnDate = DateTime.ParseExact(Fields[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);// Ex
            } 
            catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException)
            {
                errList.Add(ex.Message); 
            }

            //validate fromCode 3-5 uppercase letters
            try
            {
                FromCode = Fields[1];
            }
            catch (InvalidDataException ex)
            { errList.Add(ex.Message); }

            //validate toCode 3-5 uppercase letters
            try
            {
               ToCode = Fields[2];
            }
            catch (InvalidDataException ex)
            { errList.Add(ex.Message); }

            //validate FType
            try
            {
                FType = (FType)Enum.Parse(typeof(FType), Fields[3], true);// ex

            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            { errList.Add(ex.Message); }

            //validate passengers
            try
            {
                Passengers = int.Parse(Fields[4]);//ex
            }
            catch (FormatException ex)
            { errList.Add(ex.Message); }


            if (errList.Count > 0)
            {
                string errStr = "";

                foreach (string err in errList)
                {
                    errStr += err + "\n";
                }

                throw new InvalidDataException(errStr);
            }

        }


        private DateTime _onDate;
        private string _fromCode;
        private string _toCode;
        private FType _fType;
        private int _passengers;

        public DateTime OnDate { get => _onDate; set => _onDate = value; }
        public string GetOnDate { get => $"{OnDate:d}"; }
        public string FromCode { get => _fromCode; set
            {
                if (!Regex.IsMatch(value, @"^[A-Z]{3,5}$"))
                {
                    throw new InvalidDataException("FromCode must be 3-5 letters in uppercase");
                }
                _fromCode = value;
            }
        }
        public string ToCode { get => _toCode; set
            {
                if (!Regex.IsMatch(value, @"^[A-Z]{3,5}$"))
                {
                    throw new InvalidDataException("ToCode must be 3-5 letters in uppercase");
                }
                _toCode = value;
            }
        }
        public FType FType { get => _fType; set => _fType = value; }

        public int Passengers { get => _passengers; set
            {
                if (value<0 || value >200)
                {
                    throw new InvalidDataException("Passenger must be 1 - 200");
                }
                _passengers = value;
            }
        }

        public override string ToString()
        {
            return $"{OnDate:yyyy-MM-dd};{FromCode};{ToCode};{FType};{Passengers}";
        }

        public string ToDataString()
        {
            return $"{OnDate:yyyy-MM-dd};{FromCode};{ToCode};{FType};{Passengers}";
        }
    }

    public enum FType
    {
        Domestic, International, Private


    }
}
