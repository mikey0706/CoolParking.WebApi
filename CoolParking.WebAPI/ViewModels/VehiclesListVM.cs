using CoolParking.BL.Models;
using System.Collections.ObjectModel;

namespace CoolParking.WebAPI.ViewModels
{
    public class VehiclesListVM
    {
        public ReadOnlyCollection<Vehicle> Vehicles { get; set; }
    }
}
