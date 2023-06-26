// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.
using Fare;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CoolParking.BL.Models
{
    public class Vehicle 
    {
        public string Id { get; set; }
        public VehicleType Type { get; set; }
        public decimal Balance { get; set; }

        private static string _expression = @"([A-Z][A-Z])-\d{4}-([A-Z][A-Z])";

        public Vehicle(string id, VehicleType type, decimal balance)
        {
            Regex regex = new Regex(_expression);
            if (!regex.IsMatch(id) || balance < 0)
            {
                throw new ArgumentException();
            }
            Id = id;
            Type = type;
            Balance = balance;
        }

        public static string GenerateRandomRegistrationPlateNumber()
        {
            Xeger xEger = new Xeger(_expression);
            return xEger.Generate();
        }

    }
}