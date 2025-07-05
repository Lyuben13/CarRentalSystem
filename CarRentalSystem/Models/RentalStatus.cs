namespace CarRentalSystem.Models
{
    /// <summary>
    /// Represents the status of a rental transaction
    /// </summary>
    public enum RentalStatus
    {
        /// <summary>
        /// Rental is currently active and ongoing
        /// </summary>
        Active,

        /// <summary>
        /// Rental has been completed and car returned
        /// </summary>
        Completed,

        /// <summary>
        /// Rental is overdue (past expected return date)
        /// </summary>
        Overdue
    }
} 