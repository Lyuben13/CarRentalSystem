using System;
using System.Collections.Generic;
using System.Linq;
using CarRentalSystem.Interfaces;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Main service class containing business logic for car rental management
    /// </summary>
    public class CarRentalService : ISearchable
    {
        private readonly List<Car> cars;
        private readonly List<Rental> rentals;
        private readonly List<Customer> customers;

        /// <summary>
        /// Creates a service instance with empty lists
        /// </summary>
        public CarRentalService()
            : this(new List<Car>(), new List<Rental>(), new List<Customer>())
        {
        }

        /// <summary>
        /// Creates a service instance with initial cars
        /// </summary>
        /// <param name="initialCars">Initial list of cars</param>
        public CarRentalService(List<Car> initialCars)
            : this(initialCars, new List<Rental>(), new List<Customer>())
        {
        }

        /// <summary>
        /// Creates a service instance with provided cars and rentals
        /// </summary>
        /// <param name="initialCars">Initial list of cars</param>
        /// <param name="initialRentals">Initial list of rentals</param>
        public CarRentalService(List<Car> initialCars, List<Rental> initialRentals)
            : this(initialCars, initialRentals, new List<Customer>())
        {
        }

        /// <summary>
        /// Creates a service instance with provided cars, rentals, and customers
        /// </summary>
        /// <param name="initialCars">Initial list of cars</param>
        /// <param name="initialRentals">Initial list of rentals</param>
        /// <param name="initialCustomers">Initial list of customers</param>
        public CarRentalService(List<Car> initialCars, List<Rental> initialRentals, List<Customer> initialCustomers)
        {
            cars = initialCars ?? new List<Car>();
            rentals = initialRentals ?? new List<Rental>();
            customers = initialCustomers ?? new List<Customer>();
        }

        // === ISearchable Implementation ===
        public Car GetCarById(int id) => cars.FirstOrDefault(c => c.Id == id);

        public List<Car> SearchByModel(string model) => cars
            .Where(c => c.Model.Contains(model, StringComparison.OrdinalIgnoreCase))
            .ToList();

        public List<Car> SearchByStatus(string status) => cars
            .Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
            .ToList();

        // === Car Operations ===
        public List<Car> GetAllCars() => cars;

        public void AddCar(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            if (!car.IsValid())
                throw new ArgumentException("Car data is invalid");

            if (cars.Any(c => c.Id == car.Id))
                throw new InvalidOperationException($"Car with ID {car.Id} already exists");

            cars.Add(car);
        }

        public void RemoveCar(int id)
        {
            var car = GetCarById(id);
            if (car == null)
                throw new ArgumentException($"Car with ID {id} not found");

            car.Status = "Removed";
        }

        public void RentCar(int id, string renter)
        {
            var car = cars.FirstOrDefault(c => c.Id == id && c.Status == "Available");
            if (car == null)
                throw new InvalidOperationException($"Car {id} is not available for rent");

            car.Rent(renter);
            
            // Create a rental record
            var rental = new Rental
            {
                Id = rentals.Count > 0 ? rentals.Max(r => r.Id) + 1 : 1001,
                CarId = id,
                CustomerName = renter,
                StartDate = DateTime.Now,
                ExpectedReturn = DateTime.Now.AddDays(7), // Default 7 days
                DailyRate = car.DailyRate,
                Status = "Active"
            };
            
            rentals.Add(rental);
        }

        public void ReturnCar(int id)
        {
            var car = cars.FirstOrDefault(c => c.Id == id && c.Status == "Rented");
            if (car == null)
                throw new InvalidOperationException($"Car {id} is not currently rented");

            car.Return();
            
            // Complete the rental record using expected return date
            var rental = rentals.FirstOrDefault(r => r.CarId == id && r.Status == "Active");
            if (rental != null)
            {
                rental.Complete(rental.ExpectedReturn);
            }
        }

        public List<Car> ListAvailable() => cars.Where(c => c.Status == "Available").ToList();

        public List<Car> ListRented() => cars.Where(c => c.Status == "Rented").ToList();

        public List<Car> ListNeedingMaintenance() => cars.Where(c => c.NeedsMaintenance()).ToList();

        // === Rental Operations ===
        public List<Rental> GetAllRentals() => rentals;

        public void AddRental(Rental rental)
        {
            if (rental == null)
                throw new ArgumentNullException(nameof(rental));

            if (rentals.Any(r => r.Id == rental.Id))
                throw new InvalidOperationException($"Rental with ID {rental.Id} already exists");

            var car = GetCarById(rental.CarId);
            if (car == null)
                throw new ArgumentException($"Car with ID {rental.CarId} not found");

            if (car.Status != "Available")
                throw new InvalidOperationException($"Car {rental.CarId} is not available");

            rentals.Add(rental);
            car.Rent(rental.CustomerName);
        }

        public void CompleteRental(int carId, DateTime actualReturn)
        {
            var rental = rentals.FirstOrDefault(r => r.CarId == carId && r.Status == "Active");
            if (rental == null)
                throw new ArgumentException($"No active rental found for car {carId}");

            // Use the provided actual return date, or expected return date if not provided
            var returnDate = actualReturn != DateTime.MinValue ? actualReturn : rental.ExpectedReturn;
            rental.Complete(returnDate);
            GetCarById(carId)?.Return();
        }

        public List<Rental> GetOverdueRentals() => rentals.Where(r => r.IsOverdue()).ToList();

        public decimal GetTotalRevenue()
        {
            return rentals.Where(r => r.Status == "Completed" && r.TotalCost.HasValue)
                         .Sum(r => r.TotalCost.Value);
        }

        // === Customer Operations ===
        public List<Customer> GetAllCustomers() => customers;

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!customer.IsValid())
                throw new ArgumentException("Customer data is invalid");

            if (customers.Any(c => c.Id == customer.Id))
                throw new InvalidOperationException($"Customer with ID {customer.Id} already exists");

            customers.Add(customer);
        }

        public Customer GetCustomerById(int id) => customers.FirstOrDefault(c => c.Id == id);

        // === Business Intelligence ===
        public Dictionary<string, int> GetCarsByType()
        {
            return cars.GroupBy(c => c.Type)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, decimal> GetRevenueByMonth()
        {
            return rentals.Where(r => r.Status == "Completed" && r.TotalCost.HasValue)
                         .GroupBy(r => r.ActualReturn?.ToString("yyyy-MM"))
                         .ToDictionary(g => g.Key ?? "Unknown", g => g.Sum(r => r.TotalCost.Value));
        }
    }
}
