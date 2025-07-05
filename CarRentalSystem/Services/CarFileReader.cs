using System;
using System.Collections.Generic;
using System.IO;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    public class CarFileReader
    {
        private string filePath;

        public CarFileReader(string path)
        {
            filePath = path;
        }

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
                        CurrentRenter = parts.Length > 6 ? parts[6] : ""
                    };
                    cars.Add(car);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading line: " + lines[i]);
                    Console.WriteLine(ex.Message);
                }
            }

            return cars;
        }
    }
}
