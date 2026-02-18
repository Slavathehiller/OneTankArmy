using Assets.Player;
using Assets.Vehicles;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSettings : IPlayerSettings
    {
        public VehicleType CurrentVehicle { get; set; }

        private float _currentHealth;
        public float CurrentHealth 
        { 
            get => _currentHealth;
            set 
            { 
                _currentHealth = value; 
            }
        }
        public DateTime? RepairEndTime { get; set; }

        private const string CURRENT_VEHICLE = "CurrentVehicle";
        private const string CURRENT_HEALTH = "CurrentHealth";
        private const string REPAIR_END_TIME = "RepairEndTime";

        public void ChangeVechicle(VehicleType vehicle)
        {
            CurrentVehicle = vehicle;
            SaveSettings();
        }

        public PlayerSettings()
        {
            LoadSettings();
        }
        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey(CURRENT_VEHICLE))
                CurrentVehicle = (VehicleType)PlayerPrefs.GetInt(CURRENT_VEHICLE);
            else
                CurrentVehicle = VehicleType.Beetle;

            CurrentHealth = PlayerPrefs.GetFloat(CURRENT_HEALTH, float.MinValue);

            var repairEndTime = PlayerPrefs.GetString(REPAIR_END_TIME, "");
            RepairEndTime = repairEndTime == "" ? null : DateTime.Parse(repairEndTime);
        }

        public void SaveSettings() 
        {
            PlayerPrefs.SetInt(CURRENT_VEHICLE, (int)CurrentVehicle);
            PlayerPrefs.SetFloat(CURRENT_HEALTH, CurrentHealth);
            PlayerPrefs.SetString(REPAIR_END_TIME, RepairEndTime == null ? "" : RepairEndTime.ToString());
        }
    }
}
