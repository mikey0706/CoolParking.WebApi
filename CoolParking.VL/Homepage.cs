using CoolParking.BL.Models;
using CoolParking.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolParking.VL
{
   public class Homepage
    {
        private ParkingService _parking = new ParkingService(new TimerService(), new TimerService(), new LogService("Transaction.log"));

        public void AdminAccount() 
        {
            int opt = 100;

            while (opt != 0)
            {
                Console.WriteLine("Select your action by printing specific number.");
                Console.WriteLine("1 - Display the current Parking balance;");
                Console.WriteLine("2 - Display the number of free parking spaces (X of Y are free);");
                Console.WriteLine("3 - Display the amount of earned units for the current period (before recording in the log);");
                Console.WriteLine("4 - Display all Transactions for the current period (before recording them in the log);");
                Console.WriteLine("5 - Display the Transactions history (by reading the data from the Transactions.log file)");
                Console.WriteLine("6 - Display the list of vehicles located on Parking;");
                Console.WriteLine("7 - Return to main menu.");
                Console.WriteLine("0 - Exit;");

                opt = Convert.ToInt32(Console.ReadLine());

                if (opt == 1)
                {
                    Console.WriteLine($"Current parking ballance is: {_parking.GetBalance()}");
                }
                else
                if (opt == 2)
                {
                    Console.WriteLine($"Spaces {_parking.GetFreePlaces()} of {Settings.ParkingCapacity} are free");
                }
                else
                if (opt == 3)
                {
                    Console.WriteLine($"The ammount of earned units for the current period is {_parking.GetBalance()}");
                }
                else
                if (opt == 4)
                {
                    var arr = _parking.GetLastParkingTransactions();

                    Console.WriteLine("All transactions for the current period: ");
                    Console.WriteLine("Time | Vehicle Id | Sum");

                    for (int i = 0; i < arr.Length; i++)
                    {
                        var item = arr[i];
                        Console.WriteLine($"{item.TarnsactionTime} - {item.VehicleId} - {item.Sum}");
                    }
                }
                else
                if (opt == 5)
                {
                    Console.WriteLine("The transaction history.");
                    Console.WriteLine($"{_parking.ReadFromLog()}");
                }
                else
                if (opt == 6)
                {
                    Console.WriteLine("List of vehicles located on Parking");

                    foreach (var item in _parking.GetVehicles())
                    {
                        Console.WriteLine($"{item.Type} - {item.Id} - {item.Balance}");
                    }
                }
                else
                if (opt == 7)
                {
                    Program.Program.Menu();
                }
                else
                if (opt == 0)
                {
                    Environment.Exit(0);
                }
            }
         }

        public void UserAccount()
        {
            int opt = 100;

            while (opt != 0)
            {
                Console.WriteLine("1 - Display the number of free parking spaces (X of Y are free);");
                Console.WriteLine("2 - Put the Vehicle on Parking;");
                Console.WriteLine("3 - Pick up the Vehicle from Parking;");
                Console.WriteLine("4 - Top up the balance of a particular Vehicle.");
                Console.WriteLine("5 - Return to main menu.");
                Console.WriteLine("0 - Exit;");

                opt = Convert.ToInt32(Console.ReadLine());

                if (opt == 1)
                {
                    Console.WriteLine($"Spaces {_parking.GetFreePlaces()} of {Settings.ParkingCapacity} are free");
                }
                else
                if (opt == 2)
                {
                    Console.WriteLine("Enter the number from your vehicle's license plate (For example: 'AA-0000-AA')");
                    string id = Console.ReadLine();

                    Console.WriteLine("Enter the type of your vehicle");
                    Console.WriteLine($"1 - {VehicleType.PassengerCar}");
                    Console.WriteLine($"2 - {VehicleType.Truck}");
                    Console.WriteLine($"3 - {VehicleType.Bus}");
                    Console.WriteLine($"4 - {VehicleType.Motorcycle}");

                    VehicleType type = new VehicleType();

                    int t = Convert.ToInt32(Console.ReadLine());

                    if (t == 1)
                    {
                        type = VehicleType.PassengerCar;
                    }
                    else
                    if (t == 2)
                    {
                        type = VehicleType.Truck;
                    }
                    else
                    if (t == 3)
                    {
                        type = VehicleType.Bus;
                    }
                    else
                    if (t == 4)
                    {
                        type = VehicleType.Motorcycle;
                    }

                    Console.WriteLine("Enter your balance.");

                    decimal balance = Convert.ToDecimal(Console.ReadLine());

                    _parking.AddVehicle(new Vehicle(id, type, balance));
                }
                else
                if (opt == 3)
                {
                    Console.WriteLine("To pick up your vehicle enter the number of your lcense plate");
                    string id = Console.ReadLine();
                    _parking.RemoveVehicle(id);

                }
                else
                if (opt == 4)
                {
                    Console.WriteLine("To top up the balance of your vehicle enter the number of your lcense plate");

                    string id = Console.ReadLine();

                    Console.WriteLine("Now enter your balance.");

                    decimal balance = Convert.ToDecimal(Console.ReadLine());

                    _parking.TopUpVehicle(id, balance);
                }
                else
                if (opt == 5) 
                {
                    Program.Program.Menu();
                }
                else
                if (opt == 0)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
