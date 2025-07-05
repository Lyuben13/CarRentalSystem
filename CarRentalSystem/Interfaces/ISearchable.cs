using System;
using System.Collections.Generic;
using System.Linq;
using CarRentalSystem.Interfaces;
using CarRentalSystem.Models;


namespace CarRentalSystem.Interfaces
{
    public interface ISearchable
    {
        Car GetCarById(int id);
        List<Car> SearchByModel(string model);
        List<Car> SearchByStatus(string status);
        //Car SearchById(int id);
        //List<Car> SearchByModel(string model);
    }
}
