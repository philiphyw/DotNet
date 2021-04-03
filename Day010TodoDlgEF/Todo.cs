using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day010TodoDlgEF
{
    public class Todo
    {
        /*
    String Task; // 1-100 characters, made up of uppercase and lowercase letters, digits, space, %_-(),./\ only
	int Difficulty; // 1-5, as slider
	DateTime DueDate; // year 1900-2100 both inclusive, use formatted field
	Status; // define an enumeration - one of: Pending, Done, Delegated - matches the ComboBox in GUI
         
         
         */


        private int _id;
        private string _task;
        private int _difficulty;
        private DateTime _dueDate;
        private EStatus _tStatus;



        public Todo(string task, int difficulty, DateTime dueDate, EStatus status)
        {
            Task = task;
            Difficulty = difficulty;
            DueDate = dueDate;
            TStatus = status;
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

                EStatus statusValue = SetEnumStatusValue(Fields[3]);//FormatEx
                TStatus = statusValue;

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

        [Required]
        public int Id { get => _id; set => _id = value; }


        [Required]
        [StringLength(100)]
        public string Task
        {
            get => _task; set
            {
                if (!Regex.IsMatch(value, @"^[\w\d\s\%_\-\(\)\,\.\/\\]{1,100}$"))
                {
                    throw new InvalidDataException("Task can only contain no more than 100 characters, accept uppercase and lowercase letters, digits, space, %_-(),./\\");
                }
                _task = value;
            }
        }

        [Required]
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

        [Required]
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

        [NotMapped]
        public string GetDueDateString
        {
            get => $"{DueDate:d}";
        }
        [Required]
        public EStatus TStatus { get => _tStatus; set => _tStatus = value; }

        public string GetDateInCultureFormat
        {
            get => $"{_dueDate:d}";

        }
       

        private EStatus SetEnumStatusValue(string statusStr)
        {
            try
            {
                EStatus statusValue = (EStatus)Enum.Parse(typeof(EStatus), statusStr);
                return statusValue;
            }
            catch (FormatException ex)
            {

                throw new InvalidDataException($"Invalid value in the Status field: {ex.Message}");
            }


        }

        public override string ToString()
        {
            return $"{Task} before {DueDate:yyyy-MM-dd}/ diffculty is {Difficulty}, {TStatus}";
        }

        public string ToDataString()
        {
            return $"{Task};{Difficulty};{DueDate:yyyy-MM-dd};{TStatus}";
        }
    }







    public enum EStatus { PENDING =1, DONE=2, DELEGATED=3 }
}
