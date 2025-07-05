using System.Collections.Generic;
using CarRentalSystem.Models;

namespace CarRentalSystem.Interfaces
{
    /// <summary>
    /// Interface for file operations to support data persistence
    /// </summary>
    /// <typeparam name="T">Type of data to be stored</typeparam>
    public interface IFileOperations<T>
    {
        /// <summary>
        /// Loads data from file
        /// </summary>
        /// <returns>List of loaded items</returns>
        List<T> Load();
        
        /// <summary>
        /// Saves data to file
        /// </summary>
        /// <param name="items">Items to save</param>
        void Save(List<T> items);
    }
} 