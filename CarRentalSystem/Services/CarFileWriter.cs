using System.Collections.Generic;
using System.IO;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    public class CarFileWriter
    {
        private string filePath;

        public CarFileWriter(string path)
        {
            filePath = path;
        }

        public void SaveCars(List<Car> cars)
        {
            var lines = new List<string>
            {
                "Id,Make,Model,Year,Type,Status,CurrentRenter"
            };

            foreach (var car in cars)
            {
                lines.Add(car.GetDetails());
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
