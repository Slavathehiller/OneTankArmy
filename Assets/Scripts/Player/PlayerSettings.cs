using Assets.Player;
using Assets.Vehicles;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public enum Consumables
    {
        NotDefined = -1,
        NanoRepairKit = 0,
        Fuel = 1
    }
    public class PlayerSettings : IPlayerSettings
    {
        public VehicleType CurrentVehicle { get; set; }
        public float CurrentHealth { get; set; }
        public DateTime? RepairEndTime { get; set; }
        public int Rating {  get; set; }
        public int Money { get; set; }
        private int[] Consumables { get; set; } = new int[2];

        public int GetConsumable(Consumables value)
        {
            return Consumables[(int)value];
        }

        private void SetConsumable(Consumables value, int amount)
        {
            Consumables[(int)value] = amount;
        }

        private const string CURRENT_VEHICLE = "CurrentVehicle";
        private const string CURRENT_HEALTH = "CurrentHealth";
        private const string REPAIR_END_TIME = "RepairEndTime";
        private const string RATING = "Rating";
        private const string MONEY = "Money";
        private const string CONSUMABLES = "Consumables";

        //public void ChangeVechicle(VehicleType vehicle)
        //{
        //    CurrentVehicle = vehicle;
        //    SaveSettings();
        //}

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
            Rating = PlayerPrefs.GetInt(RATING, 0);
            Money = PlayerPrefs.GetInt(MONEY, 0);
            LoadConsumables();
        }

        public void SaveSettings() 
        {
            PlayerPrefs.SetInt(CURRENT_VEHICLE, (int)CurrentVehicle);
            PlayerPrefs.SetFloat(CURRENT_HEALTH, CurrentHealth);
            PlayerPrefs.SetString(REPAIR_END_TIME, RepairEndTime == null ? "" : RepairEndTime.ToString());
            PlayerPrefs.SetInt(RATING, Rating);
            PlayerPrefs.SetInt(MONEY, Money);
            SaveConsumables();
        }

        public void AddConsumable(Consumables value)
        {
            Consumables[(int)value]++;
        }

        public void RemoveConsumable(Consumables value)
        {
            Consumables[(int)value]--;
        }

        private void SaveConsumables()
        {
            var consumablesString = string.Join(";", Consumables);
            PlayerPrefs.SetString(CONSUMABLES, consumablesString);
        }

        private void LoadConsumables()
        {
            var consumablesString = PlayerPrefs.GetString(CONSUMABLES, string.Empty);
            if (consumablesString != string.Empty)
                Consumables = consumablesString.Split(';').Select(s => int.Parse(s)).ToArray();
        }
    }
}
