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
        /// <summary>
        /// Gets a car by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the car</param>
        /// <returns>The car with the specified ID, or null if not found</returns>
        public Car? GetCarById(int id) => cars.FirstOrDefault(c => c.Id == id);

        /// <summary>
        /// Searches for cars by model name (case-insensitive)
        /// </summary>
        /// <param name="model">The model name to search for</param>
        /// <returns>List of cars matching the model criteria</returns>
        public List<Car> SearchByModel(string model) => cars
            .Where(c => c.Model.Contains(model, StringComparison.OrdinalIgnoreCase))
            .ToList();

        /// <summary>
        /// Searches for cars by status (case-insensitive)
        /// </summary>
        /// <param name="status">The status to search for</param>
        /// <returns>List of cars with the specified status</returns>
        public List<Car> SearchByStatus(string status) => cars
            .Where(c => c.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase))
            .ToList();

        // === Car Operations ===
        /// <summary>
        /// Gets all cars in the system
        /// </summary>
        /// <returns>List of all cars</returns>
        public List<Car> GetAllCars() => cars;

        /// <summary>
        /// Adds a new car to the system
        /// </summary>
        /// <param name="car">The car to add</param>
        /// <exception cref="ArgumentNullException">Thrown when car is null</exception>
        /// <exception cref="ArgumentException">Thrown when car data is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when car with same ID already exists</exception>
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

        /// <summary>
        /// Marks a car as removed from the fleet
        /// </summary>
        /// <param name="id">The ID of the car to remove</param>
        /// <exception cref="ArgumentException">Thrown when car with specified ID is not found</exception>
        public void RemoveCar(int id)
        {
            var car = GetCarById(id);
            if (car == null)
                throw new ArgumentException($"Car with ID {id} not found");

            car.Status = CarStatus.Removed;
        }

        /// <summary>
        /// Rents a car to a customer
        /// </summary>
        /// <param name="id">The ID of the car to rent</param>
        /// <param name="renter">The name of the person renting the car</param>
        /// <exception cref="InvalidOperationException">Thrown when car is not available for rent</exception>
        public void RentCar(int id, string renter)
        {
            var car = cars.FirstOrDefault(c => c.Id == id && c.Status == CarStatus.Available);
            if (car == null)
                throw new InvalidOperationException($"Car {id} is not available for rent");

            // Create a rental record first
            var rental = new Rental
            {
                Id = rentals.Count > 0 ? rentals.Max(r => r.Id) + 1 : 1001,
                CarId = id,
                CustomerId = null, // Not used in current implementation
                CustomerName = renter,
                StartDate = DateTime.Now,
                ExpectedReturn = DateTime.Now.AddDays(7), // Default 7 days
                DailyRate = car.DailyRate,
                Status = RentalStatus.Active
            };
            
            rentals.Add(rental);
            
            // Then rent the car (only once)
            car.Rent(renter);
        }

        /// <summary>
        /// Returns a rented car and completes the rental transaction
        /// </summary>
        /// <param name="id">The ID of the car to return</param>
        /// <exception cref="InvalidOperationException">Thrown when car is not currently rented or no active rental exists</exception>
        public void ReturnCar(int id)
        {
            var car = GetCarById(id);
            if (car == null || car.Status != CarStatus.Rented)
                throw new InvalidOperationException($"Car {id} is not currently rented");

            car.Return();
            
            // Complete the rental record using current date for accurate revenue calculation
            var rental = rentals.FirstOrDefault(r => r.CarId == id && r.Status == RentalStatus.Active);
            if (rental == null)
                throw new InvalidOperationException($"No active rental found for car {id}");
            
            rental.Complete(DateTime.Now);
        }

        /// <summary>
        /// Gets all available cars
        /// </summary>
        /// <returns>List of available cars</returns>
        public List<Car> ListAvailable() => cars.Where(c => c.Status == CarStatus.Available).ToList();

        /// <summary>
        /// Gets all currently rented cars
        /// </summary>
        /// <returns>List of rented cars</returns>
        public List<Car> ListRented() => cars.Where(c => c.Status == CarStatus.Rented).ToList();

        /// <summary>
        /// Gets all cars that need maintenance
        /// </summary>
        /// <returns>List of cars needing maintenance</returns>
        public List<Car> ListNeedingMaintenance() => cars.Where(c => c.NeedsMaintenance()).ToList();

        // === Rental Operations ===
        /// <summary>
        /// Gets all rental transactions
        /// </summary>
        /// <returns>List of all rentals</returns>
        public List<Rental> GetAllRentals() => rentals;

        /// <summary>
        /// Adds a new rental transaction
        /// </summary>
        /// <param name="rental">The rental to add</param>
        /// <exception cref="ArgumentNullException">Thrown when rental is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when rental with same ID already exists or car is not available</exception>
        /// <exception cref="ArgumentException">Thrown when car with specified ID is not found</exception>
        public void AddRental(Rental rental)
        {
            if (rental == null)
                throw new ArgumentNullException(nameof(rental));

            if (rentals.Any(r => r.Id == rental.Id))
                throw new InvalidOperationException($"Rental with ID {rental.Id} already exists");

            var car = GetCarById(rental.CarId);
            if (car == null)
                throw new ArgumentException($"Car with ID {rental.CarId} not found");

            if (car.Status != CarStatus.Available)
                throw new InvalidOperationException($"Car {rental.CarId} is not available");

            rentals.Add(rental);
            car.Rent(rental.CustomerName);
        }

        /// <summary>
        /// Completes a rental transaction
        /// </summary>
        /// <param name="carId">The ID of the car</param>
        /// <param name="actualReturn">The actual return date</param>
        /// <exception cref="ArgumentException">Thrown when no active rental is found for the car</exception>
        public void CompleteRental(int carId, DateTime actualReturn)
        {
            var rental = rentals.FirstOrDefault(r => r.CarId == carId && r.Status == RentalStatus.Active);
            if (rental == null)
                throw new ArgumentException($"No active rental found for car {carId}");

            // Use the provided actual return date, or expected return date if not provided
            var returnDate = actualReturn != DateTime.MinValue ? actualReturn : rental.ExpectedReturn;
            rental.Complete(returnDate);
            
            var car = GetCarById(carId);
            if (car != null)
                car.Return();
        }

        /// <summary>
        /// Gets all overdue rental transactions
        /// </summary>
        /// <returns>List of overdue rentals</returns>
        public List<Rental> GetOverdueRentals() => rentals.Where(r => r.IsOverdue()).ToList();

        /// <summary>
        /// Calculates the total revenue from completed rentals
        /// </summary>
        /// <returns>Total revenue amount</returns>
        public decimal GetTotalRevenue()
        {
            return rentals.Where(r => r.Status == RentalStatus.Completed && r.TotalCost.HasValue)
                         .Sum(r => r.TotalCost!.Value);
        }

        // === Customer Operations ===
        /// <summary>
        /// Gets all customers in the system
        /// </summary>
        /// <returns>List of all customers</returns>
        public List<Customer> GetAllCustomers() => customers;

        /// <summary>
        /// Adds a new customer to the system
        /// </summary>
        /// <param name="customer">The customer to add</param>
        /// <exception cref="ArgumentNullException">Thrown when customer is null</exception>
        /// <exception cref="ArgumentException">Thrown when customer data is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when customer with same ID already exists</exception>
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

        /// <summary>
        /// Gets a customer by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <returns>The customer with the specified ID, or null if not found</returns>
        public Customer? GetCustomerById(int id) => customers.FirstOrDefault(c => c.Id == id);

        // === Statistics ===
        /// <summary>
        /// Gets statistics of cars grouped by type
        /// </summary>
        /// <returns>Dictionary with car type as key and count as value</returns>
        public Dictionary<string, int> GetCarsByType()
        {
            return cars.GroupBy(c => c.Type)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Gets revenue statistics grouped by month
        /// </summary>
        /// <returns>Dictionary with month (yyyy-MM) as key and revenue as value</returns>
        public Dictionary<string, decimal> GetRevenueByMonth()
        {
            return rentals.Where(r => r.Status == RentalStatus.Completed && r.TotalCost.HasValue)
                         .GroupBy(r => r.StartDate.ToString("yyyy-MM"))
                         .ToDictionary(g => g.Key, g => g.Sum(r => r.TotalCost!.Value));
        }
    }
}
