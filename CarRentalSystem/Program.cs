using System;
using System.Collections.Generic;
using CarRentalSystem.Models;
using CarRentalSystem.Services;
using System.Linq; // Added for .Any()

namespace CarRentalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string carsPath = "cars.csv";
            const string rentalsPath = "rentals.csv";
            const string customersPath = "customers.csv";

            try
            {
                var carReader = new CarFileReader(carsPath);
                var carWriter = new CarFileWriter(carsPath);
                var rentalReader = new RentalFileReader(rentalsPath);
                var rentalWriter = new RentalFileWriter(rentalsPath);

                var cars = carReader.LoadCars();
                var rentals = rentalReader.LoadRentals();
                var service = new CarRentalService(cars, rentals);

                Console.WriteLine("=== Welcome to Car Rental System ===");
                Console.WriteLine($"Loaded {cars.Count} cars and {rentals.Count} rentals.");
                ShowHelp();

                bool isRunning = true;
                while (isRunning)
                {
                    try
                    {
                        Console.Write("\nCommand> ");
                        var input = Console.ReadLine()?.Trim();
                        if (string.IsNullOrEmpty(input)) continue;

                        var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                        var cmd = parts[0].ToLower();
                        var arg = parts.Length > 1 ? parts[1] : string.Empty;

                        isRunning = ExecuteCommand(service, carWriter, rentalWriter, cmd, arg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        if (ex.InnerException != null)
                            Console.WriteLine($"Details: {ex.InnerException.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        static bool ExecuteCommand(CarRentalService service, CarFileWriter carWriter, RentalFileWriter rentalWriter, string cmd, string arg)
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

                case "listrented":
                    ListCars(service.ListRented(), "Currently rented cars:");
                    break;

                case "listrentals":
                    ListRentals(service.GetAllRentals());
                    break;

                case "listoverdue":
                    ListRentals(service.GetOverdueRentals());
                    break;

                case "listmaintenance":
                    ListCars(service.ListNeedingMaintenance(), "Cars needing maintenance:");
                    break;

                case "add":
                    AddCarInteractive(service);
                    break;

                case "addcustomer":
                    AddCustomerInteractive(service);
                    break;

                case "addrental":
                    AddRentalInteractive(service, arg);
                    break;

                case "edit":
                    EditCarInteractive(service, arg);
                    break;

                case "remove":
                    if (int.TryParse(arg, out int remId))
                    {
                        service.RemoveCar(remId);
                        Console.WriteLine($"Car {remId} marked as removed.");
                    }
                    else
                        Console.WriteLine("Usage: Remove <CarId>");
                    break;

                case "rent":
                    RentInteractive(service, arg);
                    break;

                case "return":
                    ReturnInteractive(service, arg);
                    break;

                case "completerental":
                    if (int.TryParse(arg, out int compId))
                    {
                        service.CompleteRental(compId, DateTime.MinValue); // Use expected return date
                        Console.WriteLine($"Completed rental for Car {compId}.");
                    }
                    else Console.WriteLine("Usage: CompleteRental <CarId>");
                    break;

                case "search":
                    SearchInteractive(service, arg);
                    break;

                case "searchstatus":
                case "search status":
                    var res = service.SearchByStatus(arg);
                    ListCars(res, $"Cars with status '{arg}':");
                    break;

                case "stats":
                    ShowStatistics(service);
                    break;

                case "revenue":
                    ShowRevenue(service);
                    break;

                case "save":
                    carWriter.SaveCars(service.GetAllCars());
                    rentalWriter.SaveRentals(service.GetAllRentals());
                    Console.WriteLine("Data saved to CSV files.");
                    break;

                case "exit":
                case "quit":
                    carWriter.SaveCars(service.GetAllCars());
                    rentalWriter.SaveRentals(service.GetAllRentals());
                    Console.WriteLine("Data saved and exiting. Goodbye!");
                    return false;

                default:
                    Console.WriteLine($"Unknown command: '{cmd}'. Type 'help' to see available commands.");
                    break;
            }
            return true;
        }

        static void ShowHelp()
        {
            Console.WriteLine(@"
Available commands:
  Help                  – show this help
  ListAll               – list all cars
  ListAvailable         – list only available cars
  ListRented            – list currently rented cars
  ListRentals           – list all rentals
  ListOverdue           – list overdue rentals
  ListMaintenance       – list cars needing maintenance
  Add                   – add a new car
  AddCustomer           – add a new customer
  Edit <CarId>          – edit existing car
  Remove <CarId>        – flag car as removed
  Rent <CarId> <Name>   – rent a car to customer
  Return <CarId>        – return a rented car
  AddRental <...>       – add a new rental
  CompleteRental <CarId>– complete a rental
  Search id <CarId>     – find car by ID
  Search model <text>   – find cars by model substring
  Search status <stat>  – find cars by status
  Stats                 – show system statistics
  Revenue               – show revenue information
  Save                  – save current data to CSV
  Exit/Quit             – save and quit
");
        }

        static void ListCars(IEnumerable<Car> list, string header)
        {
            Console.WriteLine($"\n{header}");
            if (!list.Any())
            {
                Console.WriteLine("No cars found.");
                return;
            }
            
            foreach (var c in list)
                Console.WriteLine(c.GetDisplayInfo());
        }

        static void ListRentals(IEnumerable<Rental> list)
        {
            Console.WriteLine("\nAll rentals:");
            if (!list.Any())
            {
                Console.WriteLine("No rentals found.");
                return;
            }
            
            foreach (var r in list)
                Console.WriteLine(r.GetDetails());
        }

        static void AddCarInteractive(CarRentalService svc)
        {
            try
            {
                Console.Write("New Id: "); 
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID. Must be a number.");
                    return;
                }

                Console.Write("Make: "); 
                var make = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(make))
                {
                    Console.WriteLine("Make cannot be empty.");
                    return;
                }

                Console.Write("Model: "); 
                var model = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(model))
                {
                    Console.WriteLine("Model cannot be empty.");
                    return;
                }

                Console.Write("Year: "); 
                if (!int.TryParse(Console.ReadLine(), out int year))
                {
                    Console.WriteLine("Invalid year. Must be a number.");
                    return;
                }

                Console.Write("Type: "); 
                var type = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(type))
                {
                    Console.WriteLine("Type cannot be empty.");
                    return;
                }

                Console.Write("License Plate: "); 
                var licensePlate = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Daily Rate: "); 
                if (!decimal.TryParse(Console.ReadLine(), out decimal rate))
                {
                    rate = 35.0m;
                }

                var car = new Car 
                { 
                    Id = id, 
                    Make = make, 
                    Model = model, 
                    Year = year, 
                    Type = type, 
                    LicensePlate = licensePlate,
                    DailyRate = rate,
                    Status = "Available" 
                };

                svc.AddCar(car);
                Console.WriteLine($"Added car {id} – {make} {model} ({year}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding car: {ex.Message}");
            }
        }

        static void AddCustomerInteractive(CarRentalService svc)
        {
            try
            {
                Console.Write("Customer ID: "); 
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID. Must be a number.");
                    return;
                }

                Console.Write("Name: "); 
                var name = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Name cannot be empty.");
                    return;
                }

                Console.Write("License Number: "); 
                var license = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(license))
                {
                    Console.WriteLine("License number cannot be empty.");
                    return;
                }

                Console.Write("Email: "); 
                var email = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Phone: "); 
                var phone = Console.ReadLine()?.Trim() ?? "";

                var customer = new Customer 
                { 
                    Id = id, 
                    Name = name, 
                    LicenseNumber = license,
                    Email = email,
                    Phone = phone
                };

                svc.AddCustomer(customer);
                Console.WriteLine($"Added customer {id} – {name}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
            }
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
            var tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp)) car.Make = tmp;

            Console.Write($"Model ({car.Model}): ");
            tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp)) car.Model = tmp;

            Console.Write($"Year ({car.Year}): ");
            tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp) && int.TryParse(tmp, out int y)) car.Year = y;

            Console.Write($"Type ({car.Type}): ");
            tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp)) car.Type = tmp;

            Console.Write($"License Plate ({car.LicensePlate}): ");
            tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp)) car.LicensePlate = tmp;

            Console.Write($"Daily Rate ({car.DailyRate:F2}): ");
            tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp) && decimal.TryParse(tmp, out decimal rate)) car.DailyRate = rate;

            Console.Write($"Status ({car.Status}): ");
            tmp = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tmp)) car.Status = tmp;

            Console.WriteLine("Car updated successfully.");
        }

        static void AddRentalInteractive(CarRentalService svc, string arg)
        {
            var parts = arg.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 && int.TryParse(parts[0], out int carId) && DateTime.TryParse(parts[2], out DateTime exp))
            {
                var rental = new Rental 
                { 
                    Id = new Random().Next(1000, 9999), // Simple ID generation
                    CarId = carId, 
                    CustomerName = parts[1], 
                    StartDate = DateTime.Now, 
                    ExpectedReturn = exp 
                };
                
                svc.AddRental(rental);
                Console.WriteLine($"Added rental: Car {carId} to {parts[1]} until {exp:yyyy-MM-dd}.");
            }
            else Console.WriteLine("Usage: AddRental <CarId> <CustomerName> <ExpectedReturn yyyy-MM-dd>");
        }

        static void RentInteractive(CarRentalService svc, string arg)
        {
            var a = arg.Split(' ', 2);
            if (a.Length == 2 && int.TryParse(a[0], out int rentId))
            {
                svc.RentCar(rentId, a[1]);
                Console.WriteLine($"Car {rentId} rented to {a[1]}.");
            }
            else Console.WriteLine("Usage: Rent <CarId> <RenterName>");
        }

        static void ReturnInteractive(CarRentalService svc, string arg)
        {
            if (int.TryParse(arg, out int retId))
            {
                svc.ReturnCar(retId);
                Console.WriteLine($"Car {retId} returned successfully.");
            }
            else Console.WriteLine("Usage: Return <CarId>");
        }

        static void SearchInteractive(CarRentalService svc, string arg)
        {
            var sa = arg.Split(' ', 2);
            if (sa.Length == 2)
            {
                if (sa[0].Equals("id", StringComparison.OrdinalIgnoreCase) && int.TryParse(sa[1], out int sid))
                {
                    var c = svc.GetCarById(sid);
                    Console.WriteLine(c != null ? c.GetDisplayInfo() : $"No car with Id {sid}");
                }
                else if (sa[0].Equals("model", StringComparison.OrdinalIgnoreCase))
                {
                    var found = svc.SearchByModel(sa[1]);
                    ListCars(found, $"Cars matching model '{sa[1]}':");
                }
                else Console.WriteLine("Usage: Search id <CarId> OR Search model <text>");
            }
            else Console.WriteLine("Usage: Search id <CarId> OR Search model <text>");
        }

        static void ShowStatistics(CarRentalService svc)
        {
            var allCars = svc.GetAllCars();
            var availableCars = svc.ListAvailable();
            var rentedCars = svc.ListRented();
            var overdueRentals = svc.GetOverdueRentals();
            var carsByType = svc.GetCarsByType();

            Console.WriteLine("\n=== System Statistics ===");
            Console.WriteLine($"Total Cars: {allCars.Count()}");
            Console.WriteLine($"Available Cars: {availableCars.Count()}");
            Console.WriteLine($"Rented Cars: {rentedCars.Count()}");
            Console.WriteLine($"Overdue Rentals: {overdueRentals.Count()}");
            
            Console.WriteLine("\nCars by Type:");
            foreach (var type in carsByType)
            {
                Console.WriteLine($"  {type.Key}: {type.Value}");
            }
        }

        static void ShowRevenue(CarRentalService svc)
        {
            var totalRevenue = svc.GetTotalRevenue();
            var revenueByMonth = svc.GetRevenueByMonth();

            Console.WriteLine("\n=== Revenue Information ===");
            Console.WriteLine($"Total Revenue: ${totalRevenue:F2}");
            
            Console.WriteLine("\nRevenue by Month:");
            foreach (var month in revenueByMonth)
            {
                Console.WriteLine($"  {month.Key}: ${month.Value:F2}");
            }
        }
    }
}
