using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Reads car records from cars.csv file
    /// </summary>
    public class CarFileReader
    {
        private readonly string filePath;

        /// <summary>
        /// Initializes a new instance of the CarFileReader with the specified file path
        /// </summary>
        /// <param name="path">The path to the CSV file containing car data</param>
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
            if (lines.Length < 2)
                return cars;

            // Чети header и намери индексите
            var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
            int idxId           = Array.IndexOf(headers, "Id");
            int idxMake         = Array.IndexOf(headers, "Make");
            int idxModel        = Array.IndexOf(headers, "Model");
            int idxYear         = Array.IndexOf(headers, "Year");
            int idxType         = Array.IndexOf(headers, "Type");
            int idxStatus       = Array.IndexOf(headers, "Status");
            int idxCurrentRenter= Array.IndexOf(headers, "CurrentRenter");
            int idxLicensePlate = Array.IndexOf(headers, "LicensePlate");
            int idxMileage      = Array.IndexOf(headers, "Mileage");
            int idxDailyRate    = Array.IndexOf(headers, "DailyRate");
            int colCount        = headers.Length;

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',').Select(p => p.Trim()).ToArray();
                if (parts.Length < colCount)
                {
                    Array.Resize(ref parts, colCount);
                    for (int k = 0; k < parts.Length; k++)
                        parts[k] ??= "";
                }
                else if (parts.Length > colCount)
                {
                    parts = parts.Take(colCount).ToArray();
                }

                try
                {
                    var car = new Car
                    {
                        Id            = idxId           >= 0 && idxId < parts.Length && int.TryParse(parts[idxId], out var id) ? id : 0,
                        Make          = idxMake         >= 0 && idxMake < parts.Length ? parts[idxMake] : "",
                        Model         = idxModel        >= 0 && idxModel < parts.Length ? parts[idxModel] : "",
                        Year          = idxYear         >= 0 && idxYear < parts.Length && int.TryParse(parts[idxYear], out var year) ? year : 0,
                        Type          = idxType         >= 0 && idxType < parts.Length ? parts[idxType] : "",
                        Status        = idxStatus       >= 0 && idxStatus < parts.Length ? ParseCarStatus(parts[idxStatus]) : CarStatus.Available,
                        CurrentRenter = idxCurrentRenter>= 0 && idxCurrentRenter < parts.Length ? parts[idxCurrentRenter] : "",
                        LicensePlate  = idxLicensePlate >= 0 && idxLicensePlate < parts.Length ? parts[idxLicensePlate] : "",
                        Mileage       = idxMileage      >= 0 && idxMileage < parts.Length && int.TryParse(parts[idxMileage], out var m) ? m : 0,
                        DailyRate     = 35.0m // ще се парсне по-долу
                    };

                    // DailyRate
                    decimal rate = 35.0m;
                    if (idxDailyRate >= 0 && idxDailyRate < parts.Length &&
                        decimal.TryParse(parts[idxDailyRate], NumberStyles.Any, CultureInfo.InvariantCulture, out var dr))
                    {
                        rate = dr;
                    }
                    car.DailyRate = rate;



                    cars.Add(car);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Грешка при четене на ред {i+1}: {ex.Message}");
                }
            }

            return cars;
        }

        private CarStatus ParseCarStatus(string statusString)
        {
            if (Enum.TryParse<CarStatus>(statusString, true, out var status))
                return status;
            return statusString?.ToLower() switch
            {
                "available" => CarStatus.Available,
                "rented" => CarStatus.Rented,
                "removed" => CarStatus.Removed,
                "maintenance" => CarStatus.Maintenance,
                _ => CarStatus.Available
            };
        }
    }
}
