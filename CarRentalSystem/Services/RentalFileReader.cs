using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Reads rental records from rentals.csv file
    /// </summary>
    public class RentalFileReader
    {
        private readonly string filePath;
        
        /// <summary>
        /// Initializes a new instance of the RentalFileReader with the specified file path
        /// </summary>
        /// <param name="path">The path to the CSV file containing rental data</param>
        public RentalFileReader(string path) => filePath = path;

        /// <summary>
        /// Loads all rentals from CSV file
        /// </summary>
        /// <returns>List of rental records</returns>
        public List<Rental> LoadRentals()
        {
            var list = new List<Rental>();
            if (!File.Exists(filePath)) 
            {
                return list;
            }

            var lines = File.ReadAllLines(filePath);
            
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                try
                {
                    var rental = new Rental
                    {
                        Id = int.Parse(parts[0]),
                        CarId = int.Parse(parts[1]),
                        CustomerId = null, // Not in current CSV format
                        CustomerName = parts[2],
                        StartDate = DateTime.Parse(parts[3]),
                        ExpectedReturn = DateTime.Parse(parts[4]),
                        ActualReturn = parts.Length > 5 && !string.IsNullOrEmpty(parts[5]) ? DateTime.Parse(parts[5]) : null,
                        DailyRate = parts.Length > 6 ? decimal.Parse(parts[6], CultureInfo.InvariantCulture) : 35.0m,
                        TotalCost = parts.Length > 7 && !string.IsNullOrEmpty(parts[7]) ? decimal.Parse(parts[7], CultureInfo.InvariantCulture) : null,
                        Status = ParseRentalStatus(parts.Length > 8 ? parts[8] : "Active")
                    };
                    list.Add(rental);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading rental line {i + 1}: {ex.Message}");
                }
            }
            return list;
        }

        /// <summary>
        /// Parses rental status from string to enum
        /// </summary>
        /// <param name="statusString">Status as string</param>
        /// <returns>RentalStatus enum value</returns>
        private RentalStatus ParseRentalStatus(string statusString)
        {
            if (Enum.TryParse<RentalStatus>(statusString, true, out var status))
                return status;
            
            // Fallback for old string values
            return statusString?.ToLower() switch
            {
                "active" => RentalStatus.Active,
                "completed" => RentalStatus.Completed,
                "overdue" => RentalStatus.Overdue,
                _ => RentalStatus.Active
            };
        }
    }
}
