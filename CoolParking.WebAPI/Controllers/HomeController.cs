using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Timers;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IParkingService _parkingService { get; set; }

        public HomeController(IParkingService parkingService) 
        {
        _parkingService = parkingService;
        }

        [HttpGet("/parking/balance")]
        public IActionResult GetParkingBalance()
        {
            return Ok(_parkingService.GetBalance());
        }

        [HttpGet("/parking/capacity")]
        public IActionResult GetParkingCapacity()
        {
            return Ok(_parkingService.GetCapacity());
        }

        [HttpGet("/parking/freePlaces")]
        public IActionResult GetParkingFreePlaces()
        {
            return Ok(_parkingService.GetFreePlaces());
        }

        [HttpPost("/vehicles")]
        public IActionResult AddVehicle([FromBody] VehicleAddVM data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vehicle = new Vehicle(data.Id, data.Type, data.Balance);  //Decided not to implement Automapper in this task

                    _parkingService.AddVehicle(vehicle);
                    return Created("api/parking/vehicles/", vehicle);
                }
                return BadRequest("Data is invalid");
            }
            catch (ArgumentException)
            {
                return BadRequest($"A vehicle with a license plate: {data.Id} already exissts.");
            }
            catch (InvalidOperationException) 
            {
                return BadRequest("There is no place on the parking.");
            }
        }

        [HttpGet("/vehicles")]
        public IActionResult GetVehicles()
        {
            var output = new VehiclesListVM();
            output.Vehicles = _parkingService.GetVehicles();
            return Ok(output);
        }

        [HttpGet("/parking/vehicles/{id}")]
        public IActionResult GetVehicleById(string id)
        {
            //There is no FindById method defined by the IParkingService interface
            //Otherwise I'd perform this action in the ParkingService class

            var car = _parkingService.GetVehicles().FirstOrDefault(v => v.Id.Equals(id));

            if (car != null)
            {
                return Ok(car);
            }
            return NotFound($"A vehicle with {id} wasn't found.");
        }



        [HttpDelete("/vehicles/{id}")]
        public IActionResult DeleteVehicleById(string id)
        {
            try
            {
               _parkingService.RemoveVehicle(id);
               return NoContent();
            }
            catch (FormatException) 
            {
                return BadRequest("Wrong field format");
            }
            catch (ArgumentException)
            {
                return NotFound("Vehicle not found");
            }
            catch (InvalidOperationException)
            {
                return NotFound("You can't withdraw a car with negative balance.");
            }
        }

        [HttpGet("/transactions/last")]
        public IActionResult GetLastTransactions()
        {
            return Ok(_parkingService.GetLastParkingTransactions());
        }

        [HttpGet("/transactions/all")]
        public IActionResult GetAllTransactions()
        {
            string output = _parkingService.ReadFromLog();
            if (!string.IsNullOrEmpty(output))
            {
                return Ok(output);
            }
            return NotFound("There is no transactions at the moment.");
        }

        [HttpPut("/transactions/topUpVehicle")]
        public IActionResult TopUpVehicle([FromBody] VehicleToUpVM data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _parkingService.TopUpVehicle(data.Id, data.Sum);
                    return Ok();
                }
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return NotFound($"There is no vehicle with {data.Id}");
            }
        }
    }
}
