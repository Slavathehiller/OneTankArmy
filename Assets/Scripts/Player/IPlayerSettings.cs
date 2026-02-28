using Assets.Vehicles;
using System;
using System.Collections.Generic;

namespace Assets.Player
{
    public interface IPlayerSettings
    {
        public VehicleType CurrentVehicle { get; set; }
        public float CurrentHealth { get; set; }
        public DateTime? RepairEndTime { get; set; }
        public int Raiting { get; set; }
        public void LoadSettings();
        public void SaveSettings();
    }
}
