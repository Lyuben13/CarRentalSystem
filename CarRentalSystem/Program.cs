using System;
using System.Collections.Generic;
using System.Linq;
using CarRentalSystem.Models;
using CarRentalSystem.Services;

namespace CarRentalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string path = "cars.csv";
            var reader = new CarFileReader(path);
            var writer = new CarFileWriter(path);
            var service = new CarRentalService(reader.LoadCars());

            Console.WriteLine("Welcome to Car Rental System!");
            ShowHelp();

            while (true)
            {
                Console.Write("\nCommand> ");
                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input))
                    continue;

                var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLower();
                var arg = parts.Length > 1 ? parts[1] : "";

                try
                {
                    switch (cmd)
                    {
                        case "help":
                            ShowHelp();
                            break;

                        case "listall":
                            ListCars(service.GetAllCars(), "All cars:");
                            break;

                        case "listavail":
                        case "listavailable":
                            ListCars(service.ListAvailable(), "Available cars:");
                            break;

                        case "add":
                            AddCarInteractive(service);
                            break;

                        case "edit":
                            EditCarInteractive(service, arg);
                            break;

                        case "remove":
                            if (int.TryParse(arg, out int remId))
                            {
                                service.RemoveCar(remId);
                                Console.WriteLine($"Car {remId} flagged as removed.");
                            }
                            else Console.WriteLine("Usage: Remove <CarId>");
                            break;

                        case "rent":
                            {

                                var a = arg.Split(' ', 2);
                                if (a.Length == 2 && int.TryParse(a[0], out int rentId))
                                {
                                    service.RentCar(rentId, a[1]);
                                    Console.WriteLine($"Car {rentId} rented to {a[1]}.");
                                }
                                else Console.WriteLine("Usage: Rent <CarId> <RenterName>");
                            }
                            break;

                        case "return":
                            if (int.TryParse(arg, out int retId))
                            {
                                service.ReturnCar(retId);
                                Console.WriteLine($"Car {retId} returned.");
                            }
                            else Console.WriteLine("Usage: Return <CarId>");
                            break;

                        case "search":

                            var sa = arg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                            if (sa.Length == 2)
                            {
                                if (sa[0].ToLower() == "id" && int.TryParse(sa[1], out int sid))
                                {
                                    var c = service.GetCarById(sid);
                                    Console.WriteLine(c != null ? c.GetDetails() : $"No car with Id {sid}");
                                }
                                else if (sa[0].ToLower() == "model")
                                {
                                    var found = service.SearchByModel(sa[1]);
                                    ListCars(found, $"Cars matching model \"{sa[1]}\":");
                                }
                                else Console.WriteLine("Usage: Search id <CarId>  OR  Search model <text>");
                            }
                            else Console.WriteLine("Usage: Search id <CarId>  OR  Search model <text>");
                            break;

                        case "save":
                            writer.SaveCars(service.GetAllCars());
                            Console.WriteLine("Data saved to CSV.");
                            break;

                        case "exit":
                            writer.SaveCars(service.GetAllCars());
                            Console.WriteLine("Saved and exiting.");
                            return;

                        default:
                            Console.WriteLine($"Unknown command: {cmd}. Type Help to see list.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine(@"
Available commands:
  Help                  – show this help
  ListAll               – list all cars
  ListAvailable         – list only available cars
  Add                   – add a new car
  Edit <CarId>          – edit existing car
  Remove <CarId>        – flag car as removed
  Rent <CarId> <Name>   – rent a car to customer
  Return <CarId>        – return a rented car
  Search id <CarId>     – find car by ID
  Search model <text>   – find cars by model substring
  Save                  – save current data to CSV
  Exit                  – save and quit
");
        }

        static void ListCars(IEnumerable<Car> list, string header)
        {
            Console.WriteLine("\n" + header);
            foreach (var c in list)
                Console.WriteLine(c.GetDetails());
        }

        static void AddCarInteractive(CarRentalService svc)
        {
            Console.Write("New Id: ");
            var id = int.Parse(Console.ReadLine()!);
            Console.Write("Make: ");
            var make = Console.ReadLine()!;
            Console.Write("Model: ");
            var model = Console.ReadLine()!;
            Console.Write("Year: ");
            var year = int.Parse(Console.ReadLine()!);
            Console.Write("Type (e.g. Sedan): ");
            var type = Console.ReadLine()!;

            var car = new Car
            {
                Id = id,
                Make = make,
                Model = model,
                Year = year,
                Type = type,
                Status = "Available",
                CurrentRenter = ""
            };
            svc.AddCar(car);
            Console.WriteLine($"Added car {id} – {make} {model} ({year}).");
        }

        static void EditCarInteractive(CarRentalService svc, string arg)
        {
            if (!int.TryParse(arg, out int id))
            {
                Console.WriteLine("Usage: Edit <CarId>");
                return;
            }

            var car = svc.GetCarById(id);
            if (car == null)
            {
                Console.WriteLine($"No car with Id {id}");
                return;
            }

            Console.WriteLine($"Editing Car {id}. Press Enter to keep current value.");
            Console.Write($"Make ({car.Make}): ");
            var tmp = Console.ReadLine();
            if (!string.IsNullOrEmpty(tmp)) car.Make = tmp;

            Console.Write($"Model ({car.Model}): ");
            tmp = Console.ReadLine();
            if (!string.IsNullOrEmpty(tmp)) car.Model = tmp;

            Console.Write($"Year ({car.Year}): ");
            tmp = Console.ReadLine();
            if (!string.IsNullOrEmpty(tmp) && int.TryParse(tmp, out int y)) car.Year = y;

            Console.Write($"Type ({car.Type}): ");
            tmp = Console.ReadLine();
            if (!string.IsNullOrEmpty(tmp)) car.Type = tmp;

            Console.Write($"Status ({car.Status}): ");
            tmp = Console.ReadLine();
            if (!string.IsNullOrEmpty(tmp)) car.Status = tmp;

            Console.WriteLine("Car updated.");
        }
    }
}
