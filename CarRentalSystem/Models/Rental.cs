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
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public DateTime? ActualReturn { get; set; }
        public decimal DailyRate { get; set; } = 35.0m;
        public decimal? TotalCost { get; set; }
        public string Status { get; set; } = "Active"; // Active, Completed, Overdue

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
            return Status == "Active" && DateTime.Now > ExpectedReturn;
        }

        /// <summary>
        /// Gets rental details as a formatted string
        /// </summary>
        /// <returns>Formatted rental information</returns>
        public string GetDetails()
        {
            var cost = CalculateCost();
            return $"Rental {Id}: Car {CarId} to {CustomerName}, " +
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
            Status = "Completed";
            TotalCost = CalculateCost();
        }
    }
}
