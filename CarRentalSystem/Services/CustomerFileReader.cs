using System;
using System.Collections.Generic;
using System.IO;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    /// <summary>
    /// Reads customer records from customers.csv file
    /// </summary>
    public class CustomerFileReader
    {
        private readonly string filePath;
        
        /// <summary>
        /// Initializes a new instance of the CustomerFileReader with the specified file path
        /// </summary>
        /// <param name="path">The path to the CSV file containing customer data</param>
        public CustomerFileReader(string path) => filePath = path;

        /// <summary>
        /// Loads all customers from CSV file
        /// </summary>
        /// <returns>List of customer records</returns>
        public List<Customer> LoadCustomers()
        {
            var customers = new List<Customer>();
            
            if (!File.Exists(filePath))
                return customers;

            var lines = File.ReadAllLines(filePath);
            
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                try
                {
                    var customer = new Customer
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        LicenseNumber = parts[2],
                        Email = parts[3],
                        Phone = parts[4],
                        RegistrationDate = DateTime.Parse(parts[5]),
                        IsActive = bool.Parse(parts[6])
                    };
                    customers.Add(customer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading customer line {i + 1}: {ex.Message}");
                }
            }
            
            return customers;
        }
    }
} 