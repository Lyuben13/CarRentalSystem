using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    public class Car : AbstractVehicle, IRentable
    {
        public string Status { get; set; } = "Available";
        public string CurrentRenter { get; set; } = "";

        public void Rent(string renter)
        {
            if (Status == "Available")
            {
                Status = "Rented";
                CurrentRenter = renter;
            }
        }

        public void Return()
        {
            Status = "Available";
            CurrentRenter = "";
        }

        public override string GetDetails()
        {
            return $"{Id},{Make},{Model},{Year},{Type},{Status},{CurrentRenter}";
        }
    }
}
