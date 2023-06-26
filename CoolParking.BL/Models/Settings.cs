// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.
using System;
using System.Collections.Generic;

namespace CoolParking.BL.Models
{
    public static class Settings
    {
        public static decimal InitialParkingBalance = 0;
        public static int ParkingCapacity = 10;
        public static double PaymentPeriod = 5000;
        public static double LoggingPeriod = 60000;

        public static Dictionary<VehicleType, decimal> Tariffs = new Dictionary<VehicleType, decimal>()
        {
            { VehicleType.PassengerCar, 2 },
            { VehicleType.Truck, 5 },
            { VehicleType.Bus, 3.5m },
            { VehicleType.Motorcycle, 1 }
        };


        public static decimal PenaltyCoefficient = 2.5m;

    }
}