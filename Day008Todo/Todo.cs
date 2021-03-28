using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day008Todo
{
    class Todo
    {
        /*
    String Task; // 1-100 characters, made up of uppercase and lowercase letters, digits, space, %_-(),./\ only
	int Difficulty; // 1-5, as slider
	DateTime DueDate; // year 1900-2100 both inclusive, use formatted field
	Status; // define an enumeration - one of: Pending, Done, Delegated - matches the ComboBox in GUI
         
         
         */



        private string _task;
        private int _difficulty;
        private DateTime _dueDate;
        private Status _status;



        public Todo(string task, int difficulty, DateTime dueDate, Status status)
        {
            Task = task;
            Difficulty = difficulty;
            DueDate = dueDate;
            Status = status;
        }
        public Todo(string dataString)
        {
            string[] Fields = dataString.Split(';');
            List<string> errList = new List<string>();


            if (Fields.Length != 4)
            {
                throw new InvalidDataException("Data String didn't match the required fields");
            }

            //validate task
            try { Task = Fields[0]; } catch (InvalidDataException ex) { errList.Add(ex.Message); }

            //validate Difficulty
            try
            {
                int.TryParse(Fields[1], out int difficulty);//FormatEx
                Difficulty = difficulty;

            }
            catch (Exception ex) when (ex is InvalidDataException || ex is FormatException)
            { errList.Add(ex.Message); }

            //validate DueDate
            try
            {
                DateTime.TryParse(Fields[2], out DateTime dueDate);//FormatEx
                DueDate = dueDate;

            }
            catch (Exception ex) when (ex is InvalidDataException || ex is FormatException)
            { errList.Add(ex.Message); }

            //validate Status
            try
            {

                Status statusValue = SetEnumStatusValue(Fields[3]);//FormatEx
                Status = statusValue;

            }
            catch (Exception ex) when (ex is InvalidDataException || ex is FormatException)
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


        public Todo()
        {
        }


        public string Task
        {
            get => _task; set
            {
                if (!Regex.IsMatch(value, @"^[\w\d\s\%_\-\(\)\,\.\/\\]{1,100}$"))
                {
                    throw new InvalidDataException("Task can only contain uppercase and lowercase letters, digits, space, %_-(),./\\");
                }
                _task = value;
            }
        }
        public int Difficulty
        {
            get => _difficulty; set
            {
                if (value < 1 || value > 5)
                {
                    throw new InvalidDataException("Difficulty must be an integer from 1 to 5");
                }
                _difficulty = value;
            }
        }
        public DateTime DueDate
        {
            get => _dueDate; set
            {
                if (value.Year < 1900 || value.Year > 2100)
                {
                    throw new InvalidDataException("Due Date must between 1900 and 2100");
                }
                _dueDate = value;
            }
        }
        public Status Status { get => _status; set => _status = value; }

        public string GetDateInCultureFormat
        {
            get => $"{_dueDate:d}";
                      
        }

        private Status SetEnumStatusValue(string statusStr)
        {
            try
            {
                Status statusValue = (Status)Enum.Parse(typeof(Status), statusStr);
                return statusValue;
            }
            catch (FormatException ex)
            {

                throw new InvalidDataException($"Invalid value in the Status field: {ex.Message}");
            }
            

        }

        public override string ToString()
        {
            return $"{Task} before {DueDate:yyyy-MM-dd}/ diffculty is {Difficulty}, {Status}";
        }

        public string ToDataString()
        {
             return $"{Task};{Difficulty};{DueDate:yyyy-MM-dd};{Status}";
        }
    }







    enum Status { PENDING, DONE, DELEGATED }
}
