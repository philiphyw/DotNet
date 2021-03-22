using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMidtermMulti
{
    class Program
    {
        private static List<Airport> AirportsList = new List<Airport>();


        //Read data from file

        static void ReadDataFromFile()
        {
            try
            {
                string[] linesArray = File.ReadAllLines(@"..\..\data.txt");//System Ex, IO Ex
                if (linesArray.Length == 0)
                {
                    return;//no data or no such file, it's acceptable in businesss requirement
                }

                foreach (string line in linesArray)
                {
                    try
                    {
                        AirportsList.Add(new Airport(line));

                    }
                    catch (InvalidDataException ex)
                    {
                        Console.WriteLine($"Error found while creating airport from data string: {ex.Message}\n >> {line}");

                    }
                }
            }
            catch (IOException ex)
            {
                Logger.LogException?.Invoke("No available data file: " + ex.Message);
            }
            catch (SystemException ex)
            {
                Logger.LogException?.Invoke("No available data file: " +ex.Message);
            }
        }

        static void WriteDataToFile()
        {
            try
            {
                if (AirportsList.Count == 0)
                {
                    Console.WriteLine("There's no data to be saved");
                    return;
                }
                var linesList = AirportsList.Select(p => p.ToDataString());

                File.WriteAllLines(@"..\..\data.txt", linesList); // IOException
                Console.WriteLine($"Finished saving data to the file. Written {linesList.Count()} items");
            }
            catch (Exception ex) when (ex is IOException || ex is SystemException)
            {
                Console.WriteLine("Error saving sorted names data to file: " + ex.Message);
                Logger.LogException?.Invoke("Error saving sorted names data to file: " + ex.Message);
            }

        }

        static void AddAirport()
        {
            List<string> errList = new List<string>();
            Airport airport = new Airport();

            Console.Write("\nEnter Code: ");
            string code = Console.ReadLine();
            try { airport.Code = code; } catch (InvalidDataException ex) { errList.Add(ex.Message); }

            Console.Write("\nEnter City: ");
            string city = Console.ReadLine();
            try { airport.City = city; } catch (InvalidDataException ex) { errList.Add(ex.Message); }

            Console.Write("\nEnter Latitude: ");
            string latitudeStr = Console.ReadLine();
            if (!double.TryParse(latitudeStr, out double latitude))
            {

                errList.Add("Latitude must be a double type number");
                Logger.LogException?.Invoke("Latitude must be a double type number");

            }
            try { airport.Latitude = latitude; } catch (InvalidDataException ex) { errList.Add(ex.Message); }


            Console.Write("\nEnter Longtitude: ");
            string longtitudeStr = Console.ReadLine();
            if (!double.TryParse(longtitudeStr, out double longtitude))
            {
                errList.Add("Longitude must be a double type number");
                Logger.LogException?.Invoke("Longitude must be a double type number");
            }
            try { airport.Longitude = longtitude; } catch (InvalidDataException ex) { errList.Add(ex.Message); }


            Console.Write("\nEnter elevation in meters: ");
            string elevationMetersStr = Console.ReadLine();
            if (!int.TryParse(elevationMetersStr, out int elevationMeters))
            {
                errList.Add("Longitude must be a integer type number");
                Logger.LogException?.Invoke("Latitude must be -90 to 90");
            }
            try { airport.ElevationMeters = elevationMeters; } catch (InvalidDataException ex) { errList.Add(ex.Message); }


            if (errList.Count != 0)
            {
                Console.WriteLine("Invalid input(s): ");
                errList.ForEach(e => Console.WriteLine(e));
                return;
            }

            //add the valid airport to the AirportsList
            AirportsList.Add(airport);
            Console.WriteLine("Airport Added.");

        }

        static void ListAllAirports()
        {
            List<string> list = AirportsList.OrderBy(a => a.Code).Select(a => a.ToString()).ToList();
            list.ForEach(a => Console.WriteLine(a));
            int airportCount = list.Count();
            Console.WriteLine($"Airport(s) on the list: {airportCount}");
        }

        static void FindNearestAirport()
        {

            Console.WriteLine("Please input the airport code:");
            

            if (AirportsList.Count() == 0)
            {
                Logger.LogException?.Invoke("There's no airport on the list");
                throw new InvalidDataException("There's no airport on the list");           
            }
            else if (AirportsList.Count() == 1)
            {
                Logger.LogException?.Invoke("There's no airport on the list");
                throw new InvalidDataException("There's no airport on the list");
            }

          

        }

        static double GetDistance(Airport a1, Airport a2)
        {
            var sCoord = new GeoCoordinate(a1.Latitude,a1.Longitude);
            var eCoord = new GeoCoordinate(a2.Latitude, a2.Longitude);

            return sCoord.GetDistanceTo(eCoord);
        }

        static void PrintNthSuperFibonacci()
        {
            SuperFib sf = new SuperFib();
            Console.Write("\nwhich Nth super-Fibonacci value would you like to see?");
            if (!int.TryParse(Console.ReadLine(), out int choice))//format ex
            {
                Logger.LogException?.Invoke("Invalid input, please input a integer number equals 0 or greater.");
                throw new InvalidDataException("Invalid input, please input a integer number equals 0 or greater.");
            }
            else if (choice < 0)
            {
                Logger.LogException?.Invoke("Invalid input, please input a integer number equals 0 or greater.");
                throw new InvalidDataException("Invalid input, please input a integer number equals 0 or greater.");
            }


            Console.WriteLine($"Value is: {sf[choice]}");
        }


        private static int getChoiceFromMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1.Add Airport");
                Console.WriteLine("2.List all airports");
                Console.WriteLine("3.Find nearest airport by code");
                Console.WriteLine("4.Find airports elevation's standard deviation");
                Console.WriteLine("5.Print Nth super-Fibonacci");
                Console.WriteLine("6.Change log delegates");
                Console.WriteLine("0.Exit");
                Console.Write("Your choice:");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    return choice;
                }
                Console.WriteLine("Invalid input, must be numerical. Try again.");
            }


        }


        static void Main(string[] args)
        {
            //Data will be read once when program begins
            ReadDataFromFile();


            while (true)
            {

                int choice = getChoiceFromMenu();
                switch (choice)
                {
                    case 1:
                        AddAirport();
                        break;
                    case 2:
                        ListAllAirports();
                        break;
                    case 3:
                        FindNearestAirport();
                        break;
                    case 4:
                        //Find airport's elevation standard deviation();
                        break;
                    case 5:
                        try
                        {
                            PrintNthSuperFibonacci();
                        }
                        catch (InvalidDataException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case 6:
                        Logger.ChooseDelegateSetup();
                        break;
                    case 0:
                        //written back once when program is about to finish.
                        WriteDataToFile();
                        Console.WriteLine();
                        return;
                    default:
                        Console.WriteLine("No such option, please try again.");
                        break;
                }
                Console.WriteLine();

            }


        }
    }
}
