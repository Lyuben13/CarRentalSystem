using System;
using System.Collections.Generic;
using System.Linq;
using CarRentalSystem.Interfaces;
using CarRentalSystem.Models;


namespace CarRentalSystem.Interfaces
{
    /// <summary>
    /// Interface for objects that support search operations
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// Gets a car by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the car</param>
        /// <returns>The car with the specified ID, or null if not found</returns>
        Car? GetCarById(int id);

        /// <summary>
        /// Searches for cars by model name
        /// </summary>
        /// <param name="model">The model name to search for (case-insensitive)</param>
        /// <returns>List of cars matching the model criteria</returns>
        List<Car> SearchByModel(string model);
    }
}
