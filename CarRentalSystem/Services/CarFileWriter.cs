using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Writes car list to cars.csv file
    /// </summary>
    public class CarFileWriter
    {
        private readonly string filePath;

        /// <summary>
        /// Initializes a new instance of the CarFileWriter with the specified file path
        /// </summary>
        /// <param name="path">The path to the CSV file where car data will be saved</param>
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
            // 1) Запиши header със същия ред на колоните
            var lines = new List<string>
            {
                "Id,Make,Model,Year,Type,Status,CurrentRenter,LicensePlate,Mileage,DailyRate"
            };

            // 2) За всяка кола формирай CSV-ред
            foreach (var c in cars)
            {
                var row = string.Join(',',
                    c.Id,
                    c.Make,
                    c.Model,
                    c.Year,
                    c.Type,
                    c.Status,
                    c.CurrentRenter ?? "",
                    c.LicensePlate ?? "",
                    c.Mileage,
                    c.DailyRate.ToString(CultureInfo.InvariantCulture)
                );
                lines.Add(row);
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
