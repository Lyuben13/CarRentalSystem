using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    /// <summary>
    /// Represents a car in the rental system
    /// </summary>
    public class Car : AbstractVehicle, IRentable
    {
        public string Status { get; set; } = "Available";
        public string CurrentRenter { get; set; } = "";
        public decimal DailyRate { get; set; } = 35.0m;
        public string LicensePlate { get; set; } = "";
        public int Mileage { get; set; } = 0;
        public DateTime LastMaintenance { get; set; } = DateTime.Now;

        /// <summary>
        /// Rents the car to a customer
        /// </summary>
        /// <param name="renter">Name of the renter</param>
        public void Rent(string renter)
        {
            if (Status == "Available")
            {
                Status = "Rented";
                CurrentRenter = renter;
            }
        }

        /// <summary>
        /// Returns the car and makes it available again
        /// </summary>
        public void Return()
        {
            Status = "Available";
            CurrentRenter = "";
        }

        /// <summary>
        /// Checks if the car needs maintenance
        /// </summary>
        /// <returns>True if maintenance is needed</returns>
        public bool NeedsMaintenance()
        {
            return (DateTime.Now - LastMaintenance).Days > 90 || Mileage > 10000;
        }

        /// <summary>
        /// Validates car data
        /// </summary>
        /// <returns>True if car data is valid</returns>
        public bool IsValid()
        {
            return Id > 0 && 
                   !string.IsNullOrWhiteSpace(Make) && 
                   !string.IsNullOrWhiteSpace(Model) && 
                   Year > 1900 && Year <= DateTime.Now.Year + 1 &&
                   !string.IsNullOrWhiteSpace(Type);
        }

        /// <summary>
        /// Gets detailed information about the car
        /// </summary>
        /// <returns>Formatted car details</returns>
        public override string GetDetails()
        {
            return $"{Id},{Make},{Model},{Year},{Type},{Status},{CurrentRenter},{LicensePlate},{Mileage},{DailyRate:F2}";
        }

        /// <summary>
        /// Gets a user-friendly display string
        /// </summary>
        /// <returns>Formatted display string</returns>
        public string GetDisplayInfo()
        {
            return $"ID: {Id} | {Make} {Model} ({Year}) | {Type} | Status: {Status} | Rate: ${DailyRate:F2}/day";
        }
    }
}
