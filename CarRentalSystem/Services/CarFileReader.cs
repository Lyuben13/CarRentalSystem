using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Reads car records from cars.csv file
    /// </summary>
    public class CarFileReader
    {
        private string filePath;

        public CarFileReader(string path)
        {
            filePath = path;
        }

        /// <summary>
        /// Loads all cars from CSV file
        /// </summary>
        /// <returns>List of car records</returns>
        public List<Car> LoadCars()
        {
            var cars = new List<Car>();

            if (!File.Exists(filePath))
                return cars;

            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) 
            {
                var parts = lines[i].Split(',');
                try
                {
                    var car = new Car
                    {
                        Id = int.Parse(parts[0]),
                        Make = parts[1],
                        Model = parts[2],
                        Year = int.Parse(parts[3]),
                        Type = parts[4],
                        Status = parts[5],
                        CurrentRenter = parts.Length > 6 ? parts[6] : "",
                        LicensePlate = parts.Length > 7 ? parts[7] : "",
                        Mileage = parts.Length > 8 ? int.Parse(parts[8]) : 0,
                        DailyRate = parts.Length > 9 ? decimal.Parse(parts[9], CultureInfo.InvariantCulture) : 35.0m
                    };
                    cars.Add(car);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading car line {i + 1}: {ex.Message}");
                }
            }

            return cars;
        }
    }
}
