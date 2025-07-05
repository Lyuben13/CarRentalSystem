using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Writes rental list to rentals.csv file
    /// </summary>
    public class RentalFileWriter
    {
        private readonly string filePath;
        
        /// <summary>
        /// Initializes a new instance of the RentalFileWriter with the specified file path
        /// </summary>
        /// <param name="path">The path to the CSV file where rental data will be saved</param>
        public RentalFileWriter(string path) => filePath = path;

        /// <summary>
        /// Saves rental list to CSV file
        /// </summary>
        /// <param name="rentals">List of rentals to save</param>
        public void SaveRentals(List<Rental> rentals)
        {
            var lines = new List<string>
            {
                "Id,CarId,CustomerName,StartDate,ExpectedReturn,ActualReturn,DailyRate,TotalCost,Status"
            };
            
            foreach (var rental in rentals)
            {
                var actualReturn = rental.ActualReturn?.ToString("yyyy-MM-dd") ?? "";
                var totalCost = rental.TotalCost?.ToString("F2", CultureInfo.InvariantCulture) ?? "";
                
                lines.Add($"{rental.Id},{rental.CarId},{rental.CustomerName}," +
                         $"{rental.StartDate:yyyy-MM-dd},{rental.ExpectedReturn:yyyy-MM-dd}," +
                         $"{actualReturn},{rental.DailyRate.ToString("F2", CultureInfo.InvariantCulture)},{totalCost},{rental.Status}");
            }
            
            File.WriteAllLines(filePath, lines);
        }
    }
}
