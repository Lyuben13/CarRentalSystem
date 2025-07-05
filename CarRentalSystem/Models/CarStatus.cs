namespace CarRentalSystem.Models
{
    /// <summary>
    /// Represents the status of a car in the rental system
    /// </summary>
    public enum CarStatus
    {
        /// <summary>
        /// Car is available for rental
        /// </summary>
        Available,

        /// <summary>
        /// Car is currently rented to a customer
        /// </summary>
        Rented,

        /// <summary>
        /// Car has been removed from the fleet (sold, scrapped, etc.)
        /// </summary>
        Removed,

        /// <summary>
        /// Car is undergoing maintenance or repairs
        /// </summary>
        Maintenance
    }
} 