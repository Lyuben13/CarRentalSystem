using System.Collections.Generic;
using System.Linq;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    public class CarRentalService
    {
        private List<Car> cars;

        public CarRentalService(List<Car> initialCars)
        {
            cars = initialCars;
        }

        public List<Car> GetAllCars() => cars;

        public void AddCar(Car car)
        {
            cars.Add(car);
        }

        public void RemoveCar(int id)
        {
            var car = cars.FirstOrDefault(c => c.Id == id);
            if (car != null)
                car.Status = "Removed";
        }

        public void RentCar(int id, string renter)
        {
            var car = cars.FirstOrDefault(c => c.Id == id && c.Status == "Available");
            car?.Rent(renter);
        }

        public void ReturnCar(int id)
        {
            var car = cars.FirstOrDefault(c => c.Id == id && c.Status == "Rented");
            car?.Return();
        }

        public List<Car> SearchByModel(string model)
        {
            return cars.Where(c => c.Model.ToLower().Contains(model.ToLower())).ToList();
        }

        public Car GetCarById(int id)
        {
            return cars.FirstOrDefault(c => c.Id == id);
        }

        public List<Car> ListAvailable()
        {
            return cars.Where(c => c.Status == "Available").ToList();
        }
    }
}
