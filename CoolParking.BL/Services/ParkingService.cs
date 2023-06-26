// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Transactions;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService, IDisposable
    {
        private bool _isDisposed;
        private ILogService _logService;
        private ITimerService _withdrawTimer;
        private ITimerService _loggTimer;
        private Parking _parking;
        private ConcurrentBag<TransactionInfo> _transactionsCache;
        private StringBuilder sb;

        public ParkingService(ITimerService withdrawTimer, ITimerService loggTimer, ILogService logService)
        {
            _withdrawTimer = withdrawTimer;
            _loggTimer = loggTimer;
            _logService = logService;
            _parking = Parking.GetInstance;
            sb = new StringBuilder();
            _transactionsCache = new ConcurrentBag<TransactionInfo>();
            InvokeTimers();
            
        }

        private void InvokeTimers() 
        {
            _withdrawTimer.Interval = Settings.PaymentPeriod;
            _loggTimer.Interval = Settings.LoggingPeriod;

            _withdrawTimer.Elapsed += new ElapsedEventHandler(OnPaymentEvent);
            _withdrawTimer.Start();

            _loggTimer.Elapsed += new ElapsedEventHandler(OnTransactionsLog);
            _loggTimer.Start();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (GetFreePlaces() <= 0)
            {
                throw new InvalidOperationException();
                
            }
            _parking.AddVehicle(vehicle);

        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposed) 
        {
            if (!_isDisposed) 
            {
                if (disposed) 
                {
                    _withdrawTimer.Stop();
                    _loggTimer.Stop();
                    _parking.Dispose();
                    _transactionsCache.Clear();
                }
            }
            _isDisposed = true;
        }

        public decimal GetBalance()
        {
            return _parking.Balance;
        }

        public int GetCapacity()
        {
            return Settings.ParkingCapacity;
        }

        public int GetFreePlaces()
        {
            int capacity = Settings.ParkingCapacity;

            if (_parking.GetVehicles().Count() > 0)
            {
                capacity -= _parking.GetVehicles().Count;
            }
            return capacity;
        }

        public TransactionInfo[] GetLastParkingTransactions()
        {
            int cnt = 0;
            TransactionInfo[] output = new TransactionInfo[_transactionsCache.Count()];

            foreach (var item in _transactionsCache)
            {

                output[cnt++] = item;
            }

            return output;
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return new ReadOnlyCollection<Vehicle>(_parking.GetVehicles());
        }

        public string ReadFromLog()
        {
            return _logService.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            var vehicle = FindVehicleById(vehicleId);
            Regex r = new Regex(@"([A-Z][A-Z])-\d{4}-([A-Z][A-Z])");

            if (vehicle == null)
            {
                throw new ArgumentException();
            }
            else
            if (!r.IsMatch(vehicleId))
            {
            throw new FormatException();
            }

                _parking.RemoveVehicle(vehicle);  
            
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            var vehicle = FindVehicleById(vehicleId);

            if (sum < 0 || vehicle == null)
            {
                throw new ArgumentException();
            }
            vehicle.Balance = decimal.Add(vehicle.Balance, sum);
        }

        //I'd add this method to IParkingService interface
        private Vehicle FindVehicleById(string vehicleId) 
        {
          return _parking.GetVehicles().FirstOrDefault(v => v.Id == vehicleId);
        }

        private void OnPaymentEvent(object sender, ElapsedEventArgs e)
        {
            foreach (var car in _parking.GetVehicles())
            {
                decimal tarif;
                Settings.Tariffs.TryGetValue(car.Type, out tarif);

                if (car.Balance < tarif & car.Balance > 0)
                {
                    var diff = decimal.Subtract(car.Balance, tarif);
                    tarif = decimal.Add(tarif, decimal.Multiply(diff, Settings.PenaltyCoefficient));
                }
                else
                if(car.Balance <= 0)
                {
                    tarif = decimal.Multiply(tarif,Settings.PenaltyCoefficient);
                }

                car.Balance =  decimal.Subtract(car.Balance, tarif);
                _parking.Balance = decimal.Add(_parking.Balance, tarif);

                _transactionsCache.Add(new TransactionInfo { TarnsactionTime = DateTime.Now, VehicleId = car.Id, Sum = tarif });

            }
        }

        private void OnTransactionsLog(object sender, ElapsedEventArgs e)
        {
            foreach (var item in _transactionsCache)
            {
                sb.AppendLine($"{item.TarnsactionTime}/{item.VehicleId}/{item.Sum}|");
            }

            _logService.Write(sb.ToString());
            sb.Clear();
            _transactionsCache.Clear();
        }
    }
}