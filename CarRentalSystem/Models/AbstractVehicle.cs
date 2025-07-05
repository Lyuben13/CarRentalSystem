using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Models
{
    /// <summary>
    /// Abstract base class for all vehicles in the rental system
    /// </summary>
    public abstract class AbstractVehicle
    {
        /// <summary>
        /// Gets or sets the unique identifier of the vehicle
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the vehicle
        /// </summary>
        public string Make { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model name of the vehicle
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the manufacturing year of the vehicle
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the type/category of the vehicle (e.g., Sedan, SUV, etc.)
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets detailed information about the vehicle as a formatted string
        /// </summary>
        /// <returns>A string containing all vehicle details in a structured format</returns>
        public abstract string GetDetails();
    }
}
