// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
using CoolParking.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CoolParking.BL.Models
{
    internal class Parking : IDisposable
    {
        private bool _isDisposed;
        private static Parking _instance;

        private static object _locker = new object();
        public decimal Balance { get; set; }

        private List<Vehicle> _vehicles;
        

        private Parking() 
        {
            _vehicles = new List<Vehicle>();
        }

        public static Parking GetInstance 
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new Parking();
                    }
                }
              return _instance;
            }
             
        }

        public void AddVehicle(Vehicle vehicle) 
        {

            if (_vehicles.FirstOrDefault(v => v.Id.Equals(vehicle.Id)) != null)
            {
                throw new ArgumentException();
            }
                _vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle) 
        {

            if (vehicle.Balance < 0)
            {
                throw new InvalidOperationException();
            }
            _vehicles.Remove(vehicle);
        }


        public ReadOnlyCollection<Vehicle> GetVehicles() 
        {
            return _vehicles.AsReadOnly();
        }

        public void Dispose()
        {
            Balance = 0;
            _vehicles.Clear();
        }
    }
}
