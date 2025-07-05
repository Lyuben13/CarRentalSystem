using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Models
{
    /// <summary>
    /// Represents a customer in the car rental system
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets customer details as a formatted string
        /// </summary>
        /// <returns>Formatted customer information</returns>
        public string GetDetails()
        {
            return $"ID: {Id}, Name: {Name}, License: {LicenseNumber}, Email: {Email}, Phone: {Phone}, Active: {IsActive}";
        }

        /// <summary>
        /// Validates customer data
        /// </summary>
        /// <returns>True if customer data is valid</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(LicenseNumber) &&
                   !string.IsNullOrWhiteSpace(Email);
        }
    }
}
