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
        /// <summary>
        /// Gets or sets the unique identifier of the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the customer
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's driver's license number
        /// </summary>
        public string LicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's phone number
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the customer was registered in the system
        /// </summary>
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets whether the customer account is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets customer details as a formatted string
        /// </summary>
        /// <returns>Formatted customer information</returns>
        public string GetDetails()
        {
            return $"Customer {Id}: {Name} | License: {LicenseNumber} | Email: {Email} | Phone: {Phone} | Active: {IsActive}";
        }

        /// <summary>
        /// Gets a user-friendly display string for the customer
        /// </summary>
        /// <returns>Formatted display string</returns>
        public string GetDisplayInfo()
        {
            var status = IsActive ? "Active" : "Inactive";
            return $"ID: {Id} | {Name} | License: {LicenseNumber} | Email: {Email} | Phone: {Phone} | Status: {status} | Registered: {RegistrationDate:yyyy-MM-dd}";
        }

        /// <summary>
        /// Validates customer data
        /// </summary>
        /// <returns>True if customer data is valid</returns>
        public bool IsValid()
        {
            return Id > 0 && 
                   !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(LicenseNumber) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Phone);
        }
    }
}
