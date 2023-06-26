using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.BL.Services;
using System.ComponentModel.DataAnnotations;

namespace CoolParking.WebAPI.ViewModels
{
    public class VehicleAddVM
    {
        [Required]
        [RegularExpression(@"([A-Z][A-Z])-\d{4}-([A-Z][A-Z])")]
        public string Id { get; set; }
        [Required]
        public VehicleType Type { get; set; }
        public decimal Balance { get; set; }

    }
}
