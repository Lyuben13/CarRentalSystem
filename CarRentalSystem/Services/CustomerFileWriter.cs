using System.Collections.Generic;
using System.IO;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Writes customer list to customers.csv file
    /// </summary>
    public class CustomerFileWriter
    {
        private readonly string filePath;
        
        /// <summary>
        /// Initializes a new instance of the CustomerFileWriter with the specified file path
        /// </summary>
        /// <param name="path">The path to the CSV file where customer data will be saved</param>
        public CustomerFileWriter(string path) => filePath = path;

        /// <summary>
        /// Saves customer list to CSV file
        /// </summary>
        /// <param name="customers">List of customers to save</param>
        public void SaveCustomers(List<Customer> customers)
        {
            var lines = new List<string>
            {
                "Id,Name,LicenseNumber,Email,Phone,RegistrationDate,IsActive"
            };
            
            foreach (var customer in customers)
            {
                lines.Add($"{customer.Id},{customer.Name},{customer.LicenseNumber}," +
                         $"{customer.Email},{customer.Phone}," +
                         $"{customer.RegistrationDate:yyyy-MM-dd},{customer.IsActive}");
            }
            
            File.WriteAllLines(filePath, lines);
        }
    }
} 