using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Models
{
    /// <summary>
    /// Represents a car rental transaction
    /// </summary>
    public class Rental
    {
        /// <summary>
        /// Gets or sets the unique identifier of the rental transaction
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the car being rented
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// Gets or sets the optional customer ID reference
        /// </summary>
        public int? CustomerId { get; set; } // Optional reference to customer record

        /// <summary>
        /// Gets or sets the name of the customer renting the car
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the rental started
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the expected return date for the rental
        /// </summary>
        public DateTime ExpectedReturn { get; set; }

        /// <summary>
        /// Gets or sets the actual return date (null if not yet returned)
        /// </summary>
        public DateTime? ActualReturn { get; set; }

        /// <summary>
        /// Gets or sets the daily rental rate in currency units
        /// </summary>
        public decimal DailyRate { get; set; } = 35.0m;

        /// <summary>
        /// Gets or sets the total cost of the rental (calculated when completed)
        /// </summary>
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the current status of the rental transaction
        /// </summary>
        public RentalStatus Status { get; set; } = RentalStatus.Active;

        /// <summary>
        /// Calculates the total cost of the rental
        /// </summary>
        /// <returns>Total cost in decimal</returns>
        public decimal CalculateCost()
        {
            var endDate = ActualReturn ?? ExpectedReturn;
            var days = (endDate - StartDate).Days;
            return Math.Max(1, days) * DailyRate;
        }

        /// <summary>
        /// Checks if the rental is overdue
        /// </summary>
        /// <returns>True if rental is overdue</returns>
        public bool IsOverdue()
        {
            return Status == RentalStatus.Active && DateTime.Now > ExpectedReturn;
        }

        /// <summary>
        /// Gets rental details as a formatted string
        /// </summary>
        /// <returns>Formatted rental information</returns>
        public string GetDetails()
        {
            var cost = CalculateCost();
            var customerInfo = CustomerId.HasValue ? $"Customer {CustomerId} ({CustomerName})" : CustomerName;
            return $"Rental {Id}: Car {CarId} to {customerInfo}, " +
                   $"From {StartDate:yyyy-MM-dd} to {ExpectedReturn:yyyy-MM-dd}, " +
                   $"Status: {Status}, Cost: ${cost:F2}";
        }

        /// <summary>
        /// Completes the rental and calculates final cost
        /// </summary>
        /// <param name="actualReturnDate">Actual return date</param>
        public void Complete(DateTime actualReturnDate)
        {
            ActualReturn = actualReturnDate;
            Status = RentalStatus.Completed;
            TotalCost = CalculateCost();
        }
    }
}
