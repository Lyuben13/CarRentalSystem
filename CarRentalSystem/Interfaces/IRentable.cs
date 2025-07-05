using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalSystem.Models;

namespace CarRentalSystem.Interfaces
{
    /// <summary>
    /// Interface for objects that can be rented in the car rental system
    /// </summary>
    public interface IRentable
    {
        /// <summary>
        /// Rents the item to a specified customer
        /// </summary>
        /// <param name="renter">Name of the person renting the item</param>
        void Rent(string renter);

        /// <summary>
        /// Returns the item and makes it available for future rentals
        /// </summary>
        void Return();
    }
}
