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
        public int CurrentPlanetID {  get; set; }
        private int[] Consumables { get; set; } = new int[2];
        private int[] QuestItems { get; set; } = new int[1];

        public int GetConsumable(Consumables value)
        {
            return Consumables[(int)value];
        }

        public int GetQuestItem(QuestItemType value)
        {
            return QuestItems[(int)value];
        }

        private const string CURRENT_VEHICLE = "CurrentVehicle";
        private const string CURRENT_HEALTH = "CurrentHealth";
        private const string REPAIR_END_TIME = "RepairEndTime";
        private const string RATING = "Rating";
        private const string MONEY = "Money";
        private const string CURRENT_PLANET_ID = "CurrentPlanetID";
        private const string CONSUMABLES = "Consumables";
        private const string QUEST_ITEMS = "QuestItems";

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
            CurrentPlanetID = PlayerPrefs.GetInt(CURRENT_PLANET_ID, 1);
            LoadConsumables();
            LoadQuestItems();
        }

        public void SaveSettings() 
        {
            PlayerPrefs.SetInt(CURRENT_VEHICLE, (int)CurrentVehicle);
            PlayerPrefs.SetFloat(CURRENT_HEALTH, CurrentHealth);
            PlayerPrefs.SetString(REPAIR_END_TIME, RepairEndTime == null ? "" : RepairEndTime.ToString());
            PlayerPrefs.SetInt(RATING, Rating);
            PlayerPrefs.SetInt(MONEY, Money);
            PlayerPrefs.SetInt(CURRENT_PLANET_ID, CurrentPlanetID);
            SaveConsumables();
            SaveQuestItems();
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

        public void AddQuestItems(QuestItemType questItemType, int amount)
        {
            QuestItems[(int)questItemType] += amount;
        }

        public void RemoveQuestItems(QuestItemType questItemType, int amount)
        {
            QuestItems[(int)questItemType] -= amount;
        }

        private void SaveQuestItems()
        {
            var questItemsString = string.Join(";", QuestItems);
            PlayerPrefs.SetString(QUEST_ITEMS, questItemsString);
        }

        private void LoadQuestItems()
        {
            var questItemsString = PlayerPrefs.GetString(QUEST_ITEMS, string.Empty);
            if (questItemsString != string.Empty)
                QuestItems = questItemsString.Split(';').Select(s => int.Parse(s)).ToArray();
        }
    }
}
