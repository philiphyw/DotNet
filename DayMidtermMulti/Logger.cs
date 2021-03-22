using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMidtermMulti
{
    class Logger
    {
        public delegate void LoggerDelegate(string reason);

        public static LoggerDelegate LogException;

        public static void LogToFile(string msg)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                File.AppendAllText(@"..\..\events.log",$"{timeStamp} -- {msg}\n");
            }
            catch (Exception ex) when (ex is IOException || ex is SystemException)
            {
                Console.WriteLine("Failed to log to file: " + ex.Message);
            }
        }

        public static void LogToConsole(string msg)
        {
            Console.WriteLine("*** " + msg);
        }

        public static void ChooseDelegateSetup()
        {
            LogException = null;


            Console.WriteLine();
            Console.WriteLine("> Press enter key to set no log");
            Console.WriteLine("> Press 1 to log to console");
            Console.WriteLine("> Press 2 to log to file");
            Console.WriteLine("> Press 1,2 to log to both console and fiel");
;
            Console.Write("Your choice:");

            string returnVal = Console.ReadLine();
            if (returnVal == null)
            {
                Console.WriteLine("Set to no log");
                return;
            }
            switch (returnVal)
            {
                  case "1":
                    LogException =  LogToConsole;
                    Console.WriteLine("Logging to console only");
                    break;
                case "2":
                    LogException = LogToFile;
                    Console.WriteLine("Logging to file only");
                    break;
                case "1,2":
                    LogException = LogToConsole;
                    LogException += LogToFile;
                    Console.WriteLine("Logging to console and file");
                    break;
                default:
                    Console.WriteLine("Invalid option. Will set to no log by default");
                    break;
            }
        }
    }
}
