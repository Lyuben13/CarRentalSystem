using System.Collections.Generic;
using System.IO;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Writes car list to cars.csv file
    /// </summary>
    public class CarFileWriter
    {
        private string filePath;

        public CarFileWriter(string path)
        {
            filePath = path;
        }

        /// <summary>
        /// Saves car list to CSV file
        /// </summary>
        /// <param name="cars">List of cars to save</param>
        public void SaveCars(List<Car> cars)
        {
            var lines = new List<string>
            {
                "Id,Make,Model,Year,Type,Status,CurrentRenter,LicensePlate,Mileage,DailyRate"
            };

            foreach (var car in cars)
            {
                lines.Add(car.GetDetails());
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
