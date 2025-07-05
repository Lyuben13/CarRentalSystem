using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalSystem.Models;

namespace CarRentalSystem.Interfaces
{
    public interface ISearchable
    {
        Car SearchById(int id);
        List<Car> SearchByModel(string model);
    }
}
